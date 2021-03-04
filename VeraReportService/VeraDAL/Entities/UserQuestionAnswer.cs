using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class UserQuestionAnswer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int UserId { get; set; }

        public string Answer { get; set; }

    }

}
