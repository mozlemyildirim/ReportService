using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;
using VeraDAL.Models;

namespace VeraDAL.DB
{
    public class UserDB
    {
        private static UserDB instance;
        public static UserDB GetInstance()
        {
            if (instance == null)
            {
                instance = new UserDB();
            }
            return instance;
        }
        public User AddNewUser(User _User)
        {
            try
            {
                using (var context = new VeraEntities())
                {

                    context.User.Add(_User);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _User : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<User> GetAllUsers()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserList = context.User.ToList();
                    return UserList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public User GetUserById(int _UserId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var User = context.User.FirstOrDefault(a => a.Id == _UserId && a.Status == 1);
                    return User != null ? User : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteUser(int _UserId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var User = context.User.FirstOrDefault(a => a.Id == _UserId);
                    if (User != null)
                    {
                        User.Status = 0;
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
        private void UpdateObject(User _newUser, ref User _oldUser)
        {
            try
            {

                foreach (PropertyInfo UserPropInfo in _newUser.GetType().GetProperties().ToList())
                {
                    _oldUser.GetType().GetProperty(UserPropInfo.Name).SetValue(_oldUser, _newUser.GetType().GetProperty(UserPropInfo.Name).GetValue(_newUser));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public User UpdateUser(User _User)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldUser = context.User.FirstOrDefault(u => u.Id == _User.Id);

                    if (oldUser != null)
                    {
                        UpdateObject(_User, ref oldUser);
                        var numberOfUpdatedUser = context.SaveChanges();
                        return numberOfUpdatedUser > 0 ? _User : null;
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
        public User ControlUser(string _userCode, string _userPassword)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var user = context.User.FirstOrDefault(x => x.UserCode == _userCode && x.Password == _userPassword && x.Status == 1);
                    if (user != null)
                        return user;
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UserRepo GetUserByUserObj(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                    var myCompany = context.Company.FirstOrDefault(x => x.Id == myCompanyId);
                    var userType = context.UserType.FirstOrDefault(x => x.Id == _userObj.UserTypeId);
                    var userRepo = new UserRepo()
                    {
                        Id = _userObj.Id,
                        Authorization = userType.Name,
                        Name = String.IsNullOrEmpty(_userObj.Name) ? "" : _userObj.Name,
                        UserCode = _userObj.UserCode,
                        UserTypeId = _userObj.UserTypeId,
                        CompanyName = myCompany.CompanyDescription,
                        CompanyId=myCompany.Id,
                        Mail = String.IsNullOrEmpty(_userObj.Mail) ? "" : _userObj.Mail,
                        Password = _userObj.Password,
                        Status = _userObj.Status,
                        Surname = String.IsNullOrEmpty(_userObj.Surname) ? "" : _userObj.Surname,
                        Telephone = String.IsNullOrEmpty(_userObj.Telephone) ? "" : _userObj.Telephone,
                        HomepageRefreshTime=_userObj.HomepageRefreshTime
                    };
                    return userRepo;
                }
            }
            catch (Exception)
            {

                throw;
               
            }



        }
        public int GetCompanyIdByUserObj(User _userObj)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var companyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                    return companyId;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<UserRepo> GetAllUserList(User _userObj, string _userCodeTF = "", string _userNameTF = "", string _userSurnameTF = "", int _userAuthorityTF = 0)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var returnList = new List<UserRepo>();
                    List<User> list = context.User.ToList();

                    var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id);
                    var myCompany = context.Company.FirstOrDefault(x => x.Id == companyUser.CompanyId);
                    var myCompanyUsers = context.CompanyUser.Where(x => x.CompanyId == myCompany.Id).ToList();
                    if (_userAuthorityTF != 0)
                    {
                        bool isCompanyAdmin = _userAuthorityTF == 2 ? true : false;
                        myCompanyUsers = myCompanyUsers.Where(x => x.IsCompanyAdmin == isCompanyAdmin).ToList();
                    }
                    var myCompanyUsersUserIds = myCompanyUsers.Select(x => x.UserId);
                    list = list.Where(x => myCompanyUsersUserIds.Contains(x.Id) && x.Status == 1).ToList();


                    if (_userCodeTF != "")
                    {
                        list = list.Where(x => x.UserCode.ToLower().Contains(_userCodeTF.Trim().ToLower())).ToList();
                    }
                    if (_userNameTF != "")
                    {
                        list = list.Where(x => x.Name.Trim().ToLower().Contains(_userNameTF.Trim().ToLower())).ToList();
                    }
                    if (_userSurnameTF != "")
                    {
                        list = list.Where(x => x.Surname.Trim().ToLower().Contains(_userSurnameTF.Trim().ToLower())).ToList();
                    }

                    if (companyUser.IsCompanyAdmin)
                    {
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
                                GeographicalAuthorityId = item.GeographicalAuthorityId,
                                GeographicalAuthority = geographicalAuthority
                            });
                        }
                        return returnList;
                    }
                    return null;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        } 
        public UserRepo GetUserByIdUserRepo(int _id)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var user = context.User.FirstOrDefault(x => x.Id == _id);
                    var companyUser = context.CompanyUser.FirstOrDefault(x => x.UserId == user.Id);
                    var company = context.Company.FirstOrDefault(x => x.Id == companyUser.CompanyId);
                    
                    UserRepo userRepo = new UserRepo()
                    {
                        CompanyName = company.CompanyDescription,
                        Id = user.Id,
                        AuthorizationTypeId = companyUser.IsCompanyAdmin==true?1:0,
                        Mail = user.Mail,
                        Name = user.Name,
                        GeographicalAuthorityId = user.GeographicalAuthorityId,
                        Password = user.Password,
                        Status = user.Status,
                        Surname = user.Surname,
                        Telephone = user.Telephone,
                        UserCode = user.UserCode,
                        UserTypeId = user.UserTypeId
                    };
                    return userRepo;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<User> GetUserListOfCompany(User _userObj)
        {
            try
            {
                using (var context= new VeraEntities())
                {
                    var myCompanyId = context.CompanyUser.FirstOrDefault(x => x.UserId == _userObj.Id).CompanyId;
                    var myCompanyUsers = context.CompanyUser.Where(x => x.CompanyId == myCompanyId).ToList();
                    var myCompanyUsersUserIds = myCompanyUsers.Select(x => x.UserId).ToList();
                    var companyUsers = context.User.Where(x => myCompanyUsersUserIds.Contains(x.Id) && x.Status==1).ToList();
                    return companyUsers;
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }


    }
}
