using ReportAnIssue.Models;
using Simplify.Mail;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ReportAnIssue.Global
{
    public class SendMail
    {
        [HttpPost]
        public static void InformUsers(List<string> users, Issue issue, string userName, string comment, byte type)
        {
            if (type == 1)
            {
                MailSender.Default.Send("usterka@cnbop.pl",
                users,
                "Utworzono nową usterkę: " + issue.Title,
                "Użytkownik " + userName + " utworzył nową usterkę o treści: " + issue.Description);
            }

            if (type == 2)
            {
                MailSender.Default.Send("usterka@cnbop.pl",
                users,
                "Dodano komentarz do usterki: " + issue.Title,
                "Użytkownik " + userName + " dodał nowy komentarz: " + comment);
            }

            if (type == 3)
            {
                MailSender.Default.Send("usterka@cnbop.pl",
                users,
                "Usterka wykonana: " + issue.Title,
                "Można ocenić usterkę");
            }

            if (type == 4)
            {
                MailSender.Default.Send("usterka@cnbop.pl",
                users,
                "Usterka zamknięta: " + issue.Title,
                "Użytkownik " + userName + " ocenił usterkę: " + issue.Rate + "; " + issue.RateComment);
            }

        }
    }
}