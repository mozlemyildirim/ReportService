using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraDAL.Entities
{
    public class Route
    {
        public int Id { get; set; }

        public string LatsLongs { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

    }
}
