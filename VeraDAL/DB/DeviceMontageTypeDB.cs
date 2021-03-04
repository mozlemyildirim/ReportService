using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class DeviceMontageTypeDB
	{
		private static DeviceMontageTypeDB instance;
		public static DeviceMontageTypeDB GetInstance()
		{
			if (instance == null)
			{
				instance = new DeviceMontageTypeDB();
			}
			return instance;
		}
		public DeviceMontageType AddNewDeviceMontageType(DeviceMontageType _DeviceMontageType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.DeviceMontageType.Add(_DeviceMontageType);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _DeviceMontageType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<DeviceMontageType> GetAllDeviceMontageTypes()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var DeviceMontageTypeList = context.DeviceMontageType.ToList();
					return DeviceMontageTypeList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public DeviceMontageType GetDeviceMontageTypeById(int _DeviceMontageTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var DeviceMontageType = context.DeviceMontageType.FirstOrDefault(a => a.Id == _DeviceMontageTypeId && a.Status == 1);
					return DeviceMontageType != null ? DeviceMontageType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteDeviceMontageType(int _DeviceMontageTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var DeviceMontageType = context.DeviceMontageType.FirstOrDefault(a => a.Id == _DeviceMontageTypeId);
					if (DeviceMontageType != null)
					{
						DeviceMontageType.Status = 0;
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
		private void UpdateObject(DeviceMontageType _newDeviceMontageType, ref DeviceMontageType _oldDeviceMontageType)
		{
			try
			{

				foreach (PropertyInfo DeviceMontageTypePropInfo in _newDeviceMontageType.GetType().GetProperties().ToList())
				{
					_oldDeviceMontageType.GetType().GetProperty(DeviceMontageTypePropInfo.Name).SetValue(_oldDeviceMontageType, _newDeviceMontageType.GetType().GetProperty(DeviceMontageTypePropInfo.Name).GetValue(_newDeviceMontageType));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public DeviceMontageType UpdateDeviceMontageType(DeviceMontageType _DeviceMontageType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldDeviceMontageType = context.DeviceMontageType.FirstOrDefault(u => u.Id == _DeviceMontageType.Id);
					if (oldDeviceMontageType != null)
					{
						UpdateObject(_DeviceMontageType, ref oldDeviceMontageType);
						var numberOfUpdatedDeviceMontageType = context.SaveChanges();
						return numberOfUpdatedDeviceMontageType > 0 ? _DeviceMontageType : null;
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
		public DeviceMontageType GetDeviceMontageTypeByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var DeviceMontageType = context.DeviceMontageType.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (DeviceMontageType == null)
					{
						DeviceMontageType = new DeviceMontageType()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.DeviceMontageType.Add(DeviceMontageType);
						context.SaveChanges();
						return DeviceMontageType;
					}

					return DeviceMontageType;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
