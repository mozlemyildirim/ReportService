using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class UserDeviceDB
    {
        private static UserDeviceDB instance;
        public static UserDeviceDB GetInstance()
        {
            if (instance == null)
            {
                instance = new UserDeviceDB();
            }
            return instance;
        }
        public UserDevice AddNewUserDevice(UserDevice _UserDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.UserDevice.Add(_UserDevice);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _UserDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<UserDevice> GetAllUserDevices()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserDeviceList = context.UserDevice.ToList();
                    return UserDeviceList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UserDevice GetUserDeviceById(int _UserDeviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserDevice = context.UserDevice.FirstOrDefault(a => a.Id == _UserDeviceId);
                    return UserDevice != null ? UserDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteUserDevice(int _userId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var userDevices = context.UserDevice.Where(a => a.UserId == _userId).ToList();
                    if (userDevices != null)
                    {
                        context.UserDevice.RemoveRange(userDevices);
                        int numOfDeleted = context.SaveChanges();
                        return numOfDeleted > 0;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void UpdateObject(UserDevice _newUserDevice, ref UserDevice _oldUserDevice)
        {
            try
            {

                foreach (PropertyInfo UserDevicePropInfo in _newUserDevice.GetType().GetProperties().ToList())
                {
                    _oldUserDevice.GetType().GetProperty(UserDevicePropInfo.Name).SetValue(_oldUserDevice, _newUserDevice.GetType().GetProperty(UserDevicePropInfo.Name).GetValue(_newUserDevice));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public UserDevice UpdateUserDevice(UserDevice _UserDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldUserDevice = context.UserDevice.FirstOrDefault(u => u.Id == _UserDevice.Id);
                    if (oldUserDevice != null)
                    {
                        UpdateObject(_UserDevice, ref oldUserDevice);
                        var numberOfUpdatedUserDevice = context.SaveChanges();
                        return numberOfUpdatedUserDevice > 0 ? _UserDevice : null;
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public List<Device> GetAllUserDevicesByUserIdToLeft(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var allDevices = DeviceDB.GetInstance().GetAllDBDevices();
                    if (_userObj.Id == 774)
                    {
                        return allDevices;
                    }
                    else
                    {
                        var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                        var myDevices = allDevices.Where(x => x.CompanyId == myCompanyId).ToList();
                        return myDevices;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Device> GetAllUserDevicesByUserIdToRight(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {

                    var allDevices = DeviceDB.GetInstance().GetAllDBDevices();
                    var UserDevices = context.UserDevice.Where(x => x.UserId == _userObj.Id).ToList();
                    var UserDevicesIds = UserDevices.Select(x => x.DeviceId).ToList();
                    var devices = allDevices.Where(x => UserDevicesIds.Contains(x.Id)).ToList();
                    return devices;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserDevice> GetUserDeviceIdsByGroupId(int _userId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserDevice = context.UserDevice.Where(x => x.UserId == _userId).ToList();
                    return UserDevice;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public bool DeleteAllUserDeviceByGroupId(int _userId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserDevices = context.UserDevice.Where(x => x.UserId == _userId).ToList();
                    context.UserDevice.RemoveRange(UserDevices);
                    int numberOfDeleted = context.SaveChanges();

                    return numberOfDeleted > 0;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<DeviceInfoForMap> GetUserDevicesByUserId(int _userObjId)
        {
            using (var context = new VeraEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {

                            var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObjId);
                            var company = context.Company.FirstOrDefault(x => x.Id == companyUser.CompanyId);
                            var altSirketler = context.Company.Where(x => x.MainCompanyId == company.Id).ToList();
                            var altSirketlerIds = altSirketler.Select(x => x.Id).ToList();
                            var userDevices = new List<Device>();
                            if (companyUser.IsCompanyAdmin)
                            {
                                userDevices = context.Device.Where(x => x.CompanyId == companyUser.CompanyId || altSirketlerIds.Contains(x.CompanyId)).ToList();
                            }
                            if (companyUser.IsCompanyAdmin == false)
                            {
                                var userDevicesIds = context.UserDevice.Where(x => x.UserId == _userObjId).Select(x => x.DeviceId).ToList();
                                userDevices = context.Device.Where(x => userDevicesIds.Contains(x.Id)).ToList();
                            }
                            var deviceIds = userDevices.Where(x=>x.LastDeviceDataId!=null).Select(x => x.Id).ToList();
                            var returnList = new List<DeviceInfoForMap>();
                            for (int i = 0; i < deviceIds.Count; i++)
                            {
                                var deviceId = deviceIds[i];
                           
                                    cmd.CommandText = $"EXEC [dbo].[USP_GetDevicesForDisplayOnMap] {deviceId}";

                                    var reader = cmd.ExecuteReader();

                                    StringBuilder sb = new StringBuilder();

                                    while (reader.Read())
                                        sb.Append(reader.GetString(0));

                                    var json = sb.ToString();

                                    var list = JsonConvert.DeserializeObject<List<DeviceInfoObject>>(json);
                                    foreach (var item in list)
                                    {
                                    returnList.Add(new DeviceInfoForMap()
                                    {
                                        Altitude = (decimal)item.Altitude,
                                        CarPlateNumber = item.CarPlateNumber,
                                        Date = item.Date.ToString(),
                                        DeviceId = item.DeviceId,
                                        DirectionDegree = (decimal)item.DirectionDegree,
                                        IoStatus = item.IoStatus.EndsWith("1") ? "Açık" : "Kapalı",
                                        KmPerHour = item.KmPerHour,
                                        lat = (decimal)item.Latitude,
                                        lng = (decimal)item.Longitude,
                                        Location = item.Location,
                                        TotaLkm = item.TotalKmDaily,
                                        ActivityTime=item.ActivityTime
                                    }) ;
                                    }
                                    reader.Close();
                                }
                             

                            return returnList;
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
        }
    }
    public class DeviceInfoObject
    {
        public int DeviceId { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CarPlateNumber { get; set; }
        public double Altitude { get; set; }
        public DateTime Date { get; set; }
        public string IoStatus { get; set; }
        public int TotalKmDaily { get; set; }
        public int KmPerHour { get; set; }
        public double DirectionDegree { get; set; }
        public int ActivityTime { get; set; }
    }
}
