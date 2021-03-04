using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;
using VeraDAL.Models;

namespace VeraDAL.DB
{
    public class CompanyDB
    {
        private static CompanyDB instance;
        public static CompanyDB GetInstance()
        {
            if (instance == null)
            {
                instance = new CompanyDB();
            }
            return instance;
        }
        public Company AddNewCompany(Company _company)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Company.Add(_company);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _company : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CompanyRepo> GetCompanyList(User _userObj, int _kullanimDurumu = -1, int _firmaTipi = 0, string _firmaKodu = "")
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var returnList = new List<CompanyRepo>();

                    List<Company> list = GetAllDBCompanies();

                    if (_userObj.UserTypeId == 2)
                    {
                        var myDistributors = context.Distributor.Where(x => x.UserId == _userObj.Id && x.Status > 0).ToList();
                        var myDistributorsId = myDistributors.Select(x => x.Id).ToList();
                        var myCompanies = list.Where(x => myDistributorsId.Contains((x.DistributorId)) && x.Status > 0).ToList();
                        var myCompanyIds = myCompanies.Select(x => x.Id).ToList();
                        list = list.Where(x => myCompanyIds.Contains(x.Id)).ToList();
                    }
                    else if (_userObj.UserTypeId == 3)
                    {
                        list = list.Where(a => a.Status == 1).ToList();
                    }
                    else
                    {
                        list = new List<Company>();
                    }
                    if (_kullanimDurumu != -1)
                        list = list.Where(x => x.CompanyStatus == (_kullanimDurumu == 1 ? true : false)).ToList();
                    if (_firmaTipi != 0)
                        list = list.Where(x => x.CompanyTypeId == _firmaTipi).ToList();
                    if (_firmaKodu != "")
                        list = list.Where(x => x.CompanyCode == _firmaKodu).ToList();

                    var allPlatforms = context.Platform.ToList();
                    var allSectors = context.Sector.ToList();
                    var allReports = context.Report.ToList();
                    var allCompanyPartners = context.CompanyPartner.ToList();
                    var allDistributors = context.Distributor.ToList();



                    foreach (var item in list)
                    {
                        var platform = allPlatforms.FirstOrDefault(x => x.Id == item.PlatformId);
                        var sector = allSectors.FirstOrDefault(x => x.Id == item.SectorId);
                        var report = allReports.FirstOrDefault(x => x.Id == item.CompanyReportId);
                        var deviceNumberOfCompany = context.Device.Count(x => x.CompanyId == item.Id && x.Status > 0);
                        var companyPartner = allCompanyPartners.FirstOrDefault(x => x.Id == item.CompanyPartnerId && x.Status == 1);
                        var distributor = allDistributors.FirstOrDefault(x => x.Id == item.DistributorId && x.Status > 0);
                        returnList.Add(new CompanyRepo()
                        {
                            Address1 = item.Address1,
                            Phone = item.Phone,
                            TechnicalEmail1 = item.TechnicalEmail1,
                            TechnicalEmail2 = item.TechnicalEmail2,
                            TechnicalEmail3 = item.TechnicalEmail3,
                            AlarmSms = item.AlarmSms,
                            CompanyCode = item.CompanyCode,
                            CompanyDescription = item.CompanyDescription,
                            CompanyGroupName = item.CompanyGroupName,
                            CompanyPartnerId = item.CompanyPartnerId,
                            CompanyReportId = item.CompanyReportId,
                            CompanyVehicleProgramming = item.CompanyVehicleProgramming,
                            Id = item.Id,
                            DeviceNumberOfCompany = deviceNumberOfCompany,
                            DistributorId = item.DistributorId,
                            CompanyStatus = item.CompanyStatus,
                            MainCompanyId = item.MainCompanyId,
                            CompanyTypeId = item.CompanyTypeId,
                            PasswordControl = item.PasswordControl,
                            PlatformId = item.PlatformId,
                            SectorId = item.SectorId,
                            Status = item.Status,
                            ReportName = report != null ? report.Name : "-",
                            PlatformName = platform != null ? platform.Name : "-",
                            SectorName = sector != null ? sector.Name : "-",
                            CompanyPartner = companyPartner != null ? companyPartner.Name : "",
                            Distributor = distributor != null ? distributor.Name : "-"
                        });
                    }

