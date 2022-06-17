using System;
using System.Collections.Generic;
using System.Text;

namespace Mobil.Model
{
    public class TokensClass
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int customer_Id { get; set; }
        public string customer_Name { get; set; }
        public string customer_NickName { get; set; }
        public string customer_Password { get; set; }
        public string customer_PhoneNumber { get; set; }
        public string customer_Email { get; set; }
        public int expires_in { get; set; }
        public int creation_Time { get; set; }
        public int expiration_Time { get; set; }
    }
}
