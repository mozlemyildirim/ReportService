using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class RouteDB
    {
        private static RouteDB instance;
        public static RouteDB GetInstance()
        {
            if (instance == null)
            {
                instance = new RouteDB();
            }
            return instance;
        }
        public Route AddNewRoute(Route _Route)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Route.Add(_Route);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _Route : null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public List<Route> GetAllRoutees()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var RouteList = context.Route.ToList();
                    return RouteList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Route GetRouteById(int _RouteId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Route = context.Route.FirstOrDefault(a => a.Id == _RouteId && a.Status == 1);
                    return Route != null ? Route : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public Route DeleteRoute(int _RouteId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Route = context.Route.FirstOrDefault(a => a.Id == _RouteId);
                    if (Route != null)
                    {
                        context.Route.Remove(Route);
                        int numOfDeleted = context.SaveChanges();
                        return numOfDeleted > 0 ? Route : null;
                    }
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void UpdateObject(Route _newRoute, ref Route _oldRoute)
        {
            try
            {

                foreach (PropertyInfo RoutePropInfo in _newRoute.GetType().GetProperties().ToList())
                {
                    _oldRoute.GetType().GetProperty(RoutePropInfo.Name).SetValue(_oldRoute, _newRoute.GetType().GetProperty(RoutePropInfo.Name).GetValue(_newRoute));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Route UpdateRoute(Route _Route)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldRoute = context.Route.FirstOrDefault(u => u.Id == _Route.Id);
                    if (oldRoute != null)
                    {
                        UpdateObject(_Route, ref oldRoute);
                        var numberOfUpdatedRoute = context.SaveChanges();
                        return numberOfUpdatedRoute > 0 ? _Route : null;
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
        public List<Route> GetRouteListOfCompany(string _routeNameTLA, User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var company = CompanyUserDB.GetInstance().GetCompanyByCompanyUserId(_userObj);
                    var companyRoutes = context.CompanyRoute.Where(x => x.CompanyId == company.Id).ToList();
                    var companyRoutesRouteIds = companyRoutes.Select(x => x.RouteId).ToList();
                    var routes = context.Route.Where(x => companyRoutesRouteIds.Contains(x.Id) && x.Status == 1).ToList();
                    if (!String.IsNullOrEmpty(_routeNameTLA))
                    {
                        routes = routes.Where(x => x.Name.Trim().ToLower().StartsWith(_routeNameTLA.Trim().ToLower()) && x.Status > 0).ToList();
                    }

                    if (routes != null)
                        return routes;
                    return null;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<Route> GetRouteList(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id);
                    var subCompaniesWithCompanyId = context.Company.Where(x => x.MainCompanyId == companyUser.CompanyId).Select(x => x.Id).ToList();
                    subCompaniesWithCompanyId.Add(companyUser.CompanyId);
                    var companyRoutesRouteIds = context.CompanyRoute.Where(x => subCompaniesWithCompanyId.Contains(x.CompanyId)).Select(x => x.RouteId).ToList();
                    var routes = context.Route.Where(x => companyRoutesRouteIds.Contains(x.Id) && x.Status == 1).ToList();
                    if (routes != null && routes.Count > 0)
                        return routes;
                    return null;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

    }
}
