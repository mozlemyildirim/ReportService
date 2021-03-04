using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Text;
namespace VeraDAL.Entities
{
	public class VeraEntities:DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            optionsBuilder.UseSqlServer("Server=89.19.10.194; Database=Vera; User Id=sa; Password=(__!VeraMobil!__2019);"); 
		}
		public DbSet<Admin> Admin { get; set; }
		public DbSet<Company> Company { get; set; }
		public DbSet<Device> Device { get; set; }
		public DbSet<CompanyReport> CompanyReport { get; set; }
		public DbSet<CompanyType> CompanyType { get; set; }
		public DbSet<DeviceMontageType> DeviceMontageType { get; set; }
		public DbSet<Distributor> Distributor { get; set; }
		public DbSet<Gateway> Gateway { get; set; }
		public DbSet<Messaging> Messaging { get; set; }
		public DbSet<Platform> Platform { get; set; }
		public DbSet<RefTelemetryType> RefTelemetryType { get; set; }
		public DbSet<Report> Report { get; set; }
		public DbSet<Sector> Sector { get; set; }
		public DbSet<ServiceType> ServiceType { get; set; }
		public DbSet<SoftwareVersion> SoftwareVersion { get; set; }
		public DbSet<TerminalProtocol> TerminalProtocol { get; set; }
		public DbSet<TerminalType> TerminalType { get; set; }
		public DbSet<UsingStatus> UsingStatus { get; set; }
		public DbSet<Vehicle> Vehicle { get; set; } 
		public DbSet<WocheckerFlag> WocheckerFlag { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<CompanyPartner> CompanyPartner { get; set; }
		public DbSet<OdometreStatus> OdometreStatus { get; set; }
		public DbSet<CompanyUser> CompanyUser { get; set; }
		public DbSet<DeviceData> DeviceData { get; set; }
        public DbSet<CompanyGroup> CompanyGroup { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupDevice> GroupDevice { get; set; }
        public DbSet<UserType> UserType{ get; set; }
        public DbSet<GeographicalAuthority> GeographicalAuthority { get; set; }
        public DbSet<UserQuestionAnswer> UserQuestionAnswer { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<CompanyShift> CompanyShift { get; set; }
        public DbSet<UserDevice> UserDevice { get; set; }
        public DbSet<CompanyArea> CompanyArea{ get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<LatLonLocation> LatLonLocation{ get; set; }
        public DbSet<TollBooth> TollBooth{ get; set; }
        public DbSet<UserReport> UserReport { get; set; }
        public DbSet<AreaDevice> AreaDevice{ get; set; }
        public DbSet<Settings> Settings{ get; set; }
        public DbSet<Route> Route { get; set; }
        public DbSet<CompanyRoute> CompanyRoute { get; set; }
        public DbSet<RouteDevice> RouteDevice { get; set; }
    }
}
