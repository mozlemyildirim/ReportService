using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class CompanyReportDB
    {
        private static CompanyReportDB instance;
        public static CompanyReportDB GetInstance()
        {
            if (instance == null)
            {
                instance = new CompanyReportDB();
            }
            return instance;
        }
        public CompanyReport AddNewCompanyReport(CompanyReport _CompanyReport)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.CompanyReport.Add(_CompanyReport);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _CompanyReport : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CompanyReport> GetAllCompanyReportes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyReportList = context.CompanyReport.ToList();
                    return CompanyReportList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CompanyReport GetCompanyReportById(int _CompanyReportId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyReport = context.CompanyReport.FirstOrDefault(a => a.Id == _CompanyReportId && a.Status == 1);
                    return CompanyReport != null ? CompanyReport : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteCompanyReport(int _CompanyReportId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyReport = context.CompanyReport.FirstOrDefault(a => a.Id == _CompanyReportId);
                    if (CompanyReport != null)
                    {
                        CompanyReport.Status = 0;
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
        private void UpdateObject(CompanyReport _newCompanyReport, ref CompanyReport _oldCompanyReport)
        {
            try
            {

                foreach (PropertyInfo CompanyReportPropInfo in _newCompanyReport.GetType().GetProperties().ToList())
                {
                    _oldCompanyReport.GetType().GetProperty(CompanyReportPropInfo.Name).SetValue(_oldCompanyReport, _newCompanyReport.GetType().GetProperty(CompanyReportPropInfo.Name).GetValue(_newCompanyReport));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public CompanyReport UpdateCompanyReport(CompanyReport _CompanyReport)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldCompanyReport = context.CompanyReport.FirstOrDefault(u => u.Id == _CompanyReport.Id);
                    if (oldCompanyReport != null)
                    {
                        UpdateObject(_CompanyReport, ref oldCompanyReport);
                        var numberOfUpdatedCompanyReport = context.SaveChanges();
                        return numberOfUpdatedCompanyReport > 0 ? _CompanyReport : null;
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
