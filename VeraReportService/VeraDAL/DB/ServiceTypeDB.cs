using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class ServiceTypeDB
	{
		private static ServiceTypeDB instance;
		public static ServiceTypeDB GetInstance()
		{
			if (instance == null)
			{
				instance = new ServiceTypeDB();
			}
			return instance;
		}
		public ServiceType AddNewServiceType(ServiceType _ServiceType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.ServiceType.Add(_ServiceType);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _ServiceType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<ServiceType> GetAllServiceTypes()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var ServiceTypeList = context.ServiceType.ToList();
					return ServiceTypeList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public ServiceType GetServiceTypeById(int _ServiceTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var ServiceType = context.ServiceType.FirstOrDefault(a => a.Id == _ServiceTypeId && a.Status == 1);
					return ServiceType != null ? ServiceType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteServiceType(int _ServiceTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var ServiceType = context.ServiceType.FirstOrDefault(a => a.Id == _ServiceTypeId);
					if (ServiceType != null)
					{
						ServiceType.Status = 0;
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
		private void UpdateObject(ServiceType _newServiceType, ref ServiceType _oldServiceType)
		{
			try
			{

				foreach (PropertyInfo ServiceTypePropInfo in _newServiceType.GetType().GetProperties().ToList())
				{
					_oldServiceType.GetType().GetProperty(ServiceTypePropInfo.Name).SetValue(_oldServiceType, _newServiceType.GetType().GetProperty(ServiceTypePropInfo.Name).GetValue(_newServiceType));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public ServiceType UpdateServiceType(ServiceType _ServiceType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldServiceType = context.ServiceType.FirstOrDefault(u => u.Id == _ServiceType.Id);
					if (oldServiceType != null)
					{
						UpdateObject(_ServiceType, ref oldServiceType);
						var numberOfUpdatedServiceType = context.SaveChanges();
						return numberOfUpdatedServiceType > 0 ? _ServiceType : null;
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
		public ServiceType GetServiceTypeByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var ServiceType = context.ServiceType.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (ServiceType == null)
					{
						ServiceType = new ServiceType()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.ServiceType.Add(ServiceType);
						context.SaveChanges();
						return ServiceType;
					}

					return ServiceType;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
