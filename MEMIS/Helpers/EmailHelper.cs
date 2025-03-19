using System.Net;
using System.Net.Mail;

namespace MEMIS.Helpers
{
  public static class EmailHelper
  {
    private static async Task SendMail(IConfiguration configuration, string to, string subject, string body, List<string> cc = null)
    {
      using (MailMessage message = new MailMessage(configuration["SmtpCredentials:Email"], to))
      {
        message.Subject = subject;
        message.IsBodyHtml = true;
        message.Body = body;

        if (cc != null && cc.Any())
        {
          foreach (string ccAddress in cc)
          {
            message.CC.Add(ccAddress);
          }
        }

        string? host = configuration["SmtpCredentials:Host"];
        int smtpPort = int.Parse(configuration["SmtpCredentials:Port"]);
        bool enableSsl = bool.Parse(configuration["SmtpCredentials:EnableSsl"]);
        string? username = configuration["SmtpCredentials:Email"];
        string? password = configuration["SmtpCredentials:Password"];

        if (!enableSsl)
        {
          ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        using (SmtpClient smtpClient = new SmtpClient(host, smtpPort))
        {
          smtpClient.UseDefaultCredentials = false;
          smtpClient.EnableSsl = enableSsl;
          smtpClient.Credentials = new NetworkCredential(username, password);
          smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

          try
          {
            await smtpClient.SendMailAsync(message);
            Console.WriteLine("Email sent successfully.");
          }
          catch (SmtpException ex)
          {
            Console.WriteLine($"SMTP Error: {ex.Message}");
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Error: {ex.Message}");
          }
        }
      }
    }

    public static async Task SendPasswordResetEmail(IConfiguration configuration, string toEmail, string resetLink)
    {
      string subject = "Your Password Has Been Reset";
      string body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; color: #333; }}
                        .container {{ max-width: 600px; margin: 20px auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px; }}
                        .button {{ background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; }}
                        .footer {{ margin-top: 20px; font-size: 12px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Password Reset Successful</h2>
                        <p>Dear User,</p>
                        <p>Your password has been successfully reset. For security reasons, we do not include your new password in this email.</p>
                        <p>To set a new password, please click the button below:</p>
                        <p><a class='button' href='{resetLink}'>Reset Your Password</a></p>
                        <p>If you did not request this change, please contact support immediately.</p>
                        <div class='footer'>This is an automated message. Please do not reply.</div>
                    </div>
                </body>
                </html>";

      await SendMail(configuration, toEmail, subject, body);
    }
  }
}
