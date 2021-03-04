using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
	public class CompanyUser
	{
		public int Id { get; set; } 
		public int UserId { get; set; } 
		public int CompanyId { get; set; } 
		public bool IsCompanyAdmin { get; set; } 
		public int Status { get; set; }
	}
}
