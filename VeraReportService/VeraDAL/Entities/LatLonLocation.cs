using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class LatLonLocation
    {
        public int Id { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }

        public string Location { get; set; }

    }

}
