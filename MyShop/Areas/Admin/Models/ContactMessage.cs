using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ContactMessage
    {
        public ContactMessage()
        {
            MessageDate = System.DateTime.Now;
        }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsChecked { get; set; }

        public  virtual bool SendEmail(string receiverEmail, string password, 
            string smtpHost , int smtpPort)
        {
            try
            {
                MailAddress senderAddress =
                  new MailAddress(receiverEmail);
                MailAddress recieverAddress =
                    new MailAddress(receiverEmail);
                MailMessage oMessage = new MailMessage();
                oMessage.Sender = senderAddress;
                oMessage.From = senderAddress;
                oMessage.ReplyToList.Add(senderAddress);
                oMessage.Subject = "دریافت یک پیام جدید";
                oMessage.Priority = MailPriority.High;
                oMessage.SubjectEncoding = Encoding.UTF8;
                oMessage.To.Add(recieverAddress);
                var persianDate = new PersianCalendar();
                var messageDate = string.Format("{0}/{1}/{2}", persianDate.GetYear(MessageDate),
                                                        persianDate.GetMonth(MessageDate),
                                                        persianDate.GetDayOfMonth(MessageDate));
                oMessage.Body = "<div style=\"text-align:center;\">" +
                                                "<div style=\"display:inline-block;max-width:100%;width:600px;padding:0;border:1px solid black\">" +
                                                "<div style=\"background-color:#7f9297;color:white;padding:10px;text-align:center;font-family:Arial\">" +
                                                "Shopikar.com" +
                                                "</div>" +
                                                "<div style=\"background-color:white;color:black;padding:10px 20px;text-align:center\">" +
                                                "<div style=\"font-family:Tahoma;\">" +
                                                "یک پیام دریافت شد" +
                                                "</div><hr>" +
                                                "<p style=\"margin:30px 10px;font-family:Tahoma;text-align:right\" dir=\"rtl\">" +
                                                "<b>نام فرستنده:</b>&nbsp;" +
                                                "<span>" + FullName + "</span>" +
                                                "<br><br>" + "<b>تاریخ ارسال:</b>&nbsp;" + "<span>" + messageDate + "</span>" + "<br><br>" +
                                                "<b>شماره همراه:</b>&nbsp;" + "<span>" + Mobile + "</span><br><br><b>ایمیل:</b>&nbsp;" +
                                                "<span>" + Email + "</span>" +
                                                "</p style=\"margin:30px 10px;font-family:Tahoma;text-align:right\" dir=\"rtl\">" + "<p>" + Description + "</p>" +
                                                "</div></div>";
                oMessage.IsBodyHtml = true;
                oMessage.BodyEncoding = Encoding.UTF8;
                oMessage.DeliveryNotificationOptions =
                    DeliveryNotificationOptions.OnFailure;
                SmtpClient oSmtpClient = new SmtpClient(smtpHost,smtpPort);
                oSmtpClient.Credentials = new NetworkCredential(receiverEmail,
                    password);
                oSmtpClient.EnableSsl = true;
                oSmtpClient.Timeout = 100000;
                oSmtpClient.Send(oMessage);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
           
        }
    }
}
