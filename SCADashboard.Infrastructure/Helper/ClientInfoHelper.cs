using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SCADashboard.Infrastructure.Helper
{
    public static class ClientInfoHelper //: IClientInfoHelper
    {
        public static string GetClientIp(HttpContext context)
        {
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(forwardedFor))
            {
                return forwardedFor.Split(',')[0];
            }

            return context.Connection.RemoteIpAddress?.ToString();
        }

        //public static (string Browser, string OS, string Device) ParseUserAgent(string userAgent)
        //{
        //    var uaParser = Parser.GetDefault();
        //    var client = uaParser.Parse(userAgent);

        //    string browser = $"{client.UA.Family} {client.UA.Major}";
        //    string os = $"{client.OS.Family} {client.OS.Major}";
        //    string device = client.Device.Family;

        //    return (browser, os, device);
        //}

        public static string GetBrowserName(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            if (userAgent.Contains("Edg")) return "Microsoft Edge";
            if (userAgent.Contains("Chrome")) return "Google Chrome";
            if (userAgent.Contains("Firefox")) return "Mozilla Firefox";
            if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome")) return "Safari";
            if (userAgent.Contains("OPR") || userAgent.Contains("Opera")) return "Opera";
            if (userAgent.Contains("Trident") || userAgent.Contains("MSIE")) return "Internet Explorer";

            return "Unknown";
        }

        public static Root ReadJsonFile(string Path)
        {
            string filePath = System.IO.Path.Combine(AppContext.BaseDirectory, "AppData", "App.json");

            string json = File.ReadAllText(filePath);
            var jsonString = System.IO.File.ReadAllText(filePath);
            var jsonObject = JsonConvert.DeserializeObject<Root>(jsonString);

            return jsonObject;
        }

        public static string ReadTemplate(IWebHostEnvironment env, string fileName)
        {
            var filePath = Path.Combine(env.WebRootPath, "templete", fileName);
            return System.IO.File.ReadAllText(filePath);
        }
        public static async Task SendSecurityEmailAsync(string email, string userName, string action, string actionUrl, string Templete)
        {
            try
            {
                string Email = string.Empty;
                string Password = string.Empty;
                string SmtpServer = string.Empty;
                int Port = 0;


                var emailconfigure = ReadJsonFile(null);

                if (emailconfigure != null)
                {
                    Email = emailconfigure.SMTP.Email ?? throw new Exception("Email address not found in configuration.");
                    Password = emailconfigure.SMTP.Password ?? throw new Exception("Email password not found in configuration.");
                    SmtpServer = emailconfigure.SMTP.SmtpServer ?? throw new Exception("SMTP server not found in configuration.");
                    Port = emailconfigure.SMTP.Port > 0 ? emailconfigure.SMTP.Port : throw new Exception("SMTP port not found in configuration.");
                }
                else
                {
                    throw new Exception("Email configuration not found.");
                }

                var from = new MailAddress(Email, "SCA Logistik & Fullfilment");
                // Use your SMTP server credentials here
                var to = new MailAddress(email);
                var password = Password; // Use a secure way to store and retrieve this password

                using var smtp = new SmtpClient
                {
                    Host = SmtpServer,
                    Port = Port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(from.Address, password)
                };

                var message = new MailMessage(from, to)
                {
                    Subject = "Security Alert: Account Password Reset",
                    IsBodyHtml = true
                };

                // Load template and replace values
                var template = System.IO.File.ReadAllText(Templete)
                .Replace("{{UserName}}", userName)
                .Replace("{{ResetPasswordLink}}",actionUrl)
                .Replace("{{Date}}", DateTime.Now.Year.ToString());
                //.Replace("{{ActionUrl}}", actionUrl);

                // Attach logo
                var htmlView = AlternateView.CreateAlternateViewFromString(template, null, "text/html");
                var logo = new LinkedResource("wwwroot/images/logo.png", MediaTypeNames.Image.Svg)
                {
                    ContentId = "logo"
                };
                
                htmlView.LinkedResources.Add(logo);
                message.AlternateViews.Add(htmlView);

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new Exception("Failed to send security email.", ex);
            }
        }

        public enum TemplateType
        {

            SignUp,
            PasswordReset,
            EmailVerification
        }

        public static string GetTemplate(TemplateType templateType)
        {
            string templatePath = templateType switch
            {
                TemplateType.PasswordReset => "PasswordReset.html",
                TemplateType.EmailVerification => "EmailVerification.html",
                _ => throw new ArgumentOutOfRangeException(nameof(templateType), templateType, null)
            };

            return templatePath;

        }

        public static string Encryption(string plainText)
        {
            var AESconfigure = ReadJsonFile(null);
            if (AESconfigure == null)
            {
                throw new Exception("AES configuration not found.");
            }

            string key = AESconfigure.Encryption.Key ?? throw new Exception("Encryption key not found in configuration.");
            string iv = AESconfigure.Encryption.IV ?? throw new Exception("Encryption IV not found in configuration.");
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public static string Decryption(string cipherText)
        {
            var AESconfigure = ReadJsonFile(null);
            if (AESconfigure == null)
            {
                throw new Exception("AES configuration not found.");
            }

            string key = AESconfigure.Encryption.Key ?? throw new Exception("Encryption key not found in configuration.");
            string iv = AESconfigure.Encryption.IV ?? throw new Exception("Encryption IV not found in configuration.");
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    public class WhatsAppService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber; // Twilio sandbox number

        public WhatsAppService(string accountSid, string authToken, string fromNumber)
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _fromNumber = fromNumber;

            TwilioClient.Init(_accountSid, _authToken);
        }

        /// <summary>
        /// Send a WhatsApp message
        /// </summary>
        /// <param name="toNumber">Recipient number in format +1234567890</param>
        /// <param name="messageBody">Message text</param>
        public void SendMessage(string toNumber, string messageBody)
        {
            try
            {
                var message = MessageResource.Create(
                    from: new PhoneNumber("whatsapp:" + _fromNumber),
                    to: new PhoneNumber("whatsapp:" + toNumber),
                    body: messageBody
                );

                //Console.WriteLine($"Message sent! SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error sending WhatsApp message: " + ex.Message);
            }
        }
    }


    //Twilio WhatsApp Example Usage
    #region
    //    // Twilio credentials
    //string accountSid = "YOUR_TWILIO_ACCOUNT_SID";
    //string authToken = "YOUR_TWILIO_AUTH_TOKEN";
    //string twilioNumber = "+14155238886"; // Twilio Sandbox number

    //var whatsAppService = new WhatsAppService(accountSid, authToken, twilioNumber);

    //// Send a message
    //whatsAppService.SendMessage("+1234567890", "Hello! This is a test message from C#.");

    #endregion


}
