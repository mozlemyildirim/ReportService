using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VeraDAL.Entities
{
	public class Company
	{
        public int Id { get; set; }

        public string CompanyCode { get; set; }

        public int? CompanyPartnerId { get; set; }

        public int? MainCompanyId { get; set; }

        public int? CompanyTypeId { get; set; }

        public int? PlatformId { get; set; }

        public int DistributorId { get; set; }

        public int? SectorId { get; set; }

        public int? CompanyReportId { get; set; }

        public string CompanyDescription { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Phone { get; set; }

        public bool? TechnicalReport { get; set; }

        public string TechnicalEmail1 { get; set; }

        public string TechnicalEmail2 { get; set; }

        public string TechnicalEmail3 { get; set; }

        public bool? CompanyStatus { get; set; }

        public bool? AlarmSms { get; set; }

        public bool? PasswordControl { get; set; }

        public bool? CompanyVehicleProgramming { get; set; }

        public string CompanyGroupName { get; set; }

        public DateTime? CreationDate { get; set; }

        public int Status { get; set; }

        public string AccountingCode { get; set; }

        public bool? InfoSense { get; set; }

        public string TaxNumber { get; set; }

        public string TaxAdministration { get; set; }

        public string LoginUrl { get; set; }

        public int? MobileCount { get; set; }

        public string BaseMap { get; set; }

        public string Fax { get; set; }

        public DateTime? EntranceDate { get; set; }

        public DateTime? ExitDate { get; set; }

        public bool? UserStatus { get; set; }

        public string PersonInCharge { get; set; }


    }

}
