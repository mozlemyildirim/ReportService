using System;
using System.Collections.Generic;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.Models
{
    public class GroupDeviceRepo:GroupDevice
    {
        public string GroupName { get; set; }
        public string CarPlateNumber { get; set; }

    }
}
