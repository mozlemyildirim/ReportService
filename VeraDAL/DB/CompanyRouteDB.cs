using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class CompanyRouteDB
    {
        private static CompanyRouteDB instance;
        public static CompanyRouteDB GetInstance()
        {
            if (instance == null)
            {
                instance = new CompanyRouteDB();
            }
            return instance;
        }
        public CompanyRoute AddNewCompanyRoute(CompanyRoute _CompanyRoute)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.CompanyRoute.Add(_CompanyRoute);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _CompanyRoute : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CompanyRoute> GetAllCompanyRoutees()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyRouteList = context.CompanyRoute.ToList();
                    return CompanyRouteList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CompanyRoute GetCompanyRouteById(int _CompanyRouteId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyRoute = context.CompanyRoute.FirstOrDefault(a => a.Id == _CompanyRouteId && a.Status == 1);
                    return CompanyRoute != null ? CompanyRoute : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteCompanyRoute(int _CompanyRouteId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyRoute = context.CompanyRoute.FirstOrDefault(a => a.Id == _CompanyRouteId);
                    if (CompanyRoute != null)
                    {
                        CompanyRoute.Status = 0;
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
        public bool DeleteCompanyRouteWithRouteId(int _areaId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var CompanyRoute = context.CompanyRoute.FirstOrDefault(a => a.RouteId == _areaId);
                    if (CompanyRoute != null)
                    {
                        CompanyRoute.Status = 0;
                        int numOfDeleted = context.SaveChanges();
                        return numOfDeleted > 0;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        private void UpdateObject(CompanyRoute _newCompanyRoute, ref CompanyRoute _oldCompanyRoute)
        {
            try
            {

                foreach (PropertyInfo CompanyRoutePropInfo in _newCompanyRoute.GetType().GetProperties().ToList())
                {
                    _oldCompanyRoute.GetType().GetProperty(CompanyRoutePropInfo.Name).SetValue(_oldCompanyRoute, _newCompanyRoute.GetType().GetProperty(CompanyRoutePropInfo.Name).GetValue(_newCompanyRoute));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public CompanyRoute UpdateCompanyRoute(CompanyRoute _CompanyRoute)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldCompanyRoute = context.CompanyRoute.FirstOrDefault(u => u.Id == _CompanyRoute.Id);
                    if (oldCompanyRoute != null)
                    {
                        UpdateObject(_CompanyRoute, ref oldCompanyRoute);
                        var numberOfUpdatedCompanyRoute = context.SaveChanges();
                        return numberOfUpdatedCompanyRoute > 0 ? _CompanyRoute : null;
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
