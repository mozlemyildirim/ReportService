using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
	public class DeviceData
	{
		public int Id { get; set; }

		public string MessageType { get; set; }

		public int DeviceId { get; set; }

		public string IoStatus { get; set; }

		public decimal Latitude { get; set; }

		public decimal Longtitude { get; set; }

		public int KmPerHour { get; set; }

		public int TotalKm { get; set; }

		public int DistanceBetweenTwoPackages { get; set; }

		public decimal DirectionDegree { get; set; }

		public DateTime GpsDateTime { get; set; }

		public decimal Altitude { get; set; }

		public DateTime CreateDate { get; set; }
	}
}
