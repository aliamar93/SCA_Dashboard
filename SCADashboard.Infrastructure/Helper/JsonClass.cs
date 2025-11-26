using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Infrastructure.Helper
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class Encryption
    {
        public string Key { get; set; }
        public string IV { get; set; }
    }

    public class Jwt
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpirationMinutes { get; set; }
    }

    public class Root
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public Encryption Encryption { get; set; }
        public Jwt Jwt { get; set; }
        public SMTP SMTP { get; set; }
        public Twilio Twilio { get; set; }
    }

    public class SMTP
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class Twilio
    {
        public string accountSid { get; set; }
        public string authToken { get; set; }
        public string twilioNumber { get; set; }
    }


}
