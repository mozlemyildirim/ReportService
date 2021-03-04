using VeraDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Models
{
    public class CompanyRepo : Company
    {
        public string PlatformName { get; set; }
        public string SectorName { get; set; }
        public string ReportName { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserSurname { get; set; }
		public string UserCode { get; set; }
		public string UserPassword { get; set; }
		public string UserTelephone { get; set; }
		public string UserEmail { get; set; }
		public int UserTypeId { get; set; }
		public int DeviceNumberOfCompany { get; set; }
		public string CompanyPartner { get; set; }
		public string Distributor { get; set; }

	}
}
