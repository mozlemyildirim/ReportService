using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VeraDAL.Entities
{
	public class Device
	{
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int? DeviceMontageTypeId { get; set; }

        public int? UsingStatusId { get; set; }

        public int? MessagingId { get; set; }

        public int? RefTelemetryRecipeId { get; set; }

        public int? ServiceTypeId { get; set; }

        public int? WocheckerFlagId { get; set; }

        public int? VehicleTypeId { get; set; }

        public string DeviceId { get; set; }

        public int TerminalProtocolId { get; set; }

        public int TerminalTypeId { get; set; }

        public int SoftwareVersionId { get; set; }

        public int GatewayId { get; set; }

        public string CarPlateNumber { get; set; }

        public string DeviceGsmNumber { get; set; }

        public bool? RFIDEngineBlock { get; set; }

        public bool? RFIDEngineBlockStatus { get; set; }

        public int? ProgrammedLocationSendTime { get; set; }

        public int? ProgrammedLocationDistance { get; set; }

        public int? ProgrammedSpeedLimit { get; set; }

        public int? ProgrammedWaitingTime { get; set; }

        public int? ProgrammedHeartBeat { get; set; }

        public int? ActivityTime { get; set; }

        public string CameraURL { get; set; }

        public bool? IsSummerTime { get; set; }

        public int? TimeDifference { get; set; }

        public string IMEINumber { get; set; }

        public bool? Journey { get; set; }

        public bool? Telemetry { get; set; }

        public bool? DoorSensor { get; set; }

        public bool? HeatSensor { get; set; }

        public bool? EngineBlock { get; set; }

        public bool? TrafficData { get; set; }

        public bool? ConstantMobile { get; set; }

        public string ConstantLatitude { get; set; }

        public string ConstantLongitude { get; set; }

        public bool? GSensor { get; set; }

        public bool? DriverScoring { get; set; }

        public DateTime? CreationDate { get; set; }

        public string CustomerPlateNumber { get; set; }

        public string CarPhoneNumber { get; set; }

        public string AveaPlaka { get; set; }

        public string MobileNote { get; set; }

        public string TechnicalNote { get; set; }

        public int? OdometreStatusId { get; set; }

        public DateTime? LastActivityTime { get; set; }

        public bool? EngineBlockStatus { get; set; }

        public int? Status { get; set; }

        public string AccountingCode { get; set; }

        public string CCID { get; set; }

        public string BillNumber { get; set; }

        public DateTime? BillDate { get; set; }

        public string MontagePerson { get; set; }

        public DateTime? MontageDate { get; set; }

        public bool? VehicleBlock { get; set; }

        public decimal? VehicleKm { get; set; }

        public string VehiclePhone { get; set; }
        public int? LastDeviceDataId { get; set; }

    }

}
