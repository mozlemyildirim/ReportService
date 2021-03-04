using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class AdminDB
	{
		private static AdminDB instance;
		public static AdminDB GetInstance()
		{
			if (instance == null)
				instance = new AdminDB();
			return instance;
		}
		public List<Admin> GetAdminList()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					return context.Admin.ToList();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Admin GetAdminById(int _adminId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var admin = context.Admin.FirstOrDefault(a => a.Id == _adminId && a.Status == 1);
					return admin != null ? admin : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public bool DeleteAdmin(int _adminId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var admin = context.Admin.FirstOrDefault(a => a.Id == _adminId);
					if (admin != null)
					{
						admin.Status = 0;
						int numOfDeleted = context.SaveChanges();
						if (numOfDeleted > 0)
						{
							return true;
						}
					}
					return false;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Admin AddNewAdmin(Admin _admin)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.Admin.Add(_admin);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _admin : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		private void UpdateObject(Admin _newAdmin, ref Admin _oldAdmin)
		{
			try
			{

				foreach (PropertyInfo AdminPropInfo in _newAdmin.GetType().GetProperties().ToList())
				{
					_oldAdmin.GetType().GetProperty(AdminPropInfo.Name).SetValue(_oldAdmin, _newAdmin.GetType().GetProperty(AdminPropInfo.Name).GetValue(_newAdmin));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		} 
		public Admin UpdateAdmin(Admin _Admin)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldAdmin = context.Admin.FirstOrDefault(u => u.Id == _Admin.Id);
					if (oldAdmin != null)
					{
						UpdateObject(_Admin, ref oldAdmin);
						var numberOfUpdatedAdmin = context.SaveChanges();
						return numberOfUpdatedAdmin > 0 ? _Admin : null;
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
