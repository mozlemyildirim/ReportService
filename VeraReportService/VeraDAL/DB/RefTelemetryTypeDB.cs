using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class RefTelemetryTypeDB
	{
		private static RefTelemetryTypeDB instance;
		public static RefTelemetryTypeDB GetInstance()
		{
			if (instance == null)
			{
				instance = new RefTelemetryTypeDB();
			}
			return instance;
		}
		public RefTelemetryType AddNewRefTelemetryType(RefTelemetryType _RefTelemetryType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.RefTelemetryType.Add(_RefTelemetryType);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _RefTelemetryType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<RefTelemetryType> GetAllRefTelemetryTypes()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var RefTelemetryTypeList = context.RefTelemetryType.ToList();
					return RefTelemetryTypeList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public RefTelemetryType GetRefTelemetryTypeById(int _RefTelemetryTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var RefTelemetryType = context.RefTelemetryType.FirstOrDefault(a => a.Id == _RefTelemetryTypeId && a.Status == 1);
					return RefTelemetryType != null ? RefTelemetryType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteRefTelemetryType(int _RefTelemetryTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var RefTelemetryType = context.RefTelemetryType.FirstOrDefault(a => a.Id == _RefTelemetryTypeId);
					if (RefTelemetryType != null)
					{
						RefTelemetryType.Status = 0;
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
		private void UpdateObject(RefTelemetryType _newRefTelemetryType, ref RefTelemetryType _oldRefTelemetryType)
		{
			try
			{

				foreach (PropertyInfo RefTelemetryTypePropInfo in _newRefTelemetryType.GetType().GetProperties().ToList())
				{
					_oldRefTelemetryType.GetType().GetProperty(RefTelemetryTypePropInfo.Name).SetValue(_oldRefTelemetryType, _newRefTelemetryType.GetType().GetProperty(RefTelemetryTypePropInfo.Name).GetValue(_newRefTelemetryType));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public RefTelemetryType UpdateRefTelemetryType(RefTelemetryType _RefTelemetryType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldRefTelemetryType = context.RefTelemetryType.FirstOrDefault(u => u.Id == _RefTelemetryType.Id);
					if (oldRefTelemetryType != null)
					{
						UpdateObject(_RefTelemetryType, ref oldRefTelemetryType);
						var numberOfUpdatedRefTelemetryType = context.SaveChanges();
						return numberOfUpdatedRefTelemetryType > 0 ? _RefTelemetryType : null;
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
		public RefTelemetryType GetRefTelemetryTypeByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var RefTelemetryType = context.RefTelemetryType.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (RefTelemetryType == null)
					{
						RefTelemetryType = new RefTelemetryType()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.RefTelemetryType.Add(RefTelemetryType);
						context.SaveChanges();
						return RefTelemetryType;
					}

					return RefTelemetryType;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
