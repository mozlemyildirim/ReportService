using System;
using System.Collections.Generic;
using System.Text;
using VeraDAL.Entities; 

namespace VeraDAL.Models

{
	public class DeviceRepo : Device
	{
		//
		public string Gateway { get; set; }
		public string TerminalType { get; set; }
		public string TerminalProtocol { get; set; }
		public string SoftwareVersion { get; set; }
		public string ServiceType { get; set; }
		public string DeviceMontageType { get; set; }
		public string UsingStatus { get; set; }
		public string Messaging { get; set; }
		public string RefTelemetryRecipe { get; set; }
		public string WocheckerFlag { get; set; }
		public string VehicleType { get; set; }
		public string OdometreTypeName { get; set; }
		public string CompanyName { get; set; }
		public string DistributorName { get; set; }


		public bool Aktiflik { get; set; }
		public bool Kontak { get; set; }
		public bool MotorBlokajı { get; set; }
		public string SonAktiflikZamani { get; set; }
		public string SonKonumZamani { get; set; }
		public int AracSonKmBilgisi { get; set; }
		public string SonKonum { get; set; }
		public int AnlikHiz { get; set; }
		public string CalismaSuresi { get; set; }



	}
}
