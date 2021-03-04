using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VeraDAL.Entities;
using VeraDAL.Models;

namespace VeraDAL.DB
{
    public class DeviceDB
    {
        string lastLocationTime;
        private static DeviceDB instance;
        public static DeviceDB GetInstance()
        {
            if (instance == null)
            {
                instance = new DeviceDB();
            }
            return instance;
        }
        public Device AddNewDevice(Device _device)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Device.Add(_device);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _device : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Device GetDeviceById(int _id)
        {
            try
            {
                using (var context = new VeraEntities())
                {

                    var device = context.Device.FirstOrDefault(a => a.Id == _id);
                    return device != null ? device : null;
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        public List<DeviceRepo> GetAllDevices(User _userObj, string _cihazId = "", string _cihazGsmNo = "", string _plaka = "", int _sirket = 0, int _kullanimDurumu = 0,
            int _altFirmalıGoster = 0, int _aktiflik = -1, int _firmaTipi = 0, int _terminalTypeId = 0, int _softwareVersionId = 0,
             int _cihazMontajTuru = 0)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var returnList = new List<DeviceRepo>();
                    List<Device> list = GetAllDBDevices();
                    if (_userObj.UserTypeId == 2) /*Bayi Adminse*/
                    {
                        var myDistributors = context.Distributor.Where(x => x.UserId == _userObj.Id && x.Status > 0).ToList();
                        var myDistributorsId = myDistributors.Select(x => x.Id).ToList();
                        var myCompanies = context.Company.Where(x => myDistributorsId.Contains((x.DistributorId)) && x.Status > 0).ToList();
                        var myCompanyIds = myCompanies.Select(x => x.Id).ToList();
                        list = list.Where(x => myCompanyIds.Contains(x.CompanyId)).ToList();
                    }
                    else if (_userObj.UserTypeId == 3) /*Super Adminse*/
                    {
                        list = list.Where(a => a.Status == 1).ToList();
                    }
                    else
                    {
                        list = new List<Device>();
                    }
                    if (_cihazId != "")
                        list = list.Where(x => x.DeviceId.Contains(Convert.ToString(_cihazId))).ToList();
                    if (_cihazGsmNo != "")
                        list = list.Where(x => x.DeviceGsmNumber.Contains(Convert.ToString(_cihazGsmNo))).ToList();
                    if (_plaka != "")
                        list = list.Where(x => x.CarPlateNumber.Contains(Convert.ToString(_plaka))).ToList();
                    if (_sirket != 0)
                    { // GOZDEN GECİRİLECEK
                        var company = CompanyDB.GetInstance().GetCompanyById(_sirket);
                        if (_altFirmalıGoster != 0)
                        {
                            var companies = context.Company.Where(x => x.MainCompanyId == company.Id).ToList();
                            var companyIds = companies.Select(x => x.Id).ToList();
                            list = list.Where(x => companyIds.Contains(x.CompanyId)).ToList();
                        }
                        else
                        {
                            list = list.Where(x => x.CompanyId == company.Id).ToList();
                        }
                    }
                    if (_kullanimDurumu != 0)
                        list = list.Where(x => x.UsingStatusId == _kullanimDurumu).ToList();

                    if (_aktiflik != -1)
                        list = list.Where(x => x.Status == _aktiflik).ToList();
                    if (_firmaTipi != 0)
                    {
                        var companies = context.Company.Where(a => a.CompanyTypeId == _firmaTipi && a.Status > 0).ToList();
                        var companyIds = companies.Select(x => x.Id).ToList();
                        list = list.Where(x => companyIds.Contains(x.CompanyId)).ToList();
                    }
                    if (_softwareVersionId != 0)
                        list = list.Where(x => x.SoftwareVersionId == _softwareVersionId).ToList();
                    if (_cihazMontajTuru != 0)
                        list = list.Where(x => x.DeviceMontageTypeId == _cihazMontajTuru).ToList();
                    if (_terminalTypeId != 0)
                        list = list.Where(x => x.TerminalTypeId == _terminalTypeId).ToList();

                    var allGateWays = context.Gateway.ToList();
                    var allTerminalTypes = context.TerminalType.ToList();
                    var allTerminalProtocols = context.TerminalProtocol.ToList();
                    var allSoftwareVersions = context.SoftwareVersion.ToList();
                    var allServiceTypes = context.ServiceType.ToList();
                    var allUsingStatus = context.UsingStatus.ToList();
                    var allMessaging = context.Messaging.ToList();
                    var allRefTelemetryTypes = context.RefTelemetryType.ToList();
                    var allWochechkerFlags = context.WocheckerFlag.ToList();
                    var allVehicleTypes = context.Vehicle.ToList();
                    var allDeviceMontageTypes = context.DeviceMontageType.ToList();
                    var allOdometreStatus = context.OdometreStatus.ToList();

                    var allCompany = CompanyDB.GetInstance().GetAllDBCompanies();

                    var allDistributors = context.Distributor.ToList();
                    //var allDeviceDatas = context.DeviceData.ToList();

                    list.ForEach(item =>
                    //foreach (var item in list)
                    {
                        var gateway = allGateWays.FirstOrDefault(x => x.Id == item.GatewayId);
                        var terminalType = allTerminalTypes.FirstOrDefault(x => x.Id == item.TerminalTypeId);
                        var terminalProtocol = allTerminalProtocols.FirstOrDefault(x => x.Id == item.TerminalProtocolId);
                        var softwareVersion = allSoftwareVersions.FirstOrDefault(x => x.Id == item.SoftwareVersionId);
                        var serviceType = allServiceTypes.FirstOrDefault(x => x.Id == item.ServiceTypeId);
                        var usingStatus = allUsingStatus.FirstOrDefault(x => x.Id == item.UsingStatusId);
                        var messaging = allMessaging.FirstOrDefault(x => x.Id == item.MessagingId);
                        var refTelemetryType = allRefTelemetryTypes.FirstOrDefault(x => x.Id == item.RefTelemetryRecipeId);
                        var wocheckerType = allWochechkerFlags.FirstOrDefault(x => x.Id == item.WocheckerFlagId);
                        var vehicleType = allVehicleTypes.FirstOrDefault(x => x.Id == item.VehicleTypeId);
                        var deviceMontageType = allDeviceMontageTypes.FirstOrDefault(x => x.Id == item.DeviceMontageTypeId);
                        var odometreType = allOdometreStatus.FirstOrDefault(x => x.Id == item.OdometreStatusId);
                        var company = allCompany.FirstOrDefault(x => x.Id == item.CompanyId);
                        var distributor = allDistributors.FirstOrDefault(x => x.Id == company.DistributorId);

                        DeviceData lastDeviceData = null;

                        if (item.LastDeviceDataId != null)
                        {
                            lastDeviceData = context.DeviceData.FirstOrDefault(x => x.Id == item.LastDeviceDataId);
                        }

                        returnList.Add(new DeviceRepo()
                        {
                            Id = item.Id,
                            CompanyId = item.CompanyId,
                            DeviceMontageType = deviceMontageType != null ? deviceMontageType.Name : "",
                            SoftwareVersion = softwareVersion != null ? softwareVersion.VersionCode : "",
                            UsingStatus = usingStatus != null ? usingStatus.Name : "",
                            Messaging = messaging != null ? messaging.Name : "",
                            RefTelemetryRecipe = refTelemetryType != null ? refTelemetryType.Name : "",
                            ServiceType = serviceType != null ? serviceType.Name : "",
                            WocheckerFlag = wocheckerType != null ? wocheckerType.Name : "",
                            VehicleType = vehicleType != null ? vehicleType.Name : "",
                            DeviceId = item.DeviceId,
                            TerminalProtocol = terminalProtocol != null ? terminalProtocol.Name : "",
                            TerminalType = terminalType != null ? terminalType.Name : "",
                            Gateway = gateway != null ? gateway.Name : "",
                            CarPlateNumber = item.CarPlateNumber != null ? item.CarPlateNumber : "",
                            DeviceGsmNumber = item.DeviceGsmNumber != null ? item.DeviceGsmNumber : "",
                            RFIDEngineBlock = item.RFIDEngineBlock,
                            RFIDEngineBlockStatus = item.RFIDEngineBlockStatus,
                            ProgrammedLocationSendTime = item.ProgrammedLocationSendTime,
                            ProgrammedSpeedLimit = item.ProgrammedSpeedLimit,
                            ProgrammedWaitingTime = item.ProgrammedWaitingTime,
                            ProgrammedHeartBeat = item.ProgrammedHeartBeat,
                            ProgrammedLocationDistance = item.ProgrammedLocationDistance,
                            AccountingCode = item.AccountingCode,
                            ActivityTime = item.ActivityTime,
                            CameraURL = item.CameraURL,
                            IsSummerTime = item.IsSummerTime,
                            TimeDifference = item.TimeDifference,
                            IMEINumber = item.IMEINumber != null ? item.IMEINumber : "",
                            Journey = item.Journey,
                            Telemetry = item.Telemetry,
                            DoorSensor = item.DoorSensor,
                            HeatSensor = item.HeatSensor,
                            EngineBlock = item.EngineBlock,
                            TrafficData = item.TrafficData,
                            ConstantMobile = item.ConstantMobile,
                            ConstantLatitude = item.ConstantLatitude,
                            ConstantLongitude = item.ConstantLongitude,
                            GSensor = item.GSensor,
                            DriverScoring = item.DriverScoring,
                            OdometreTypeName = odometreType != null ? odometreType.Name : "",
                            CompanyName = company != null ? company.CompanyDescription : "",
                            AracSonKmBilgisi = lastDeviceData != null ? lastDeviceData.TotalKm : 0,
                            AveaPlaka = item.AveaPlaka != null || item.AveaPlaka != "" ? item.AveaPlaka : "",
                            BillDate = item.BillDate,
                            BillNumber = item.BillNumber,
                            CarPhoneNumber = item.CarPhoneNumber,
                            CCID = item.CCID,
                            CreationDate = item.CreationDate,
                            CustomerPlateNumber = item.CustomerPlateNumber,
                            EngineBlockStatus = item.EngineBlockStatus,
                            MobileNote = item.MobileNote,
                            Aktiflik = lastDeviceData != null ? ((DateTime.Now - lastDeviceData.CreateDate).TotalMinutes < item.ActivityTime) == true : false,
                            Kontak = lastDeviceData != null ? lastDeviceData.IoStatus.EndsWith("1") : false,
                            SonAktiflikZamani = lastDeviceData != null ? Convert.ToString(lastDeviceData.CreateDate) : "",
                            SonKonumZamani = lastDeviceData != null ? GetLastActivtyTimeOfDevice(item.Id).ToString() : "",
                            SonKonum = lastDeviceData == null ? "" : GetLocationFromLatLon2(lastDeviceData.Latitude.ToString(), lastDeviceData.Longtitude.ToString()),
                            AnlikHiz = lastDeviceData != null ? lastDeviceData.KmPerHour : 0,
                            DistributorName = distributor.Name,
                            TechnicalNote = item.TechnicalNote,
                            MontageDate = item.MontageDate != null ? item.MontageDate : null,
                            MontagePerson = item.MontagePerson != null ? item.MontagePerson : "",
                            VehicleKm = item.VehicleKm
                        });
                    });
                    return (returnList);
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public bool DeleteDevice(int _deviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var device = context.Device.FirstOrDefault(a => a.Id == _deviceId && a.Status == 1);
                    if (device != null)
                    {
                        device.Status = 0;
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
        public List<SoftwareVersion> GetSoftwareVersionsByTerminalTypeId(int _terminalTypeId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var softwareVersions = context.SoftwareVersion.Where(x => x.TerminalTypeId == _terminalTypeId).ToList();
                    return softwareVersions;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private void UpdateObject(Device _newDevice, ref Device _oldDevice)
        {
            try
            {

                foreach (PropertyInfo DevicePropInfo in _newDevice.GetType().GetProperties().ToList())
                {
                    _oldDevice.GetType().GetProperty(DevicePropInfo.Name).SetValue(_oldDevice, _newDevice.GetType().GetProperty(DevicePropInfo.Name).GetValue(_newDevice));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Device UpdateDevice(Device _Device)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldDevice = context.Device.FirstOrDefault(u => u.Id == _Device.Id);
                    if (oldDevice != null)
                    {
                        UpdateObject(_Device, ref oldDevice);
                        var numberOfUpdatedDevice = context.SaveChanges();
                        return numberOfUpdatedDevice > 0 ? _Device : null;
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
        public string GetLocationFromLatLon2(string lat, string lon)
        {
            //if (lat == null || lon == null)
            //{
            //    return "";
            //}


            //lat = lat.Replace(",", ".").Trim();
            //lon = lon.Replace(",", ".").Trim();
            //using (var client = new System.Net.WebClient())
            //{
            //    var url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lon + "&key=AIzaSyDIebpQ6tJ98Z8RK0X3szDZ-azq21n43V4&language=tr";
            //    var results = client.DownloadData(url);
            //    var result = Encoding.UTF8.GetString(results);
            //    var obj = JsonConvert.DeserializeObject<GeoLocationObject>(result);

            //    if (obj.status != "OK")
            //    {
            //        return "";
            //    }
            //    return obj.results[0].formatted_address;
            //    // TODO: do something with the downloaded result from the remote
            //    // web site
            //}
            try
            {
                using (var context = new VeraEntities())
                {
                    var latD = Convert.ToDecimal(lat.Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));
                    var lonD = Convert.ToDecimal(lon.Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));

                    var konum = context.LatLonLocation.FirstOrDefault(x => x.Latitude == latD && x.Longtitude == lonD);

                    return konum != null ? konum.Location : "";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DateTime GetLastActivtyTimeOfDevice(int _deviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var cmd = context.Database.GetDbConnection().CreateCommand();

                    if (context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                    {
                        context.Database.GetDbConnection().Open();
                    }

                    cmd.CommandText = "EXEC USP_GetLastLocationTimeOfDevice " + _deviceId.ToString();
                    DateTime result = Convert.ToDateTime(cmd.ExecuteScalar());

                    context.Database.GetDbConnection().Close();

                    return result;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public DeviceData GetLastDeviceData(int _deviceId)
        {
            using (var context = new VeraEntities())
            {

                string query = $"SELECT TOP 1 * FROM DeviceData WHERE DeviceId = {_deviceId} ORDER BY Id DESC FOR JSON AUTO  ";

                var cmd = context.Database.GetDbConnection().CreateCommand();

                if (context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                {
                    context.Database.GetDbConnection().Open();
                }

                cmd.CommandText = query;
                var str = cmd.ExecuteScalar();
                if (str == null)
                    return null;

                var result = JsonConvert.DeserializeObject<List<DeviceData>>(str.ToString()).FirstOrDefault();

                context.Database.GetDbConnection().Close();

                return result;
            }
        }
        public List<Device> GetAllDBDevices()
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
                            cmd.CommandText = "EXEC USP_GetAllDevices";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<Device>>(json);
                            return list;
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
        public DeviceData GetFirstDeviceData(int _deviceId)
        {
            using (var context = new VeraEntities())
            {
                string query = $"SELECT TOP 1 * FROM DeviceData WHERE DeviceId = {_deviceId} ORDER BY Id ASC FOR JSON AUTO  ";

                var cmd = context.Database.GetDbConnection().CreateCommand();

                if (context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                {
                    context.Database.GetDbConnection().Open();
                }

                cmd.CommandText = query;
                var str = cmd.ExecuteScalar();
                if (str == null)
                    return null;

                var result = JsonConvert.DeserializeObject<List<DeviceData>>(str.ToString()).FirstOrDefault();

                context.Database.GetDbConnection().Close();

                return result;
            }
        }
        public List<DeviceModelForExcell> GetAllDBDevicesForExcel()
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
                            cmd.CommandText = "EXEC USP_GetDevicesForExcell";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<DeviceModelForExcell>>(json);
                            return list;
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
        public List<Device> GetAllDevicesForMap(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    List<Device> list = GetAllDBDevices();
                    if (_userObj.UserTypeId == 1)/*Company Adminse*/
                    {
                        var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                        var myCompany = context.Company.FirstOrDefault(x => x.Id == myCompanyId);
                        var myCompanies = context.Company.Where(x => x.MainCompanyId == myCompany.Id);
                        var myCompanyIds = myCompanies.Select(x => x.Id);
                        list = list.Where(x => myCompanyIds.Contains(x.CompanyId) || x.CompanyId == myCompanyId).ToList();
                    }
                    else
                    {
                        return null;
                    }

                    return list;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<DeviceObjectForFrontSide> GetDevicesForFrontSide(User _userObj, int _lastDeviceId)
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
                            var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id);
                            if (companyUser.IsCompanyAdmin)
                            { 
                                     cmd.CommandText = "exec dbo.USP_GetDevicesForFrontSide " + _userObj.Id + "," + _lastDeviceId;

                                     var reader = cmd.ExecuteReader();

                                     StringBuilder sb = new StringBuilder();

                                     while (reader.Read())
                                         sb.Append(reader.GetString(0));

                                     var json = sb.ToString();
                                     if (json == "")
                                     {
                                         return null;
                                     }
                                     var list = JsonConvert.DeserializeObject<List<DeviceObjectForFrontSide>>(json);

                                     if (list.Count > 0 && list != null)
                                     {
                                         return list;
                                     }
                                     return null;
                            }
                            if (companyUser.IsCompanyAdmin != true)
                            {
                                cmd.CommandText = "exec dbo.[USP_GetUserDevicesForFrontSide] " + _userObj.Id;
                                var reader = cmd.ExecuteReader();

                                StringBuilder sb = new StringBuilder();

                                while (reader.Read())
                                    sb.Append(reader.GetString(0));

                                var json = sb.ToString();
                                if (json == "")
                                {
                                    return null;
                                }
                                var list = JsonConvert.DeserializeObject<List<DeviceObjectForFrontSide>>(json);

                                if (list.Count > 0 && list != null)
                                {
                                    return list;
                                }
                                return null;
                            }
                            return null;
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
        public List<DeviceObjectForFrontSide> GetDevicesInformationForFrontSide(User _userObj, int _lastDeviceId)
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
                            var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id);
                            if (companyUser.IsCompanyAdmin)
                            {
                                cmd.CommandText = "exec dbo.USP_GetDevicesInformationForFrontSide " + _userObj.Id + "," + _lastDeviceId;

                                var reader = cmd.ExecuteReader();

                                StringBuilder sb = new StringBuilder();

                                while (reader.Read())
                                    sb.Append(reader.GetString(0));

                                var json = sb.ToString();
                                if (json == "")
                                {
                                    return null;
                                }
                                var list = JsonConvert.DeserializeObject<List<DeviceObjectForFrontSide>>(json);

                                if (list.Count > 0 && list != null)
                                {
                                    return list;
                                }
                                return null;
                            }
                            if (companyUser.IsCompanyAdmin != true)
                            {
                                cmd.CommandText = "exec dbo.[USP_GetUserDevicesInformationForFrontSide] " + _userObj.Id;
                                var reader = cmd.ExecuteReader();

                                StringBuilder sb = new StringBuilder();

                                while (reader.Read())
                                    sb.Append(reader.GetString(0));

                                var json = sb.ToString();
                                if (json == "")
                                {
                                    return null;
                                }
                                var list = JsonConvert.DeserializeObject<List<DeviceObjectForFrontSide>>(json);

                                if (list.Count > 0 && list != null)
                                {
                                    return list;
                                }
                                return null;
                            }
                            return null;
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
        public List<DeviceObjectForExcellFrontSide> GetDevicesInformationForExcel(User _userObj)
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
                            cmd.CommandText = "EXEC dbo.[USP_GetDevicesInformationForExcel]"+_userObj.Id;

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<DeviceObjectForExcellFrontSide>>(json);
                            return list;
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
        public List<DeviceObjectForSearchFrontSide> SearchDevicesForFrontSide(string _searchCarPlateNumberToFilter, int _lastDeviceId)
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

                            cmd.CommandText = "exec dbo.USP_SearchDevicesForFrontSide " + _searchCarPlateNumberToFilter + "," + _lastDeviceId;

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            if (json == "")
                            {
                                return null;
                            }
                            var list = JsonConvert.DeserializeObject<List<DeviceObjectForSearchFrontSide>>(json);

                            if (list.Count > 0 && list != null)
                            {
                                return list;
                            }
                            return null;
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
        public List<Device> GetDevicesToFilter(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    List<Device> devices = GetAllDBDevices();
                    if (_userObj.Id ==774 )
                    {
                        return devices;
                    }
                    var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id);
                    var company = context.Company.FirstOrDefault(x => x.Id == companyUser.CompanyId);
                    devices = devices.Where(x => x.CompanyId == company.Id).ToList();
                    return devices;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<Device> GetAllDBDevicesOfCompany(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var allDbDevices = GetAllDBDevices();
                    var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                    allDbDevices = allDbDevices.Where(x => x.CompanyId == myCompanyId && x.Status == 1).ToList();
                    return allDbDevices;
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        public List<PastLocationObject> GetPastLocationList(string _startDateTLPL, string _endDateTLPL, int _lastIdTLPL, int _deviceIdTLPL)
        {
            using (var context = new VeraEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        string startDate = _startDateTLPL;

                        string endDate = _endDateTLPL;

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = $"exec [dbo].[USP_GetPastLocationList] '{startDate} 00:00:00', '{endDate} 23:59:59', {_lastIdTLPL}, {_deviceIdTLPL}";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<PastLocationObject>>(json);
                            return list;
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
        public List<DeviceListObjectTSV> GetDeviceListTSV(User _userObj, int _lastDeviceIdTSV, string _carPlateNumberTSV, int _deviceActivityTSV, int _usingStatusIdTSV)
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
                            if (_carPlateNumberTSV != null && _carPlateNumberTSV != "")
                            {
                                
                                cmd.CommandText = $"exec dbo.GetDevicesByCarPlateNumberTSV '{_carPlateNumberTSV}'"+","+_userObj.Id+","+_lastDeviceIdTSV;

                            }
                            else
                            {
                                cmd.CommandText = "exec dbo.GetAllDBDevicesTSV " + _userObj.Id + "," + _lastDeviceIdTSV;
                            }
                            
                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            if (json == "")
                            {
                                return null;
                            }
                            var list = JsonConvert.DeserializeObject<List<DeviceListObjectTSV>>(json);


                            if (_deviceActivityTSV != 0)
                            {
                                var activeOrPassive = _deviceActivityTSV == 1 ? "1" : "0";
                                list = list.Where(x => x.Activity.EndsWith(activeOrPassive)).ToList();
                            }
                            if (_usingStatusIdTSV != 0)
                            {
                                var activeOrPassive = _usingStatusIdTSV == 1 ? "1" : "0";
                                list = list.Where(x => x.Status == activeOrPassive).ToList();
                            }
                            if (list.Count > 0 && list != null)
                            {
                                return list;
                            }
                            return null;
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
        public List<DeviceObjectToSearch> GetDevicesByCarPlateNumber(string _searchCarPlateNumberToFilter, User _userObj) {
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
                            cmd.CommandText = $"exec [dbo].[USP_GetDevicesByCarPlateNumber] '{_searchCarPlateNumberToFilter}',{_userObj.Id}";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<DeviceObjectToSearch>>(json);
                            return list;
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
        public List<DeviceObjectForFrontSide> GetLastLocationTimeOfDevicesWithGroup(int _groupIdTLPLT, int _lastDeviceIdTLPLT, User _userObj) {
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
                            cmd.CommandText = $"exec [dbo].[USP_GetLastLocationTimeOfDevicesWithGroup] {_userObj.Id}, {_lastDeviceIdTLPLT} , {_groupIdTLPLT} ";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0)); 
                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<DeviceObjectForFrontSide>>(json);
                            return list;
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

    public class DeviceObjectToSearch
    {
        public string Plaka { get; set; }
        public string Enlem { get; set; }
        public string Boylam { get; set; }
        public string Konum { get; set; }
    }
    public class DeviceListObjectTSV
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string CarPlateNumber { get; set; }
        public string DeviceGsmNumber { get; set; }
        public string DeviceId { get; set; }
        public string DeviceMontageType { get; set; }
        public string EngineBlockStatus { get; set; }
        public string OperationTime { get; set; }
        public string PositionTime { get; set; }
        public string PositionDistance { get; set; }
        public string SpeedLimit { get; set; }
        public string WaitingTime { get; set; }
        public string MontageData { get; set; }
        public string LastLocationTime { get; set; }
        public string Activity { get; set; }
        public string ActivityTime { get; set; }
        public string TotalKm { get; set; }
        public string Status { get; set; }
    }
    public class PastLocationObject
    {
        public int LastDeviceDataId { get; set; }
        public string AracAdi { get; set; }
        public string Zaman { get; set; }
        public string Hiz { get; set; }
        public string AracKm { get; set; }
        public string TotalKm { get; set; }
        public string Enlem { get; set; }
        public string Boylam { get; set; }
        public string Konum { get; set; }
    }

    public class DeviceObjectForFrontSide
    {
        public int LastDeviceId { get; set; }
        public string CarPlateNumber { get; set; }
        public string LastLocationTime { get; set; }
        public string KmPerHour { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string IoStatus { get; set; }
        public string Konum { get; set; }
        public string TotalKmDaily { get; set; }
        public string DirectionDegree { get; set; }
        public string Mail { get; set; }
        public string ActivityTime { get; set; } 
        public string LastDeviceDataCreateDate { get; set; } 
    }
    public class DeviceObjectForDevicesInfo
    {
        public string CompanyName { get; set; }
        public string CarPlateNumber { get; set; }
        public string DeviceGSMNumber { get; set; }
        public string DeviceId { get; set; }
        public string LastLocationTime { get; set; }
        public string DeviceMontageType { get; set; }
        public string Activity { get; set; }
        public string EngineBlockStatus { get; set; }
        public string ActivityTime { get; set; }
        public string TotalKm { get; set; }
        public string OperationTime { get; set; }
        public string PositionTime { get; set; }
        public string PositionDistance { get; set; }
        public string SpeedLimit { get; set; }
        public string WaitingTime { get; set; }
        public string MontageDate { get; set; }
    }
    public class DeviceObjectForExcellFrontSide
    {
        public int LastDeviceId { get; set; }
        public string Aktiflik { get; set; }
        public string KontakDurumu { get; set; }
        public string Mesaj { get; set; }
        public string Konum { get; set; }
        public string Enlem { get; set; }
        public string Boylam { get; set; }
        public string Görevli { get; set; }
        public string Mail { get; set; } 
        public string AracAdi { get; set; }  
        public string KonumZamani { get; set; }
        public string Hiz { get; set; }
        public string LastDeviceDataCreateDate { get; set; }

    }
    public class DeviceObjectForSearchFrontSide
    {
        public int LastDeviceId { get; set; }
        public string CarPlateNumber { get; set; }
        public string LastLocationTime { get; set; }
        public string KmPerHour { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string IoStatus { get; set; }
        public string Konum { get; set; }
        public string TotalKmDaily { get; set; }
        public string DirectionDegree { get; set; }

    }

    public class DeviceModelForExcell
    {
        public string MobileID { get; set; }
        public string Sirket { get; set; }
        public string Plaka { get; set; }
        public string IMEINo { get; set; }
        public string TerminalProtokol { get; set; }
        public string Gateway { get; set; }
        public string TerminalTipi { get; set; }
        public string YazilimVers { get; set; }
        public string OdometreStatus { get; set; }
        public string ProgramlananKonumSuresi { get; set; }
        public string ProgramlananKonumMesafe { get; set; }
        public string ProgramlananHizLimiti { get; set; }
        public string ProgramlananBekleme { get; set; }
        public string ProgramlananHeartBeat { get; set; }
        public string AktiflikSuresi { get; set; }
        public string Sefer { get; set; }
        public string Mesajlasma { get; set; }
        public string Telemetri { get; set; }
        public string IsiSensoru { get; set; }
        public string KapiSensoru { get; set; }
        public string SabitMobil { get; set; }
        public string SabitLatitude { get; set; }
        public string WocheckerFlag { get; set; }
        public string TeknikNot { get; set; }
        public string MontajTarihi { get; set; }
        public string CihazMontajTuru { get; set; }
        public string MotorBlokaji { get; set; }
        public string MotorBlokajDurumu { get; set; }
        public string AracBlokaji { get; set; }
        public string GSensor { get; set; }
        public string SurucuSkorlama { get; set; }
        public string RFIDMotorBlokaj { get; set; }
        public string RFIDMotorBlokajDurumu { get; set; }
        public string MobilNot { get; set; }
        public string AveaPlaka { get; set; }
        public string MontajiYapan { get; set; }
        public string SonAktiflikZamani { get; set; }
        public string Name { get; set; }
        public string MusteriPlakasi { get; set; }
    }




    public class PlusCode
    {
        public string global_code { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast2 northeast { get; set; }
        public Southwest2 southwest { get; set; }
    }

    public class Geometry
    {
        public Bounds bounds { get; set; }
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }

    public class GeoLocationObject
    {
        public PlusCode plus_code { get; set; }
        public List<Result> results { get; set; }
        public string status { get; set; }
    }

}