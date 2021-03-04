using System;
using System.Collections.Generic;
using System.Text;
using VeraDAL.Entities;
using System.Linq;
using System.Reflection;

namespace VeraDAL.DB
{
    public class UserQuestionAnswerDB
    {
        private static UserQuestionAnswerDB instance;
        public static UserQuestionAnswerDB GetInstance()
        {
            if (instance == null)
            {
                instance = new UserQuestionAnswerDB();
            }
            return instance;
        }
        public UserQuestionAnswer AddNewUserQuestionAnswer(UserQuestionAnswer _UserQuestionAnswer)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.UserQuestionAnswer.Add(_UserQuestionAnswer);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _UserQuestionAnswer : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<UserQuestionAnswer> GetAllUserQuestionAnsweres()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var UserQuestionAnswerList = context.UserQuestionAnswer.ToList();
                    return UserQuestionAnswerList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void UpdateObject(UserQuestionAnswer _newUserQuestionAnswer, ref UserQuestionAnswer _oldUserQuestionAnswer)
        {
            try
            {

                foreach (PropertyInfo UserQuestionAnswerPropInfo in _newUserQuestionAnswer.GetType().GetProperties().ToList())
                {
                    _oldUserQuestionAnswer.GetType().GetProperty(UserQuestionAnswerPropInfo.Name).SetValue(_oldUserQuestionAnswer, _newUserQuestionAnswer.GetType().GetProperty(UserQuestionAnswerPropInfo.Name).GetValue(_newUserQuestionAnswer));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public UserQuestionAnswer UpdateUserQuestionAnswer(UserQuestionAnswer _UserQuestionAnswer)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldUserQuestionAnswer = context.UserQuestionAnswer.FirstOrDefault(u => u.Id == _UserQuestionAnswer.Id);
                    if (oldUserQuestionAnswer != null)
                    {
                        UpdateObject(_UserQuestionAnswer, ref oldUserQuestionAnswer);
                        var numberOfUpdatedUserQuestionAnswer = context.SaveChanges();
                        return numberOfUpdatedUserQuestionAnswer > 0 ? _UserQuestionAnswer : null;
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

        public UserQuestionAnswer GetUserQuestionAnswerByUserId(int _userId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var userQuestionandAnswer = context.UserQuestionAnswer.FirstOrDefault(x => x.UserId == _userId);
                    return userQuestionandAnswer;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool CheckUserQuestionAnswerByUserQuAnsId(int _userId, int _questionId, string _answer)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    bool isQuestionTrue = context.UserQuestionAnswer.FirstOrDefault(x => x.UserId == _userId && x.QuestionId == _questionId && x.Answer == _answer)!=null?true:false;
                    return isQuestionTrue;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
