using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class DeviceDataDB
	{
		private static DeviceDataDB instance;
		public static DeviceDataDB GetInstance()
		{
			if (instance == null)
			{
				instance = new DeviceDataDB();
			}
			return instance;
		}
		public DeviceData AddNewDeviceData(DeviceData _DeviceData)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.DeviceData.Add(_DeviceData);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _DeviceData : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<DeviceData> GetAllDeviceDataes()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var DeviceDataList = context.DeviceData.ToList();
					return DeviceDataList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public DeviceData GetDeviceDataById(int _DeviceDataId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var DeviceData = context.DeviceData.FirstOrDefault(a => a.Id == _DeviceDataId);
					return DeviceData != null ? DeviceData : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		private void UpdateObject(DeviceData _newDeviceData, ref DeviceData _oldDeviceData)
		{
			try
			{

				foreach (PropertyInfo DeviceDataPropInfo in _newDeviceData.GetType().GetProperties().ToList())
				{
					_oldDeviceData.GetType().GetProperty(DeviceDataPropInfo.Name).SetValue(_oldDeviceData, _newDeviceData.GetType().GetProperty(DeviceDataPropInfo.Name).GetValue(_newDeviceData));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public DeviceData UpdateDeviceData(DeviceData _DeviceData)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldDeviceData = context.DeviceData.FirstOrDefault(u => u.Id == _DeviceData.Id);
					if (oldDeviceData != null)
					{
						UpdateObject(_DeviceData, ref oldDeviceData);
						var numberOfUpdatedDeviceData = context.SaveChanges();
						return numberOfUpdatedDeviceData > 0 ? _DeviceData : null;
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
