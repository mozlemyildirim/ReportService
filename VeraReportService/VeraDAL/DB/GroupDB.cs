using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class GroupDB
    {
        private static GroupDB instance;
        public static GroupDB GetInstance()
        {
            if (instance == null)
            {
                instance = new GroupDB();
            }
            return instance;
        }
        public Group AddNewGroup(Group _Group)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Group.Add(_Group);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _Group : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Group> GetAllGroups()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var GroupList = context.Group.ToList();
                    return GroupList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Group GetGroupById(int _GroupId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Group = context.Group.FirstOrDefault(a => a.Id == _GroupId && a.Status == 1);
                    return Group != null ? Group : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteGroup(int _GroupId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Group = context.Group.FirstOrDefault(a => a.Id == _GroupId);
                    if (Group != null)
                    {
                        Group.Status = 0;
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
        private void UpdateObject(Group _newGroup, ref Group _oldGroup)
        {
            try
            {

                foreach (PropertyInfo GroupPropInfo in _newGroup.GetType().GetProperties().ToList())
                {
                    _oldGroup.GetType().GetProperty(GroupPropInfo.Name).SetValue(_oldGroup, _newGroup.GetType().GetProperty(GroupPropInfo.Name).GetValue(_newGroup));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Group UpdateGroup(Group _Group)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldGroup = context.Group.FirstOrDefault(u => u.Id == _Group.Id);
                    if (oldGroup != null)
                    {
                        UpdateObject(_Group, ref oldGroup);
                        var numberOfUpdatedGroup = context.SaveChanges();
                        return numberOfUpdatedGroup > 0 ? _Group : null;
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
        public List<Group> GetAllCompanyGroupsByUserId(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {

                    if (_userObj.Id ==774)
                    {
                        return context.Group.Where(x=>x.Status==1).ToList();
                    }
                    var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                    var myCompanyGroups = context.CompanyGroup.Where(x => x.CompanyId == myCompanyId).ToList();
                    var myCompanyGroupsIds = myCompanyGroups.Select(x => x.Id).ToList();
                    var myGroups = context.Group.Where(x => myCompanyGroupsIds.Contains(x.Id) && x.Status > 0).ToList();
                    if (myGroups != null)
                    {
                        return myGroups;
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Group> GetGroupsByGroupName(User _userObj, string _groupName)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var groupName = _groupName.Trim().ToLower();

                    var companyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                    var companyGroups = context.CompanyGroup.Where(x => x.CompanyId == companyId).ToList();
                    var companyGroupsGroupIds = companyGroups.Select(x => x.GroupId).ToList();
                    var groups = context.Group.Where(x => companyGroupsGroupIds.Contains(x.Id)).ToList();
                    groups = groups.Where(x => x.Name != null && x.Name.Trim().ToLower().Contains(groupName) && x.Status == 1).ToList();

                    if (groups != null)
                    {
                        return groups;
                    }
                    return null;

                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }
    }
}
