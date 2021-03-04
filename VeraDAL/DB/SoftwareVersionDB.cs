using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class SoftwareVersionDB
	{
		private static SoftwareVersionDB instance;
		public static SoftwareVersionDB GetInstance()
		{
			if (instance == null)
			{
				instance = new SoftwareVersionDB();
			}
			return instance;
		}
		public SoftwareVersion AddNewSoftwareVersion(SoftwareVersion _SoftwareVersion)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.SoftwareVersion.Add(_SoftwareVersion);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _SoftwareVersion : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<SoftwareVersion> GetAllSoftwareVersions()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var SoftwareVersionList = context.SoftwareVersion.ToList();
					return SoftwareVersionList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public SoftwareVersion GetSoftwareVersionById(int _SoftwareVersionId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var SoftwareVersion = context.SoftwareVersion.FirstOrDefault(a => a.Id == _SoftwareVersionId && a.Status == 1);
					return SoftwareVersion != null ? SoftwareVersion : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteSoftwareVersion(int _SoftwareVersionId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var SoftwareVersion = context.SoftwareVersion.FirstOrDefault(a => a.Id == _SoftwareVersionId);
					if (SoftwareVersion != null)
					{
						SoftwareVersion.Status = 0;
						int numOfDeleted = context.SaveChanges();
						return numOfDeleted > 0;
					}
				}
				return false;
			}
			catch (Exception)
			{

				throw;
			}

		}
		private void UpdateObject(SoftwareVersion _newSoftwareVersion, ref SoftwareVersion _oldSoftwareVersion)
		{
			try
			{

				foreach (PropertyInfo SoftwareVersionPropInfo in _newSoftwareVersion.GetType().GetProperties().ToList())
				{
					_oldSoftwareVersion.GetType().GetProperty(SoftwareVersionPropInfo.Name).SetValue(_oldSoftwareVersion, _newSoftwareVersion.GetType().GetProperty(SoftwareVersionPropInfo.Name).GetValue(_newSoftwareVersion));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public SoftwareVersion UpdateSoftwareVersion(SoftwareVersion _SoftwareVersion)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldSoftwareVersion = context.SoftwareVersion.FirstOrDefault(u => u.Id == _SoftwareVersion.Id);
					if (oldSoftwareVersion != null)
					{
						UpdateObject(_SoftwareVersion, ref oldSoftwareVersion);
						var numberOfUpdatedSoftwareVersion = context.SaveChanges();
						return numberOfUpdatedSoftwareVersion > 0 ? _SoftwareVersion : null;
					}

					return null;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public SoftwareVersion GetSoftwareVersionByNameOrInsert(string _name, int _terminalTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var SoftwareVersion = context.SoftwareVersion.FirstOrDefault(x => x.VersionCode.Trim() == _name.Trim() && x.Status > 0 && x.TerminalTypeId == _terminalTypeId);

					if (SoftwareVersion == null)
					{
						SoftwareVersion = new SoftwareVersion()
						{
							VersionCode = _name.Trim(),
							Status = 1,
							TerminalTypeId = _terminalTypeId
						};

						context.SoftwareVersion.Add(SoftwareVersion);
						context.SaveChanges();
						return SoftwareVersion;
					}

					return SoftwareVersion;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
