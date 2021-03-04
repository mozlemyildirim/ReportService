using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class AreaDeviceDB
    {
        private static AreaDeviceDB instance;
        public static AreaDeviceDB GetInstance()
        {
            if (instance == null)
            {
                instance = new AreaDeviceDB();
            }
            return instance;
        }
        public AreaDevice AddNewAreaDevice(AreaDevice _AreaDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.AreaDevice.Add(_AreaDevice);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _AreaDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<AreaDevice> GetAllAreaDevicees()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var AreaDeviceList = context.AreaDevice.ToList();
                    return AreaDeviceList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AreaDevice GetAreaDeviceById(int _AreaDeviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var AreaDevice = context.AreaDevice.FirstOrDefault(a => a.Id == _AreaDeviceId && a.Status == 1);
                    return AreaDevice != null ? AreaDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteAreaDevice(int _AreaDeviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var areaDevice = context.AreaDevice.FirstOrDefault(a => a.DeviceId == _AreaDeviceId);
                    if (areaDevice != null)
                    {
                        context.AreaDevice.Remove(areaDevice);
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
        private void UpdateObject(AreaDevice _newAreaDevice, ref AreaDevice _oldAreaDevice)
        {
            try
            {

                foreach (PropertyInfo AreaDevicePropInfo in _newAreaDevice.GetType().GetProperties().ToList())
                {
                    _oldAreaDevice.GetType().GetProperty(AreaDevicePropInfo.Name).SetValue(_oldAreaDevice, _newAreaDevice.GetType().GetProperty(AreaDevicePropInfo.Name).GetValue(_newAreaDevice));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public AreaDevice UpdateAreaDevice(AreaDevice _AreaDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldAreaDevice = context.AreaDevice.FirstOrDefault(u => u.Id == _AreaDevice.Id);
                    if (oldAreaDevice != null)
                    {
                        UpdateObject(_AreaDevice, ref oldAreaDevice);
                        var numberOfUpdatedAreaDevice = context.SaveChanges();
                        return numberOfUpdatedAreaDevice > 0 ? _AreaDevice : null;
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
        public bool DeleteAreaDevices(int _areaId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var areasToDelete = context.AreaDevice.Where(x => x.AreaId == _areaId).ToList();
                    if (areasToDelete != null)
                    {
                        context.AreaDevice.RemoveRange(areasToDelete);
                        int numOfDeleted = context.SaveChanges();
                        return numOfDeleted > 0;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<Device> GetAreaVehiclesByAreaId(int _areaId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var areaDevicesDeviceIds = context.AreaDevice.Where(x => x.AreaId == _areaId).Select(x => x.DeviceId).ToList();
                    var devices = context.Device.Where(x => areaDevicesDeviceIds.Contains(x.Id)).ToList();
                    if (devices != null)
                        return devices;
                    return null;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<AreaWithVehicle> GetAreasWithVehicles(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var company = CompanyUserDB.GetInstance().GetCompanyByCompanyUserId(_userObj);
                    var subCompaniesAndOwnCompanyIds = context.Company.Where(x => x.MainCompanyId == company.Id).Select(x => x.Id).ToList();
                    subCompaniesAndOwnCompanyIds.Add(company.Id);

                    var returnList = new List<AreaWithVehicle>();

                    var companyAreasAreaIds = context.CompanyArea.Where(x => subCompaniesAndOwnCompanyIds.Contains(x.CompanyId)).Select(x => x.AreaId).ToList();
                    var areaDevices = context.AreaDevice.Where(x => companyAreasAreaIds.Contains(x.AreaId)).ToList();

                    foreach (var item in areaDevices)
                    {
                        var areaName = context.Area.FirstOrDefault(x => x.Id == item.AreaId).Name;
                        var deviceCarPlateNumber = context.Device.FirstOrDefault(x => x.Id == item.DeviceId).CarPlateNumber;
                        returnList.Add(new AreaWithVehicle()
                        {
                            CarPlateNumber = deviceCarPlateNumber,
                            AreaName = areaName
                        });
                    }
                    if (returnList != null)
                        return returnList;
                    return null;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
    }
    public class AreaWithVehicle
    {
        public string AreaName { get; set; }
        public string CarPlateNumber { get; set; }
    }
}
