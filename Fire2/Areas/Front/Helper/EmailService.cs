using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fire2.Areas.Front.Models;
using MailKit.Net.Smtp;
using MimeKit;
namespace Fire2.Areas.Front.Helper
{
    public class EmailService
    {
        public static void SendContactMessage(ContactViewModel model)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("網站聯絡通知", "")); //原設定email
            message.To.Add(new MailboxAddress(model.Name, model.EmailAddress));
            message.Cc.Add(new MailboxAddress("收信人名稱", model.EmailAddress));
            message.Subject = "聯絡表單提交通知";
            var body = $@"
                姓名：{HttpUtility.HtmlEncode(model.Name)}<br/>
                性別：{HttpUtility.HtmlEncode(model.Gender.ToString())}<br/>
                連絡電話：{HttpUtility.HtmlEncode(model.Phone)}<br/>
                Email:{HttpUtility.HtmlEncode(model.EmailAddress)}<br/>
                詢問內容：<br/>
                {HttpUtility.HtmlEncode(model.ContactContent).Replace("\n","<br/>")}

                ";

            message.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                //設定連線 gmail ("smtp Server", Port, SSL加密) 
                client.Connect("smtp.gmail.com", 587, false); // localhost 測試使用加密需先關閉 
                client.Authenticate("", "");//原設定email + 應用程式密碼(去google申請)
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}