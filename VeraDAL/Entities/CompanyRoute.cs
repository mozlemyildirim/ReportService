using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraDAL.Entities
{
    public class CompanyRoute
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int RouteId { get; set; }

        public int Status { get; set; }

    }
}
