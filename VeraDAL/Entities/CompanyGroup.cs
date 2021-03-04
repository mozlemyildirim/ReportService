using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class CompanyGroup
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int CompanyId { get; set; }
        public int Status { get; set; }

    }

}
