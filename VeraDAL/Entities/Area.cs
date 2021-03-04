using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class Area
    {
        public int Id { get; set; }

        public string LatsLongs { get; set; }

        public string Name { get; set; }

        public bool? IsRestricted { get; set; }

        public int Status { get; set; }

    }
}
