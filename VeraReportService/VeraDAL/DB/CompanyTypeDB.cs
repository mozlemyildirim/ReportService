using VeraDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace VeraDAL.DB
{
    public class CompanyTypeDB
    {
        private static CompanyTypeDB instance;
        public static CompanyTypeDB GetInstance()
        {
            if (instance == null)
            {
                instance = new CompanyTypeDB();
            }
            return instance;
        }
        public CompanyType AddNewCompanyType(CompanyType _CompanyType)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.CompanyType.Add(_CompanyType);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _CompanyType : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CompanyType> GetAllCompanyTypes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyTypeList = context.CompanyType.ToList();
                    return CompanyTypeList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CompanyType GetCompanyTypeById(int _CompanyTypeId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyType = context.CompanyType.FirstOrDefault(a => a.Id == _CompanyTypeId && a.Status == 1);
                    return CompanyType != null ? CompanyType : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteCompanyType(int _CompanyTypeId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyType = context.CompanyType.FirstOrDefault(a => a.Id == _CompanyTypeId);
                    if (CompanyType != null)
                    {
                        CompanyType.Status = 0;
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
		private void UpdateObject(CompanyType _newCompanyType, ref CompanyType _oldCompanyType)
		{
			try
			{

				foreach (PropertyInfo CompanyTypePropInfo in _newCompanyType.GetType().GetProperties().ToList())
				{
					_oldCompanyType.GetType().GetProperty(CompanyTypePropInfo.Name).SetValue(_oldCompanyType, _newCompanyType.GetType().GetProperty(CompanyTypePropInfo.Name).GetValue(_newCompanyType));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public CompanyType UpdateCompanyType(CompanyType _CompanyType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldCompanyType = context.CompanyType.FirstOrDefault(u => u.Id == _CompanyType.Id);
					if (oldCompanyType != null)
					{
						UpdateObject(_CompanyType, ref oldCompanyType);
						var numberOfUpdatedCompanyType = context.SaveChanges();
						return numberOfUpdatedCompanyType > 0 ? _CompanyType : null;
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
