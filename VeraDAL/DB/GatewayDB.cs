using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class GatewayDB
	{
		private static GatewayDB instance;
		public static GatewayDB GetInstance()
		{
			if (instance == null)
			{
				instance = new GatewayDB();
			}
			return instance;
		}
		public Gateway AddNewGateway(Gateway _Gateway)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.Gateway.Add(_Gateway);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _Gateway : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<Gateway> GetAllGateways()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var GatewayList = context.Gateway.ToList();
					return GatewayList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Gateway GetGatewayById(int _GatewayId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Gateway = context.Gateway.FirstOrDefault(a => a.Id == _GatewayId && a.Status == 1);
					return Gateway != null ? Gateway : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteGateway(int _GatewayId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Gateway = context.Gateway.FirstOrDefault(a => a.Id == _GatewayId);
					if (Gateway != null)
					{
						Gateway.Status = 0;
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
		private void UpdateObject(Gateway _newGateway, ref Gateway _oldGateway)
		{
			try
			{

				foreach (PropertyInfo GatewayPropInfo in _newGateway.GetType().GetProperties().ToList())
				{
					_oldGateway.GetType().GetProperty(GatewayPropInfo.Name).SetValue(_oldGateway, _newGateway.GetType().GetProperty(GatewayPropInfo.Name).GetValue(_newGateway));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Gateway UpdateGateway(Gateway _Gateway)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldGateway = context.Gateway.FirstOrDefault(u => u.Id == _Gateway.Id);
					if (oldGateway != null)
					{
						UpdateObject(_Gateway, ref oldGateway);
						var numberOfUpdatedGateway = context.SaveChanges();
						return numberOfUpdatedGateway > 0 ? _Gateway : null;
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
		public Gateway GetGatewayByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Gateway = context.Gateway.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (Gateway == null)
					{
						Gateway = new Gateway()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.Gateway.Add(Gateway);
						context.SaveChanges();
						return Gateway;
					}

					return Gateway;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
