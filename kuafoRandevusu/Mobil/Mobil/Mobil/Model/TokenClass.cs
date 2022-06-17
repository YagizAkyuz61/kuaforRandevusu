using System;
using System.Collections.Generic;
using System.Text;

namespace Mobil.Model
{
    public class TokenClass
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int user_Id { get; set; }
        public string user_Name { get; set; }
        public string user_NickName { get; set; }
        public string user_Password { get; set; }
        public string user_PhoneNumber { get; set; }
        public string user_Address { get; set; }
        public string user_Exp { get; set; }
        public int expires_in { get; set; }
        public int creation_Time { get; set; }
        public int expiration_Time { get; set; }
    }
}
