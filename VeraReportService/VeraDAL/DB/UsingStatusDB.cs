using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class UsingStatusDB
	{
		private static UsingStatusDB instance;
		public static UsingStatusDB GetInstance()
		{
			if (instance == null)
			{
				instance = new UsingStatusDB();
			}
			return instance;
		}
		public UsingStatus AddNewUsingStatus(UsingStatus _UsingStatus)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.UsingStatus.Add(_UsingStatus);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _UsingStatus : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<UsingStatus> GetAllUsingStatuses()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var UsingStatusList = context.UsingStatus.ToList();
					return UsingStatusList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public UsingStatus GetUsingStatusById(int _UsingStatusId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var UsingStatus = context.UsingStatus.FirstOrDefault(a => a.Id == _UsingStatusId && a.Status == 1);
					return UsingStatus != null ? UsingStatus : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteUsingStatus(int _UsingStatusId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var UsingStatus = context.UsingStatus.FirstOrDefault(a => a.Id == _UsingStatusId);
					if (UsingStatus != null)
					{
						UsingStatus.Status = 0;
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
		private void UpdateObject(UsingStatus _newUsingStatus, ref UsingStatus _oldUsingStatus)
		{
			try
			{

				foreach (PropertyInfo UsingStatusPropInfo in _newUsingStatus.GetType().GetProperties().ToList())
				{
					_oldUsingStatus.GetType().GetProperty(UsingStatusPropInfo.Name).SetValue(_oldUsingStatus, _newUsingStatus.GetType().GetProperty(UsingStatusPropInfo.Name).GetValue(_newUsingStatus));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public UsingStatus UpdateUsingStatus(UsingStatus _UsingStatus)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldUsingStatus = context.UsingStatus.FirstOrDefault(u => u.Id == _UsingStatus.Id);
					if (oldUsingStatus != null)
					{
						UpdateObject(_UsingStatus, ref oldUsingStatus);
						var numberOfUpdatedUsingStatus = context.SaveChanges();
						return numberOfUpdatedUsingStatus > 0 ? _UsingStatus : null;
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
	}
}
