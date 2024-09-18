// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using System.Net.Mail;
using System.Net;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

/// <summary>
/// 
/// </summary>
public class EmailService : IEmail
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var fromEmail = new MailAddress("your-email@example.com", "Your Name");
        var toEmail = new MailAddress(to);
        var msg = new MailMessage(fromEmail, toEmail)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true // HTML
        };

        using var smtp = new SmtpClient
        {
            Host = "smtp.example.com", // Smtp Server
            Port = 587, // Smtp port
            EnableSsl = true, // Enable SSL
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential("your-email@example.com", "your-password") // 
        };
        try
        {
            await smtp.SendMailAsync(msg);

            //TODO Activity
        }
        catch (Exception ex)
        {
            var message = ex.Message;

            //TODO Log
        }



    }
}
