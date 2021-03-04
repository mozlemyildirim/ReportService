using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class UserReportDB
    {
        private static UserReportDB instance;
        public static UserReportDB GetInstance()
        {
            if (instance == null)
            {
                instance = new UserReportDB();
            }
            return instance;
        }
        public UserReport AddNewUserReport(UserReport _UserReport)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.UserReport.Add(_UserReport);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _UserReport : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<UserReport> GetAllUserReport()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserReportList = context.UserReport.ToList();
                    return UserReportList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UserReport GetUserReportById(int _UserReportId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserReport = context.UserReport.FirstOrDefault(a => a.Id == _UserReportId);
                    return UserReport != null ? UserReport : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteUserReport(int _userId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var userReports = context.UserReport.Where(a => a.UserId == _userId).ToList();
                    if (userReports != null)
                    {
                        context.UserReport.RemoveRange(userReports);
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
        private void UpdateObject(UserReport _newUserReport, ref UserReport _oldUserReport)
        {
            try
            {

                foreach (PropertyInfo UserReportPropInfo in _newUserReport.GetType().GetProperties().ToList())
                {
                    _oldUserReport.GetType().GetProperty(UserReportPropInfo.Name).SetValue(_oldUserReport, _newUserReport.GetType().GetProperty(UserReportPropInfo.Name).GetValue(_newUserReport));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public UserReport UpdateUserReport(UserReport _UserReport)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldUserReport = context.UserReport.FirstOrDefault(u => u.Id == _UserReport.Id);
                    if (oldUserReport != null)
                    {
                        UpdateObject(_UserReport, ref oldUserReport);
                        var numberOfUpdatedUserReport = context.SaveChanges();
                        return numberOfUpdatedUserReport > 0 ? _UserReport : null;
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
        public List<Report> GetAllUserReportByUserIdToLeft(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var allReports = ReportDB.GetInstance().GetAllReportes();
                    var userCompany = CompanyUserDB.GetInstance().GetCompanyUserById(_userObj.Id);
                    if (userCompany.IsCompanyAdmin)
                    {
                        return allReports;
                    }
                    else
                    {
                        var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                        var myDevices = allReports.Where(x => x.Id == myCompanyId).ToList();
                        return myDevices;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Report> GetAllUserReportByUserIdToRight(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {

                    var allReports = ReportDB.GetInstance().GetAllReportes();
                    var UserReport = context.UserReport.Where(x => x.UserId == _userObj.Id).ToList();
                    var UserReportIds = UserReport.Select(x => x.ReportId).ToList();
                    var reports = allReports.Where(x => UserReportIds.Contains(x.Id)).ToList();
                    return reports;

                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        public List<UserReport> GetUserReportIdsByGroupId(int _userId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserReport = context.UserReport.Where(x => x.UserId == _userId).ToList();
                    return UserReport;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        } 
        public bool DeleteAllUserReportByGroupId(int _userId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserReport = context.UserReport.Where(x => x.UserId == _userId).ToList();
                    context.UserReport.RemoveRange(UserReport);
                    int numberOfDeleted = context.SaveChanges();

                    return numberOfDeleted > 0;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<UserReport> GetUserReportByUserId(int _raporatamauserid)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    
                    var UserReport = context.UserReport.Where(x => x.UserId == _raporatamauserid).ToList();
                    return UserReport;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<UserReportInfo> GetUserReportByUser(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var userCompany = CompanyUserDB.GetInstance().GetCompanyUserById(_userObj.Id);
                    var userReports = context.UserReport.Where(x => x.UserId == _userObj.Id).ToList();
                    var userReportsIds = userReports.Select(x => x.ReportId).ToList();
                    var reports = context.Report.ToList();
                    
                    var userReportList = new List<UserReportInfo>();
                    foreach (var item in userReports)
                            {
                                var report= reports.FirstOrDefault(x=> x.Id == item.ReportId);
                                userReportList.Add(new UserReportInfo() {
                                    UserReportId=item.Id,
                                    ReportId=report.Id,
                                    Name=report.Name,
                                    ReportStatus=item.AutomaticReportingStatus==1 ? "Aktif":"Pasif",
                                    ReportType=item.UserReportType==2 ? "PDF":"EXCEL"
                                });
                            }
                        return userReportList;

                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public UserReportIsAssigned GetUsersAssignedReports(User userObj) {
            try
            {
                using (var context= new VeraEntities())
                {
                    var userReports = context.UserReport.Where(x => x.UserId == userObj.Id).Select(x=>x.ReportId).ToList();
                    var reports = context.Report.Where(x=> userReports.Contains(x.Id)).ToList();
                    var userReportIsAssigned = new UserReportIsAssigned();
                    foreach (var item in reports)
                    {
                        if (item.Name == "SeferOzetiRaporu") {
                            userReportIsAssigned.SeferOzetiRaporu = true;
                        }
                        if (item.Name == "SeferOlayRaporu")
                        {
                            userReportIsAssigned.SeferOlayRaporu = true;
                        }
                        if (item.Name == "GecmisKonumRaporu")
                        {
                            userReportIsAssigned.GecmisKonumRaporu = true;
                        }
                        if (item.Name == "SonKonumRaporu")
                        {
                            userReportIsAssigned.SonKonumRaporu = true;
                        }
                        if (item.Name == "AracGiseRaporu")
                        {
                            userReportIsAssigned.AracGiseRaporu = true;
                        }
                        if (item.Name == "BolgeGirisCikisRaporu")
                        {
                            userReportIsAssigned.BolgeGirisCikisRaporu = true;
                        }
                        if (item.Name == "MesafeOzetRaporu")
                        {
                            userReportIsAssigned.MesafeOzetRaporu = true;
                        }
                        if (item.Name == "KontakDurumRaporu")
                        {
                            userReportIsAssigned.KontakDurumRaporu= true;
                        }
                    }
                    return userReportIsAssigned;
                }
            }
            catch (Exception exc)
            { 
                throw exc;
            }
            
        }

    }
    public class UserReportIsAssigned
    { 
       public bool SeferOzetiRaporu  { get; set; } 
       public bool SeferOlayRaporu   { get; set; }    
       public bool GecmisKonumRaporu  { get; set; }        
       public bool SonKonumRaporu  { get; set; }    
       public bool AracGiseRaporu    { get; set; }    
       public bool BolgeGirisCikisRaporu { get; set; }    
       public bool MesafeOzetRaporu { get; set; }
       public bool KontakDurumRaporu { get; set; }

    }
    public class UserReportInfo
    {
        public int UserReportId { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string ReportStatus { get; set; }
        public string ReportType { get; set; }

    }
}
