using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VeraDAL.Entities
{
	public class Distributor
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int? CompanyPartnerId { get; set; }

		public DateTime? EntranceDate { get; set; }

		public DateTime? ExitDate { get; set; }

		public bool? Activity { get; set; }

		public string Phone1 { get; set; }

		public string Phone2 { get; set; }

		public string Fax { get; set; }

		public string City { get; set; }

		public string PersonInCharge { get; set; }

		public string Code { get; set; }

		public string Address { get; set; }

		public DateTime? CreationDate { get; set; }

		public int Status { get; set; }

		public int? UserId { get; set; }


	}

}
