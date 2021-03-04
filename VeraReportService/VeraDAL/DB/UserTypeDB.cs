using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class UserTypeDB
    {
        private static UserTypeDB instance;
        public static UserTypeDB GetInstance()
        {
            if (instance == null)
            {
                instance = new UserTypeDB();
            }
            return instance;
        }
        public UserType AddNewUserType(UserType _UserType)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.UserType.Add(_UserType);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _UserType : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<UserType> GetAllUserTypes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserTypeList = context.UserType.ToList();
                    return UserTypeList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
       
         
        private void UpdateObject(UserType _newUserType, ref UserType _oldUserType)
        {
            try
            {

                foreach (PropertyInfo UserTypePropInfo in _newUserType.GetType().GetProperties().ToList())
                {
                    _oldUserType.GetType().GetProperty(UserTypePropInfo.Name).SetValue(_oldUserType, _newUserType.GetType().GetProperty(UserTypePropInfo.Name).GetValue(_newUserType));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public UserType UpdateUserType(UserType _UserType)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldUserType = context.UserType.FirstOrDefault(u => u.Id == _UserType.Id);
                    if (oldUserType != null)
                    {
                        UpdateObject(_UserType, ref oldUserType);
                        var numberOfUpdatedUserType = context.SaveChanges();
                        return numberOfUpdatedUserType > 0 ? _UserType : null;
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
