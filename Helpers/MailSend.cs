using System.Net;
using System.Net.Mail;

namespace tickets.Helpers
{
    public class MailSend
    {
        private readonly IConfiguration configuration;

        public MailSend(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

          //"UsuarioMail": "soporte@fba.unlp.edu.ar",
          //"PasswordMail": "Soporte2011.",
          //"ServerMail": "mail.fba.unlp.edu.ar",
          //"Port": "993",
          //"Ssl": "true",
          //"DefaultCredential": "true",

        public void SendRegistro(String receptor, String asunto, String msg)
        {
            MailMessage mail = new MailMessage();

            String Usermail = this.configuration["UsuarioMail"];
            String Password = this.configuration["PasswordMail"];
            String UserLogin = this.configuration["UsuarioLogin"];

            mail.From = new MailAddress(Usermail);
            mail.To.Add(mail.From);
            mail.Subject = asunto;
            mail.Body = msg;
            mail.IsBodyHtml= true;
            mail.Priority= MailPriority.Normal;

            String smtp = this.configuration["ServerMail"];
            int port = int.Parse(this.configuration["Port"]);
            bool ssl = bool.Parse(this.configuration["Ssl"]);
            bool defaultCredential = bool.Parse(this.configuration["DefaultCredential"]);

            SmtpClient smtpClient= new SmtpClient();

            smtpClient.Host = smtp;
            smtpClient.Port = port;
            smtpClient.EnableSsl= ssl;
            smtpClient.UseDefaultCredentials= defaultCredential;

            NetworkCredential networkCredential= new NetworkCredential(UserLogin, Password);

            smtpClient.Credentials = networkCredential;

            smtpClient.Send(mail);


        }


    }
}
