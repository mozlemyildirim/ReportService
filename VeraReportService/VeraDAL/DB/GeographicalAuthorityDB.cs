using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class GeographicalAuthorityDB
    {
        private static GeographicalAuthorityDB instance;
        public static GeographicalAuthorityDB GetInstance()
        {
            if (instance == null)
            {
                instance = new GeographicalAuthorityDB();
            }
            return instance;
        }
        public GeographicalAuthority AddNewGeographicalAuthority(GeographicalAuthority _GeographicalAuthority)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.GeographicalAuthority.Add(_GeographicalAuthority);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _GeographicalAuthority : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<GeographicalAuthority> GetAllGeographicalAuthoritys()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var GeographicalAuthorityList = context.GeographicalAuthority.ToList();
                    return GeographicalAuthorityList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        private void UpdateObject(GeographicalAuthority _newGeographicalAuthority, ref GeographicalAuthority _oldGeographicalAuthority)
        {
            try
            {

                foreach (PropertyInfo GeographicalAuthorityPropInfo in _newGeographicalAuthority.GetType().GetProperties().ToList())
                {
                    _oldGeographicalAuthority.GetType().GetProperty(GeographicalAuthorityPropInfo.Name).SetValue(_oldGeographicalAuthority, _newGeographicalAuthority.GetType().GetProperty(GeographicalAuthorityPropInfo.Name).GetValue(_newGeographicalAuthority));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public GeographicalAuthority UpdateGeographicalAuthority(GeographicalAuthority _GeographicalAuthority)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldGeographicalAuthority = context.GeographicalAuthority.FirstOrDefault(u => u.Id == _GeographicalAuthority.Id);
                    if (oldGeographicalAuthority != null)
                    {
                        UpdateObject(_GeographicalAuthority, ref oldGeographicalAuthority);
                        var numberOfUpdatedGeographicalAuthority = context.SaveChanges();
                        return numberOfUpdatedGeographicalAuthority > 0 ? _GeographicalAuthority : null;
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
