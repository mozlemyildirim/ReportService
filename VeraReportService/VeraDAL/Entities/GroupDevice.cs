using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class GroupDevice
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public int GroupId { get; set; }
        public int Status { get; set; }
    }

}
