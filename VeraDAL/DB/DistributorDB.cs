using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;
using VeraDAL.Models;

namespace VeraDAL.DB
{
	public class DistributorDB
	{
		private static DistributorDB instance;
		public static DistributorDB GetInstance()
		{
			if (instance == null)
			{
				instance = new DistributorDB();
			}
			return instance;
		}
		public Distributor AddNewDistributor(Distributor _distributor)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.Distributor.Add(_distributor);
					var numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _distributor : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<DistributorRepo> GetAllDistributors(User _userObj, string _bayiAdi = "")
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var returnList = new List<DistributorRepo>();


					List<Distributor> list = null;

					if (_userObj.UserTypeId == 2)
					{
						list = context.Distributor.Where(a => a.Status == 1 && a.UserId == _userObj.Id).ToList();
					}
					else if (_userObj.UserTypeId == 3)
					{
						list = context.Distributor.Where(a => a.Status == 1).ToList();
					}
					else
					{
						list = new List<Distributor>();
					}

					if (_bayiAdi != "")
					{ 
						list = list.Where(x => x.Name.ToLower().Contains(_bayiAdi.ToLower())).ToList();
                    }
					foreach (var item in list) 
					{
						var companyPartner = context.CompanyPartner.FirstOrDefault(x=>x.Id== item.CompanyPartnerId);
						//repoItems
						returnList.Add(new DistributorRepo()
						{
							Id = item.Id,
							Name = item.Name,
							CompanyPartnerId = item.CompanyPartnerId,
							EntranceDate = item.EntranceDate,
							ExitDate = item.ExitDate,
							Activity = item.Activity,
							Phone1 = item.Phone1,
							Phone2 = item.Phone2,
							Fax = item.Fax,
							City = item.City,
							PersonInCharge = item.PersonInCharge,
							Code = item.Code,
							Address = item.Address,
							UserId = item.UserId,
							CreationDate = item.CreationDate,
							Status = item.Status,
							CompanyPartner=companyPartner==null?null:companyPartner.Name 
						});
					}
					return returnList;

				}

			}
			catch (Exception)
			{

				throw;
			}

		}
		public DistributorRepo GetDistributorByIdForDisplaying(int _id)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var distributor = context.Distributor.FirstOrDefault(a => a.Id == _id && a.Status == 1);
					if (distributor != null)
					{
						var user = context.User.FirstOrDefault(a => a.Id == distributor.UserId);

						var returnObj = new DistributorRepo()
						{
							Id = distributor.Id,
							Name = distributor.Name,
							CompanyPartnerId = distributor.CompanyPartnerId,
							EntranceDate = distributor.EntranceDate,
							ExitDate = distributor.ExitDate,
							Activity = distributor.Activity,
							Phone1 = distributor.Phone1,
							Phone2 = distributor.Phone2,
							Fax = distributor.Fax,
							City = distributor.City,
							PersonInCharge = distributor.PersonInCharge,
							Code = distributor.Code,
							Address = distributor.Address,
							CreationDate = distributor.CreationDate,
							Status = distributor.Status,
							UserName = user.Name,
							UserSurname = user.Surname,
							UserId = user.Id,
							UserCode = user.UserCode,
							UserEmail = user.Mail,
							UserPassword = user.Password,
							UserTelephone = user.Telephone
						};

						return returnObj;
					}
					return null;
				}

			}
			catch (Exception)
			{

				throw;
			}
		}
		public bool DeleteDistributor(int _distributorId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var distributor = context.Distributor.FirstOrDefault(a => a.Id == _distributorId);
					if (distributor != null)
					{
						distributor.Status = 0;
						int numOfAffectedRows = context.SaveChanges();
						return numOfAffectedRows > 0;
					}
				}
				return false;
			}
			catch (Exception)
			{

				throw;
			}
		}
		private void UpdateObject(Distributor _newDistributor, ref Distributor _oldDistributor)
		{
			try
			{

				foreach (PropertyInfo DistributorPropInfo in _newDistributor.GetType().GetProperties().ToList())
				{
					_oldDistributor.GetType().GetProperty(DistributorPropInfo.Name).SetValue(_oldDistributor, _newDistributor.GetType().GetProperty(DistributorPropInfo.Name).GetValue(_newDistributor));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Distributor UpdateDistributor(Distributor _Distributor)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldDistributor = context.Distributor.FirstOrDefault(u => u.Id == _Distributor.Id);
					if (oldDistributor != null)
					{
						UpdateObject(_Distributor, ref oldDistributor);
						var numberOfUpdatedDistributor = context.SaveChanges();
						return numberOfUpdatedDistributor > 0 ? _Distributor : null;
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
		public bool CheckIfUserExist(string _userCode)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var control = context.User.FirstOrDefault(x => x.UserCode == _userCode);
					return control != null;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
		public int GetDistributorIdByName(string _distributorName) {
			try
			{
				using (var context = new VeraEntities())
				{
					int distributorId = context.Distributor.FirstOrDefault(x=>x.Name.Trim() == _distributorName.Trim() && x.Status==1).Id; 
				    return distributorId;
				 
				}
			}
			catch (Exception exc)
			{

				throw exc;
			}
		}
	}
}