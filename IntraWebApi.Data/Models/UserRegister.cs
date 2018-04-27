using System;
using System.Collections.Generic;
using System.Text;

namespace IntraWebApi.Data.Models
{
    public class UserRegister
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Civility { get; set; }
        public string Username { get; set; }
        public string PassWord { get; set; }
    }
}
