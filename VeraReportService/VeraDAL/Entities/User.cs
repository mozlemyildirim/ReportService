using System;
using System.Collections.Generic;
using System.Text;

namespace VeraDAL.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserCode { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Telephone { get; set; }

        public string Mail { get; set; }

        public int UserTypeId { get; set; }

        public int Status { get; set; }

        public string Password { get; set; }

        public int? GeographicalAuthorityId { get; set; }

        public int? HomepageRefreshTime { get; set; }

    }

}
