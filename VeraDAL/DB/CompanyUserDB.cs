using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;
using VeraDAL.Models;

namespace VeraDAL.DB
{
	public class CompanyUserDB
	{

		private static CompanyUserDB instance;
		public static CompanyUserDB GetInstance()
		{
			if (instance == null)
			{
				instance = new CompanyUserDB();
			}
			return instance;
		}
		public CompanyUser AddNewCompanyUser(CompanyUser _CompanyUser)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.CompanyUser.Add(_CompanyUser);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _CompanyUser : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<CompanyUser> GetAllCompanyUsers()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var CompanyUserList = context.CompanyUser.Where(a => a.Status == 1).ToList();
					return CompanyUserList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public CompanyUser GetCompanyUserById(int _CompanyUserId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var CompanyUser = context.CompanyUser.FirstOrDefault(a => a.UserId == _CompanyUserId && a.Status == 1);
					return CompanyUser != null ? CompanyUser : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteCompanyUser(int _CompanyUserId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var CompanyUser = context.CompanyUser.FirstOrDefault(a => a.UserId == _CompanyUserId);
					if (CompanyUser != null)
					{
						CompanyUser.Status = 0;
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
		private void UpdateObject(CompanyUser _newCompanyUser, ref CompanyUser _oldCompanyUser)
		{
			try
			{

				foreach (PropertyInfo CompanyUserPropInfo in _newCompanyUser.GetType().GetProperties().ToList())
				{
					_oldCompanyUser.GetType().GetProperty(CompanyUserPropInfo.Name).SetValue(_oldCompanyUser, _newCompanyUser.GetType().GetProperty(CompanyUserPropInfo.Name).GetValue(_newCompanyUser));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public CompanyUser UpdateCompanyUser(CompanyUser _CompanyUser)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldCompanyUser = context.CompanyUser.FirstOrDefault(u => u.Id == _CompanyUser.Id);
					if (oldCompanyUser != null)
					{
						UpdateObject(_CompanyUser, ref oldCompanyUser);
						var numberOfUpdatedCompanyUser = context.SaveChanges();
						return numberOfUpdatedCompanyUser > 0 ? _CompanyUser : null;
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
		public List<UserRepo> GetUsersByCompanyId(int _id)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					List<User> list = null;
					var returnList = new List<UserRepo>();
					 
						//var myDistributor = context.Distributor.FirstOrDefault(x => x.UserId == _userObj.Id);
						//var myCompanies = context.Company.Where(x => x.DistributorId == myDistributor.Id).ToList();
						var myCompany = context.Company.FirstOrDefault(x=>x.Id==_id &&x.Status==1); 
						var myCompanyUsers = context.CompanyUser.Where(x => x.CompanyId==myCompany.Id && x.IsCompanyAdmin!=true).ToList();
						var myCompanyUserIds = myCompanyUsers.Select(x => x.UserId).ToList();
						list = context.User.Where(x => myCompanyUserIds.Contains(x.Id) && x.Status==1).ToList();
					 
					foreach (var item in list)
					{
						var authorizationType = context.CompanyUser.FirstOrDefault(x => x.UserId == item.Id).IsCompanyAdmin == true ? "Admin" : "User";
                        var geographicalAuthority = context.GeographicalAuthority.FirstOrDefault(x => x.Id == item.GeographicalAuthorityId).Name;

                        returnList.Add(new UserRepo()
						{
							Id = item.Id,
							Authorization = authorizationType,
							CompanyName = myCompany.CompanyDescription,
							Mail = item.Mail,
							Name = item.Name,
							Password = item.Password,
							Status = item.Status,
							Surname = item.Surname,
							Telephone = item.Telephone,
							UserCode = item.UserCode,
							UserTypeId = item.UserTypeId,
                            GeographicalAuthorityId=item.GeographicalAuthorityId,
                            GeographicalAuthority=geographicalAuthority 
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
        public Company GetCompanyByCompanyUserId(User _userObj)
        {
            try
            {
                using (var context =  new VeraEntities())
                {
                    var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                    var myCompany = context.Company.FirstOrDefault(x => x.Id == myCompanyId);
                    return myCompany;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CompanyUser GetCompanyUserByUserId(int _userId) {
            try
            {
                using (var context = new VeraEntities())
                {
                    var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == _userId);
                    return companyUser;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<User> GetCompanyUser(int _aracatamacompanyname)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var allDbCompanyUsers = GetAllCompanyUsers();
                    var myCompanyId = context.Company.FirstOrDefault(x => x.Id == _aracatamacompanyname).Id;
                    allDbCompanyUsers = allDbCompanyUsers.Where(x => x.CompanyId == myCompanyId).ToList();
                    var allDbCompanyUsersIds = allDbCompanyUsers.Select(x => x.UserId).ToList();
                    var users = context.User.Where(x => allDbCompanyUsersIds.Contains(x.Id)).ToList();
                    return users;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }

        }



    }
}
