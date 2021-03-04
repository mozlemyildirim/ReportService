using System;
using System.Collections.Generic;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.Models
{
	public class UserRepo:User

	{
		public string CompanyName { get; set; }
		public string Authorization { get; set; }
		public int AuthorizationTypeId { get; set; }
        public string GeographicalAuthority { get; set; }
        public int? HomepageRefreshTime { get; set; }
        public int CompanyId { get; set; }
    }
}
