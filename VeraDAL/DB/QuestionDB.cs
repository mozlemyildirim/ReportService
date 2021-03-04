using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class QuestionDB
    {
        private static QuestionDB instance;
        public static QuestionDB GetInstance()
        {
            if (instance == null)
            {
                instance = new QuestionDB();
            }
            return instance;
        }
        public Question AddNewQuestion(Question _Question)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Question.Add(_Question);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _Question : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Question> GetAllQuestiones()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var QuestionList = context.Question.ToList();
                    return QuestionList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void UpdateObject(Question _newQuestion, ref Question _oldQuestion)
        {
            try
            {

                foreach (PropertyInfo QuestionPropInfo in _newQuestion.GetType().GetProperties().ToList())
                {
                    _oldQuestion.GetType().GetProperty(QuestionPropInfo.Name).SetValue(_oldQuestion, _newQuestion.GetType().GetProperty(QuestionPropInfo.Name).GetValue(_newQuestion));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Question UpdateQuestion(Question _Question)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldQuestion = context.Question.FirstOrDefault(u => u.Id == _Question.Id);
                    if (oldQuestion != null)
                    {
                        UpdateObject(_Question, ref oldQuestion);
                        var numberOfUpdatedQuestion = context.SaveChanges();
                        return numberOfUpdatedQuestion > 0 ? _Question : null;
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
