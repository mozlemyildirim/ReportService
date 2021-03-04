using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class UserReport
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ReportId { get; set; }

        public int AutomaticReportingStatus { get; set; }

        public int UserReportType { get; set; }

        public string AutomaticReportingVehicles { get; set; }

    }
}