                    return returnList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Company GetCompanyById(int _companyId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var company = context.Company.FirstOrDefault(a => a.Id == _companyId && a.Status == 1);
                    return company != null ? company : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public CompanyRepo GetCompanyByIdForDisplaying(int _id)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var company = context.Company.FirstOrDefault(a => a.Id == _id && a.Status == 1);
                    if (company != null)
                    {

                        var companyUser = context.CompanyUser.FirstOrDefault(b => b.CompanyId == company.Id);
                        var user = context.User.FirstOrDefault(x => x.Id == companyUser.UserId && x.Status == 1);

                        var returnObj = new CompanyRepo()
                        {
                            Id = company.Id,
                            MainCompanyId = company.MainCompanyId,
                            CompanyTypeId = company.CompanyTypeId,
                            CompanyPartnerId = company.CompanyPartnerId,
                            PlatformId = company.PlatformId,
                            DistributorId = company.DistributorId,
                            SectorId = company.SectorId,
                            Address1 = company.Address1,
                            CompanyReportId = company.CompanyReportId,
                            CompanyCode = company.CompanyCode,
                            CompanyDescription = company.CompanyDescription,
                            Phone = company.Phone,
                            TechnicalReport = company.TechnicalReport,
                            TechnicalEmail1 = company.TechnicalEmail1,
                            TechnicalEmail2 = company.TechnicalEmail2,
                            TechnicalEmail3 = company.TechnicalEmail3,
                            CompanyStatus = company.CompanyStatus,
                            AlarmSms = company.AlarmSms,
                            PasswordControl = company.PasswordControl,
                            CompanyVehicleProgramming = company.CompanyVehicleProgramming,
                            CompanyGroupName = company.CompanyGroupName,
                            CreationDate = company.CreationDate,
                            UserId = user.Id,
                            UserCode = user.UserCode,
                            UserEmail = user.Mail,
                            UserName = user.Name,
                            UserPassword = user.Password,
                            UserSurname = user.Surname,
                            UserTelephone = user.Telephone,
                            UserTypeId = user.UserTypeId,
                            UserStatus = company.UserStatus,
                            AccountingCode = company.AccountingCode
                        };

                        return returnObj;
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteCompany(int _companyId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var company = context.Company.FirstOrDefault(a => a.Id == _companyId);
                    if (company != null)
                    {
                        company.Status = 0;
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
        private void UpdateObject(Company _newCompany, ref Company _oldCompany)
        {
            try
            {

                foreach (PropertyInfo CompanyPropInfo in _newCompany.GetType().GetProperties().ToList())
                {
                    _oldCompany.GetType().GetProperty(CompanyPropInfo.Name).SetValue(_oldCompany, _newCompany.GetType().GetProperty(CompanyPropInfo.Name).GetValue(_newCompany));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Company UpdateCompany(Company _Company)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldCompany = context.Company.FirstOrDefault(u => u.Id == _Company.Id);
                    if (oldCompany != null)
                    {
                        UpdateObject(_Company, ref oldCompany);
                        var numberOfUpdatedCompany = context.SaveChanges();
                        return numberOfUpdatedCompany > 0 ? _Company : null;
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
        public Company GetCompanyByName(string _name)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var company = context.Company.FirstOrDefault(x => x.CompanyDescription.Trim() == _name.Trim() && x.Status == 1);
                    if (company != null)
                    {
                        return company;
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Company> GetAllDBCompanies()
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
                            cmd.CommandText = "EXEC USP_GetAllCompanies";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<Company>>(json);
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
        public List<CompanyModelForExcel> GetAllDBCompaniesForExcel()
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
                            cmd.CommandText = "EXEC USP_GetCompanyListForExcell";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<CompanyModelForExcel>>(json);
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

        public List<Company> GetAllCompaniesByUserId(User _userObj) {
            try
            {
                using (var context= new VeraEntities())
                {
                    var companyList = GetAllDBCompanies();
                    var company = CompanyUserDB.GetInstance().GetCompanyByCompanyUserId(_userObj);
                    companyList = companyList.Where(x => x.MainCompanyId == company.Id || x.Id == company.Id).ToList();
                    if (companyList!=null)
                    {
                        return companyList;
                    }
                    return null;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
public class CompanyModelForExcel
{
    public string SirketKodu { get; set; }
    public string Sirket { get; set; }
    public string MobilSayisi { get; set; }
    public string IsOrtagi { get; set; }
    public string BayiAdi { get; set; }
    public string BaseMap { get; set; }
    public string TeknikRapor { get; set; }
    public string KullanimDurumu { get; set; }
    public string GirisZamani { get; set; }
    public string AlarmSms { get; set; }
    public string SifreKontrol { get; set; }
    public string InfoSense { get; set; }
    public string AccountingCode { get; set; }
    public string LoginUrl { get; set; }
    public string Platform { get; set; }
    public string Sektor { get; set; }
    public string Adres1 { get; set; }
    public string YetkiliKisi { get; set; }
    public string Telefon { get; set; }
    public string Adres2 { get; set; }
    public string VergiDairesi { get; set; }
    public string VergiNo { get; set; }
}