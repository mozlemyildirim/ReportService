using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class Shift
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Name { get; set; }

        public bool? DataProcess { get; set; }

        public int Status { get; set; }

        public bool? Pazartesi { get; set; }

        public bool? Sali { get; set; }

        public bool? Carsamba { get; set; }

        public bool? Persembe { get; set; }

        public bool? Cuma { get; set; }

        public bool? Cumartesi { get; set; }

        public bool? Pazar { get; set; }

        public string StartHour { get; set; }

        public string EndHour { get; set; }

        public DateTime? CreationDate { get; set; }

    }

}
