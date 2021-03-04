using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class AreaDB
    {
        private static AreaDB instance;
        public static AreaDB GetInstance()
        {
            if (instance == null)
            {
                instance = new AreaDB();
            }
            return instance;
        }
        public Area AddNewArea(Area _Area)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Area.Add(_Area);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _Area : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Area> GetAllAreaes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var AreaList = context.Area.ToList();
                    return AreaList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Area GetAreaById(int _AreaId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Area = context.Area.FirstOrDefault(a => a.Id == _AreaId && a.Status == 1);
                    return Area != null ? Area : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteArea(int _AreaId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Area = context.Area.FirstOrDefault(a => a.Id == _AreaId);
                    if (Area != null)
                    {
                        Area.Status = 0;
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
        private void UpdateObject(Area _newArea, ref Area _oldArea)
        {
            try
            {

                foreach (PropertyInfo AreaPropInfo in _newArea.GetType().GetProperties().ToList())
                {
                    _oldArea.GetType().GetProperty(AreaPropInfo.Name).SetValue(_oldArea, _newArea.GetType().GetProperty(AreaPropInfo.Name).GetValue(_newArea));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Area UpdateArea(Area _Area)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldArea = context.Area.FirstOrDefault(u => u.Id == _Area.Id);
                    if (oldArea != null)
                    {
                        UpdateObject(_Area, ref oldArea);
                        var numberOfUpdatedArea = context.SaveChanges();
                        return numberOfUpdatedArea > 0 ? _Area : null;
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
        public List<Area> GetAreaListOfCompany(string _areaNameTLA, int _areaTypeTLA, User _userObj) {
            try
            {
                using (var context= new VeraEntities())
                {
                    var company = CompanyUserDB.GetInstance().GetCompanyByCompanyUserId(_userObj);
                    var companyAreas = context.CompanyArea.Where(x => x.CompanyId == company.Id).ToList();
                    var companyAreasAreaIds = companyAreas.Select(x => x.AreaId).ToList();
                    var areas = context.Area.Where(x => companyAreasAreaIds.Contains(x.Id) && x.Status==1).ToList();
                    if (!String.IsNullOrEmpty(_areaNameTLA)) {
                        areas = areas.Where(x => x.Name.Trim().ToLower().Contains(_areaNameTLA.Trim().ToLower())).ToList();
                    }
                    if (_areaTypeTLA != 0) {
                        var areaIsRestricted = _areaTypeTLA == 1 ? true : false;
                        areas = areas.Where(x => x.IsRestricted == areaIsRestricted).ToList();
                    }
                    if (areas != null)
                        return areas;
                    return null;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<Area> GetAreaList(User _userObj) {
            try
            {
                using (var context  = new VeraEntities())
                {
                    if (_userObj.Id == 774) {
                        return context.Area.Where(x=>x.Status==1).ToList();
                    }
                    var company = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id);
                    var companyGroupsGroupIds = context.CompanyGroup.Where(x => x.CompanyId == company.Id).Select(x => x.GroupId).ToList();
                    var areas = context.Area.Where(x => companyGroupsGroupIds.Contains(x.Id) && x.Status==1).ToList();
                    if (areas != null)
                        return areas;
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
