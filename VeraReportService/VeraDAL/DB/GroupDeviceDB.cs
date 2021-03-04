using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class GroupDeviceDB
    {
        private static GroupDeviceDB instance;
        public static GroupDeviceDB GetInstance()
        {
            if (instance == null)
            {
                instance = new GroupDeviceDB();
            }
            return instance;
        }
        public GroupDevice AddNewGroupDevice(GroupDevice _GroupDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.GroupDevice.Add(_GroupDevice);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _GroupDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<GroupDevice> GetAllGroupDevices()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var GroupDeviceList = context.GroupDevice.ToList();
                    return GroupDeviceList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public GroupDevice GetGroupDeviceById(int _GroupDeviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var GroupDevice = context.GroupDevice.FirstOrDefault(a => a.Id == _GroupDeviceId && a.Status == 1);
                    return GroupDevice != null ? GroupDevice : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteGroupDevice(int _GroupDeviceId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var GroupDevice = context.GroupDevice.FirstOrDefault(a => a.Id == _GroupDeviceId);
                    if (GroupDevice != null)
                    {
                        GroupDevice.Status = 0;
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
        private void UpdateObject(GroupDevice _newGroupDevice, ref GroupDevice _oldGroupDevice)
        {
            try
            {

                foreach (PropertyInfo GroupDevicePropInfo in _newGroupDevice.GetType().GetProperties().ToList())
                {
                    _oldGroupDevice.GetType().GetProperty(GroupDevicePropInfo.Name).SetValue(_oldGroupDevice, _newGroupDevice.GetType().GetProperty(GroupDevicePropInfo.Name).GetValue(_newGroupDevice));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public GroupDevice UpdateGroupDevice(GroupDevice _GroupDevice)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldGroupDevice = context.GroupDevice.FirstOrDefault(u => u.Id == _GroupDevice.Id);
                    if (oldGroupDevice != null)
                    {
                        UpdateObject(_GroupDevice, ref oldGroupDevice);
                        var numberOfUpdatedGroupDevice = context.SaveChanges();
                        return numberOfUpdatedGroupDevice > 0 ? _GroupDevice : null;
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
        public List<Device> GetAllGroupDevicesByUserIdToLeft(User _userObj)
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
        public List<Device> GetAllGroupDevicesByUserIdToRight(User _userObj, int _groupId)
        {
            try
            {
                using (var context = new VeraEntities())
                {

                    var allDevices = DeviceDB.GetInstance().GetAllDBDevices();
                    var groupDevices = context.GroupDevice.Where(x => x.GroupId == _groupId);
                    var groupDevicesIds = groupDevices.Select(x => x.DeviceId).ToList();
                    var devices = allDevices.Where(x => groupDevicesIds.Contains(x.Id)).ToList();
                    return devices;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GroupDevice> GetGroupDeviceIdsByGroupId(int _groupID)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var groupDevice = context.GroupDevice.Where(x => x.GroupId == _groupID).ToList();
                    return groupDevice;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public bool DeleteAllGroupDeviceByGroupId(int _groupId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var groupDevices = context.GroupDevice.Where(x => x.GroupId == _groupId).ToList();
                    context.GroupDevice.RemoveRange(groupDevices);
                    int numberOfDeleted = context.SaveChanges();

                    return numberOfDeleted > 0;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

    }
}
