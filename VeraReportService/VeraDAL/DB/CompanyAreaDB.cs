using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class CompanyAreaDB
    {
        private static CompanyAreaDB instance;
        public static CompanyAreaDB GetInstance()
        {
            if (instance == null)
            {
                instance = new CompanyAreaDB();
            }
            return instance;
        }
        public CompanyArea AddNewCompanyArea(CompanyArea _CompanyArea)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.CompanyArea.Add(_CompanyArea);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _CompanyArea : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CompanyArea> GetAllCompanyAreaes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyAreaList = context.CompanyArea.ToList();
                    return CompanyAreaList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CompanyArea GetCompanyAreaById(int _CompanyAreaId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyArea = context.CompanyArea.FirstOrDefault(a => a.Id == _CompanyAreaId && a.Status == 1);
                    return CompanyArea != null ? CompanyArea : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteCompanyArea(int _CompanyAreaId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyArea = context.CompanyArea.FirstOrDefault(a => a.Id == _CompanyAreaId);
                    if (CompanyArea != null)
                    {
                        CompanyArea.Status = 0;
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
        private void UpdateObject(CompanyArea _newCompanyArea, ref CompanyArea _oldCompanyArea)
        {
            try
            {

                foreach (PropertyInfo CompanyAreaPropInfo in _newCompanyArea.GetType().GetProperties().ToList())
                {
                    _oldCompanyArea.GetType().GetProperty(CompanyAreaPropInfo.Name).SetValue(_oldCompanyArea, _newCompanyArea.GetType().GetProperty(CompanyAreaPropInfo.Name).GetValue(_newCompanyArea));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public CompanyArea UpdateCompanyArea(CompanyArea _CompanyArea)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldCompanyArea = context.CompanyArea.FirstOrDefault(u => u.Id == _CompanyArea.Id);
                    if (oldCompanyArea != null)
                    {
                        UpdateObject(_CompanyArea, ref oldCompanyArea);
                        var numberOfUpdatedCompanyArea = context.SaveChanges();
                        return numberOfUpdatedCompanyArea > 0 ? _CompanyArea : null;
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
