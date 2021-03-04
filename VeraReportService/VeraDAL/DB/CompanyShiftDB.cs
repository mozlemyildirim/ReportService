using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class CompanyShiftDB
    {
        private static CompanyShiftDB instance;
        public static CompanyShiftDB GetInstance()
        {
            if (instance == null)
            {
                instance = new CompanyShiftDB();
            }
            return instance;
        }
        public CompanyShift AddNewCompanyShift(CompanyShift _CompanyShift)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.CompanyShift.Add(_CompanyShift);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _CompanyShift : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CompanyShift> GetAllCompanyShiftes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyShiftList = context.CompanyShift.ToList();
                    return CompanyShiftList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CompanyShift GetCompanyShiftById(int _CompanyShiftId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyShift = context.CompanyShift.FirstOrDefault(a => a.Id == _CompanyShiftId && a.Status == 1);
                    return CompanyShift != null ? CompanyShift : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteCompanyShift(int _CompanyShiftId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyShift = context.CompanyShift.FirstOrDefault(a => a.Id == _CompanyShiftId);
                    if (CompanyShift != null)
                    {
                        CompanyShift.Status = 0;
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
        private void UpdateObject(CompanyShift _newCompanyShift, ref CompanyShift _oldCompanyShift)
        {
            try
            {

                foreach (PropertyInfo CompanyShiftPropInfo in _newCompanyShift.GetType().GetProperties().ToList())
                {
                    _oldCompanyShift.GetType().GetProperty(CompanyShiftPropInfo.Name).SetValue(_oldCompanyShift, _newCompanyShift.GetType().GetProperty(CompanyShiftPropInfo.Name).GetValue(_newCompanyShift));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public CompanyShift UpdateCompanyShift(CompanyShift _CompanyShift)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldCompanyShift = context.CompanyShift.FirstOrDefault(u => u.Id == _CompanyShift.Id);
                    if (oldCompanyShift != null)
                    {
                        UpdateObject(_CompanyShift, ref oldCompanyShift);
                        var numberOfUpdatedCompanyShift = context.SaveChanges();
                        return numberOfUpdatedCompanyShift > 0 ? _CompanyShift : null;
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
