using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodPantry.Models;
using System.Net.Mail;

namespace FoodPantry.Tests.Controllers
{
    [TestClass]
    public class FoodPantryLogsControllerUnitTests
    {
        // [TestMethod]
        public void TestSendEmail()
        {
            EmailSetting emailSetting = new EmailSetting();
            emailSetting.SubjectContents = "Did you get this test?";
            emailSetting.EmailContents = "Got some stuff for you";
            string fromEmailAddress = "adamsaladino@delta.edu";
            string studentEmailAddress = "adam.saladino@ellucian.com";
            string emailServer = "exchmail.delta.edu";
            SendEmail(emailSetting, fromEmailAddress, studentEmailAddress, emailServer);
        }

        private void SendEmail(EmailSetting emailSetting, string fromEmailAddress, string studentEmailAddress, string emailServer)
        {
            try
            {
                MailMessage mail = new MailMessage(fromEmailAddress, studentEmailAddress);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = emailServer;

                mail.Subject = emailSetting.SubjectContents;
                mail.Body = emailSetting.EmailContents;
                client.Send(mail);
            }
            catch (Exception) { }
        }
    }
}
