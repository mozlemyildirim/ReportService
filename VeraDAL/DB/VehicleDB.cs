using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class VehicleDB
	{
		private static VehicleDB instance;
		public static VehicleDB GetInstance()
		{
			if (instance == null)
			{
				instance = new VehicleDB();
			}
			return instance;
		}
		public Vehicle AddNewVehicle(Vehicle _Vehicle)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.Vehicle.Add(_Vehicle);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _Vehicle : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<Vehicle> GetAllVehicles()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var VehicleList = context.Vehicle.ToList();
					return VehicleList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Vehicle GetVehicleById(int _VehicleId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Vehicle = context.Vehicle.FirstOrDefault(a => a.Id == _VehicleId && a.Status == 1);
					return Vehicle != null ? Vehicle : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteVehicle(int _VehicleId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Vehicle = context.Vehicle.FirstOrDefault(a => a.Id == _VehicleId);
					if (Vehicle != null)
					{
						Vehicle.Status = 0;
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
		private void UpdateObject(Vehicle _newVehicle, ref Vehicle _oldVehicle)
		{
			try
			{

				foreach (PropertyInfo VehiclePropInfo in _newVehicle.GetType().GetProperties().ToList())
				{
					_oldVehicle.GetType().GetProperty(VehiclePropInfo.Name).SetValue(_oldVehicle, _newVehicle.GetType().GetProperty(VehiclePropInfo.Name).GetValue(_newVehicle));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Vehicle UpdateVehicle(Vehicle _Vehicle)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldVehicle = context.Vehicle.FirstOrDefault(u => u.Id == _Vehicle.Id);
					if (oldVehicle != null)
					{
						UpdateObject(_Vehicle, ref oldVehicle);
						var numberOfUpdatedVehicle = context.SaveChanges();
						return numberOfUpdatedVehicle > 0 ? _Vehicle : null;
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
