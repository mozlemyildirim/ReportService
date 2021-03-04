using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
	public class SoftwareVersion
	{
		public int Id { get; set; }

		public string VersionCode { get; set; }

		public int TerminalTypeId { get; set; }

		public int Status { get; set; }
	}
}
