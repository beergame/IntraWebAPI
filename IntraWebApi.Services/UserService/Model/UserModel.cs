using System;
using System.Collections.Generic;
using System.Text;

namespace IntraWebApi.Services.UserService.Model
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Civility { get; set; }
        public string Username { get; set; }
        public string PassWord { get; set; }
    }
}
