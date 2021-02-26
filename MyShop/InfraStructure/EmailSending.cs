using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;

namespace Models
{
    /// <summary>
    /// This class is for sending automatic emails to users when thay want to register new email, resetpassword, ...
    /// </summary>
    public class EmailSending
    {
        /// <summary>
        /// The constructor of EmailSending Class which get some parameters and set properties with this parameters.
        /// </summary>
        /// <param name="emailReciever">User email</param>
        /// <param name="emailBody">Email body</param>
        /// <param name="subject">the Subject of email.</param>
        public EmailSending(string emailReciever,
            string emailBody, string subject,
            string brandName = "ShopiKar", string phoneSupport = "09124690677")
        {
            EmailSender = "shopikarweb@gmail.com";
            EmailReciever = emailReciever;
            EmailBody = emailBody;
            Subject = subject;
            BrandName = brandName;
            PhoneSupport = phoneSupport;
           
        }
        private string EmailSender { get; set; }
        private string EmailReciever { get; set; }
        private string EmailBody { get; set; }
        private string Subject { get; set; }
        private string BrandName { get; set; }
        private string PhoneSupport { get; set; }

        /// <summary>
        /// This taks is created for Sending email using constructor parameters.
        /// </summary>
        /// <returns>send email.</returns>
        private async Task SendEmail()
        {
            try
            {
                MailMessage myMessage = new MailMessage();
                MailAddress senderAddress =
                new MailAddress(EmailSender);
                myMessage.From = senderAddress;
                myMessage.Sender = senderAddress;
                myMessage.ReplyToList.Add(senderAddress);
                myMessage.To.Add(EmailReciever);
                //System.Net.Mail.Attachment oAttachment = new System.Net.Mail.Attachment();
                //objMM.Attachments.Add()


                //objMM.CC.Add();
                //objMM.Bcc.Add();
                string mailBody = "<div style=\"text-align:center;\">" +
                               "<div style=\"display:inline-block;max-width:100%;width:600px;padding:0;border:1px solid black\">" +
                               "<div style=\"background-color:#7f9297;color:white;padding:10px;text-align:center;font-family:Arial\">" +
                               BrandName +
                               "</div>" +
                               "<div style=\"background-color:white;color:black;padding:10px 20px;text-align:center\">" +
                               "<div style=\"font-family:Tahoma;\" direction=\"rtl\">" + EmailBody +
                               "<div style=\"padding:10px;padding-bottom:40px;text-align:center;font-family:Arial\" dir =\"ltr\">" +
                               "<hr>" +
                               "<span> Support:</span>" +
                               "<span> " + PhoneSupport + " </span>" +
                               "</div></div></div></div>";
                myMessage.BodyEncoding = System.Text.ASCIIEncoding.UTF8;

                myMessage.Body = mailBody;


                myMessage.IsBodyHtml = true;
                myMessage.Subject = Subject;
                myMessage.Priority = MailPriority.High;
                myMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
                smtp.Credentials = new System.Net.NetworkCredential("shopikarweb@gmail.com",
                    "6507654a");
                smtp.EnableSsl = false;
                smtp.Timeout = 10000;
                smtp.SendAsync(myMessage,"Message Send");
                var result = await Task.FromResult(0);
                return;
            }
            catch (Exception)
            {

              
            }

        }

    }


}
