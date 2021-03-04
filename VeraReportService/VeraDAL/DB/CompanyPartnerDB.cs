using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class CompanyPartnerDB
	{
		private static CompanyPartnerDB instance;
		public static CompanyPartnerDB GetInstance()
		{
			if (instance == null)
			{
				instance = new CompanyPartnerDB();
			}
			return instance;
		}
		public CompanyPartner AddNewCompanyPartner(CompanyPartner _CompanyPartner)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.CompanyPartner.Add(_CompanyPartner);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _CompanyPartner : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<CompanyPartner> GetAllCompanyPartners()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var CompanyPartnerList = context.CompanyPartner.ToList();
					return CompanyPartnerList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public CompanyPartner GetCompanyPartnerById(int _CompanyPartnerId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var CompanyPartner = context.CompanyPartner.FirstOrDefault(a => a.Id == _CompanyPartnerId && a.Status == 1);
					return CompanyPartner != null ? CompanyPartner : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteCompanyPartner(int _CompanyPartnerId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var CompanyPartner = context.CompanyPartner.FirstOrDefault(a => a.Id == _CompanyPartnerId);
					if (CompanyPartner != null)
					{
						CompanyPartner.Status = 0;
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
		private void UpdateObject(CompanyPartner _newCompanyPartner, ref CompanyPartner _oldCompanyPartner)
		{
			try
			{

				foreach (PropertyInfo CompanyPartnerPropInfo in _newCompanyPartner.GetType().GetProperties().ToList())
				{
					_oldCompanyPartner.GetType().GetProperty(CompanyPartnerPropInfo.Name).SetValue(_oldCompanyPartner, _newCompanyPartner.GetType().GetProperty(CompanyPartnerPropInfo.Name).GetValue(_newCompanyPartner));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public CompanyPartner UpdateCompanyPartner(CompanyPartner _CompanyPartner)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldCompanyPartner = context.CompanyPartner.FirstOrDefault(u => u.Id == _CompanyPartner.Id);
					if (oldCompanyPartner != null)
					{
						UpdateObject(_CompanyPartner, ref oldCompanyPartner);
						var numberOfUpdatedCompanyPartner = context.SaveChanges();
						return numberOfUpdatedCompanyPartner > 0 ? _CompanyPartner : null;
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
		public CompanyPartner GetCompanyPartnerByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var CompanyPartner = context.CompanyPartner.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (CompanyPartner == null)
					{
						CompanyPartner = new CompanyPartner()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.CompanyPartner.Add(CompanyPartner);
						context.SaveChanges();
						return CompanyPartner;
					}

					return CompanyPartner;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
