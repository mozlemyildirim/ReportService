using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class CompanyGroupDB
    {
        private static CompanyGroupDB instance;
        public static CompanyGroupDB GetInstance()
        {
            if (instance == null)
            {
                instance = new CompanyGroupDB();
            }
            return instance;
        }
        public CompanyGroup AddNewCompanyGroup(CompanyGroup _CompanyGroup)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.CompanyGroup.Add(_CompanyGroup);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _CompanyGroup : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CompanyGroup> GetAllCompanyGroups()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyGroupList = context.CompanyGroup.ToList();
                    return CompanyGroupList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CompanyGroup GetCompanyGroupById(int _CompanyGroupId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyGroup = context.CompanyGroup.FirstOrDefault(a => a.Id == _CompanyGroupId && a.Status == 1);
                    return CompanyGroup != null ? CompanyGroup : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteCompanyGroup(int _CompanyGroupId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyGroup = context.CompanyGroup.FirstOrDefault(a => a.Id == _CompanyGroupId);
                    if (CompanyGroup != null)
                    {
                        CompanyGroup.Status = 0;
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
        private void UpdateObject(CompanyGroup _newCompanyGroup, ref CompanyGroup _oldCompanyGroup)
        {
            try
            {

                foreach (PropertyInfo CompanyGroupPropInfo in _newCompanyGroup.GetType().GetProperties().ToList())
                {
                    _oldCompanyGroup.GetType().GetProperty(CompanyGroupPropInfo.Name).SetValue(_oldCompanyGroup, _newCompanyGroup.GetType().GetProperty(CompanyGroupPropInfo.Name).GetValue(_newCompanyGroup));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public CompanyGroup UpdateCompanyGroup(CompanyGroup _CompanyGroup)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldCompanyGroup = context.CompanyGroup.FirstOrDefault(u => u.Id == _CompanyGroup.Id);
                    if (oldCompanyGroup != null)
                    {
                        UpdateObject(_CompanyGroup, ref oldCompanyGroup);
                        var numberOfUpdatedCompanyGroup = context.SaveChanges();
                        return numberOfUpdatedCompanyGroup > 0 ? _CompanyGroup : null;
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
       
    }
}
