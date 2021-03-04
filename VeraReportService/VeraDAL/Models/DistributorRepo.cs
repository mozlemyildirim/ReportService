using System;
using System.Collections.Generic;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.Models
{
	public class DistributorRepo:Distributor
	{
		public string UserName { get; set; }
		public string UserSurname { get; set; }
		public string UserCode { get; set; }
		public string UserPassword { get; set; }
		public string UserTelephone { get; set; }
		public string UserEmail { get; set; }
		public string CompanyPartner { get; set; }
	}
}
