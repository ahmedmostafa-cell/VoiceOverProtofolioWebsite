using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;


        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message , string id)
        {
            var emailMessage = CreateEmailMessage(message , id);

            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message message , string id)
        {
            var mailMessage = CreateEmailMessage(message , id);

            await SendAsync(mailMessage);
        }


        public async Task SendEmailAsyncToCustomer(Message message)
        {
            var mailMessage = CreateEmailMessageToCustomer(message);

            await SendAsync(mailMessage);
        }


        public async Task SendEmailAsyncToCustomerWithBookingDetails(Message message, Guid id)
        {
            var mailMessage = CreateEmailMessageToCustomerWithBookingDetails(message, id);

            await SendAsync(mailMessage);
        }

        
        public async Task SendEmailAsyncToCustomerNotConfirmedBooking(Message message, string Comment, string CustomerEmail)
        {
            var mailMessage = CreateEmailMessageToCustomerNotConfirmed(message , Comment , CustomerEmail);

            await SendAsync(mailMessage);
        }




        

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    client.CheckCertificateRevocation = false;
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    client.CheckCertificateRevocation = false;
                    
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }





        private MimeMessage CreateEmailMessage(Message message , string id)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            
            string body = CreateBody( id);
           





            var bodyBuilder = new BodyBuilder { HtmlBody = CreateBody(id) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
       
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }




        private MimeMessage CreateEmailMessageToCustomer(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            string body = CreateBodyToAdmin();






            var bodyBuilder = new BodyBuilder { HtmlBody = CreateBodyToAdmin() };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
            
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }




        private MimeMessage CreateEmailMessageToCustomerWithBookingDetails(Message message, Guid id)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            string body = CreateBodyToAdminWithBookingDetails( id);






            var bodyBuilder = new BodyBuilder { HtmlBody = CreateBodyToAdminWithBookingDetails(id) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private MimeMessage CreateEmailMessageToCustomerNotConfirmed(Message message, string Comment, string CustomerEmail)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            string body = CreateBodyToCustomerNotConfirmed( Comment,  CustomerEmail);






            var bodyBuilder = new BodyBuilder { HtmlBody = CreateBodyToCustomerNotConfirmed(Comment , CustomerEmail) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }





        

        private string CreateBody(string id) 
        {
            string Body = string.Empty;
            
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Templates", "SendEmailToAdmin.html")))
            {
                Body = reader.ReadToEnd();
            }

            Body = Body.Replace("InvoiceNUMBER", id.ToString());

           

            Body = Body.Replace("InvoiceNUMBEeeR", "1gUVZ2AchQ8VNblfxskZZA_aSy6qYnbps");
            return Body;
        }




        private string CreateBodyToAdmin()
        {
            string Body = string.Empty;

            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Templates", "SendEmailToCustomer.html")))
            {
                Body = reader.ReadToEnd();
            }

           

            return Body;
        }


        private string CreateBodyToAdminWithBookingDetails(Guid id)
        {
            string Body = string.Empty;

            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Templates", "SendEmailToCustomer.html")))
            {
                Body = reader.ReadToEnd();
            }

            Body = Body.Replace("InvoiceNUMBER", id.ToString());

            return Body;
        }




        private string CreateBodyToCustomerNotConfirmed(string Comment, string CustomerEmail)
        {
            string Body = string.Empty;

            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Templates", "SendEmailToCustomer NotConfirmed.html")))
            {
                Body = reader.ReadToEnd();
            }

            Body = Body.Replace("comment", Comment);

            return Body;
        }
    }
}
