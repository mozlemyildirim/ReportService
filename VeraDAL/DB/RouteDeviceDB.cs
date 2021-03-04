using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class RouteDeviceDB
    {
        private static RouteDeviceDB instance;
        public static RouteDeviceDB GetInstance()
        {
            if (instance == null)
            {
                instance = new RouteDeviceDB();
            }
            return instance;
        }
        public RouteDevice AddNewRouteDevice(RouteDevice _RouteDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.RouteDevice.Add(_RouteDevice);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _RouteDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<RouteDevice> GetAllRouteDevicees()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteDeviceList = context.RouteDevice.ToList();
                    return RouteDeviceList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public RouteDevice GetRouteDeviceById(int _RouteDeviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteDevice = context.RouteDevice.FirstOrDefault(a => a.Id == _RouteDeviceId && a.Status == 1);
                    return RouteDevice != null ? RouteDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteRouteDevice(int _RouteDeviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteDevice = context.RouteDevice.FirstOrDefault(a => a.DeviceId == _RouteDeviceId);
                    if (RouteDevice != null)
                    {
                        context.RouteDevice.Remove(RouteDevice);
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
        private void UpdateObject(RouteDevice _newRouteDevice, ref RouteDevice _oldRouteDevice)
        {
            try
            {

                foreach (PropertyInfo RouteDevicePropInfo in _newRouteDevice.GetType().GetProperties().ToList())
                {
                    _oldRouteDevice.GetType().GetProperty(RouteDevicePropInfo.Name).SetValue(_oldRouteDevice, _newRouteDevice.GetType().GetProperty(RouteDevicePropInfo.Name).GetValue(_newRouteDevice));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public RouteDevice UpdateRouteDevice(RouteDevice _RouteDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldRouteDevice = context.RouteDevice.FirstOrDefault(u => u.Id == _RouteDevice.Id);
                    if (oldRouteDevice != null)
                    {
                        UpdateObject(_RouteDevice, ref oldRouteDevice);
                        var numberOfUpdatedRouteDevice = context.SaveChanges();
                        return numberOfUpdatedRouteDevice > 0 ? _RouteDevice : null;
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
        public bool DeleteRouteDevices(int _routeId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteDevicesToDelete = context.RouteDevice.Where(x => x.RouteId == _routeId).ToList();
                    if (RouteDevicesToDelete != null && RouteDevicesToDelete.Count > 0)
                    {
                        context.RouteDevice.RemoveRange(RouteDevicesToDelete);
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
        public List<Device> GetRouteVehiclesByRouteId(int _routeId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteDevicesDeviceIds = context.RouteDevice.Where(x => x.RouteId == _routeId && x.Status == 1).Select(x => x.DeviceId).ToList();
                    var devices = context.Device.Where(x => RouteDevicesDeviceIds.Contains(x.Id)).ToList();
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
        public List<Route> GetRouteVehiclesByDeviceId(int _deviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteDevicesDeviceIds = context.RouteDevice.Where(x => x.DeviceId == _deviceId && x.Status == 1).Select(x => x.RouteId).ToList();
                    var routes = context.Route.Where(x => RouteDevicesDeviceIds.Contains(x.Id) && x.Status == 1).ToList();
                    if (routes != null)
                    {
                        return routes;
                    }
                    return null;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<RouteWithVehicle> GetroutesWithVehicles(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var company = CompanyUserDB.GetInstance().GetCompanyByCompanyUserId(_userObj);
                    var subCompaniesAndOwnCompanyIds = context.Company.Where(x => x.MainCompanyId == company.Id).Select(x => x.Id).ToList();
                    subCompaniesAndOwnCompanyIds.Add(company.Id);

                    var returnList = new List<RouteWithVehicle>();

                    var companyRoutesRouteIds = context.CompanyRoute.Where(x => subCompaniesAndOwnCompanyIds.Contains(x.CompanyId)).Select(x => x.RouteId).ToList();
                    var companyRoutesStatus1 = context.Route.Where(x => companyRoutesRouteIds.Contains(x.Id) && x.Status == 1).Select(x => x.Id).ToList();
                    var RouteDevices = context.RouteDevice.Where(x => companyRoutesStatus1.Contains(x.RouteId)).ToList();

                    foreach (var item in RouteDevices)
                    {
                        var routeName = context.Route.FirstOrDefault(x => x.Id == item.RouteId).Name;
                        var deviceCarPlateNumber = context.Device.FirstOrDefault(x => x.Id == item.DeviceId).CarPlateNumber;
                        returnList.Add(new RouteWithVehicle()
                        {
                            CarPlateNumber = deviceCarPlateNumber,
                            RouteName = routeName
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
        public bool DeleteAllRouteDevices(int _vehicleId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteDeviceList = context.RouteDevice.Where(a => a.DeviceId == _vehicleId).ToList();
                    if (RouteDeviceList != null && RouteDeviceList.Count > 0)
                    {
                        context.RouteDevice.RemoveRange(RouteDeviceList);
                        int numOfDeleted = context.SaveChanges();
                        return numOfDeleted > 0;
                    }
                }
                return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
    public class RouteWithVehicle
    {
        public string RouteName { get; set; }
        public string CarPlateNumber { get; set; }
    }
}
