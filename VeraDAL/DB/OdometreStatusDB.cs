using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class OdometreStatusDB
	{
		private static OdometreStatusDB instance;
		public static OdometreStatusDB GetInstance()
		{
			if (instance == null)
			{
				instance = new OdometreStatusDB();
			}
			return instance;
		}
		public OdometreStatus AddNewOdometreStatus(OdometreStatus _OdometreStatus)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.OdometreStatus.Add(_OdometreStatus);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _OdometreStatus : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<OdometreStatus> GetAllOdometreStatuss()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var OdometreStatusList = context.OdometreStatus.ToList();
					return OdometreStatusList;
				}
			}
			catch (Exception exc)
			{

				throw exc;
			}
		}
		public OdometreStatus GetOdometreStatusById(int _OdometreStatusId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var OdometreStatus = context.OdometreStatus.FirstOrDefault(a => a.Id == _OdometreStatusId && a.Status == 1);
					return OdometreStatus != null ? OdometreStatus : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteOdometreStatus(int _OdometreStatusId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var OdometreStatus = context.OdometreStatus.FirstOrDefault(a => a.Id == _OdometreStatusId);
					if (OdometreStatus != null)
					{
						OdometreStatus.Status = 0;
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
		private void UpdateObject(OdometreStatus _newOdometreStatus, ref OdometreStatus _oldOdometreStatus)
		{
			try
			{

				foreach (PropertyInfo OdometreStatusPropInfo in _newOdometreStatus.GetType().GetProperties().ToList())
				{
					_oldOdometreStatus.GetType().GetProperty(OdometreStatusPropInfo.Name).SetValue(_oldOdometreStatus, _newOdometreStatus.GetType().GetProperty(OdometreStatusPropInfo.Name).GetValue(_newOdometreStatus));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public OdometreStatus UpdateOdometreStatus(OdometreStatus _OdometreStatus)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldOdometreStatus = context.OdometreStatus.FirstOrDefault(u => u.Id == _OdometreStatus.Id);
					if (oldOdometreStatus != null)
					{
						UpdateObject(_OdometreStatus, ref oldOdometreStatus);
						var numberOfUpdatedOdometreStatus = context.SaveChanges();
						return numberOfUpdatedOdometreStatus > 0 ? _OdometreStatus : null;
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

		public OdometreStatus GetOdometreStatusByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var OdometreStatus = context.OdometreStatus.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (OdometreStatus == null)
					{
						OdometreStatus = new OdometreStatus()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.OdometreStatus.Add(OdometreStatus);
						context.SaveChanges();
						return OdometreStatus;
					}

					return OdometreStatus;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
