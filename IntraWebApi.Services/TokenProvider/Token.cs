using System;
using System.Collections.Generic;
using System.Text;

namespace IntraWebApi.Services.TokenProvider
{
    public class Token
    {
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
    }
}
