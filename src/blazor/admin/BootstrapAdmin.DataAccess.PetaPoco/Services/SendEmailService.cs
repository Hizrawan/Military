// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using System.Net.Mail;
using System.Net;
using System.Text;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class SendEmailService : ISendEmail
{
    private IDBManager DBManager { get; }

    [NotNull]
    private string? SmtpServer { get; set; }

    [NotNull]
    private string? Port { get; set; }

    [NotNull]
    private string? SenderEmail { get; set; }

    [NotNull]
    private string? SenderPassword { get; set; }

    [NotNull]
    private string? SenderName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dict"></param>
    public SendEmailService(IDBManager db, IDict dict)
    {
        DBManager = db;

        SmtpServer = dict.GetAll().FirstOrDefault(d => d.Category == "SendEmailSetting" && d.Code == "SmtpServer")?.Name;
        Port = dict.GetAll().FirstOrDefault(d => d.Category == "SendEmailSetting" && d.Code == "Port")?.Name;
        SenderEmail = dict.GetAll().FirstOrDefault(d => d.Category == "SendEmailSetting" && d.Code == "SenderEmail")?.Name;
        SenderPassword = dict.GetAll().FirstOrDefault(d => d.Category == "SendEmailSetting" && d.Code == "SenderPassword")?.Name;
        SenderName = dict.GetAll().FirstOrDefault(d => d.Category == "SendEmailSetting" && d.Code == "SenderName")?.Name;
    }

    public List<SendEmail> GetAll()
    {
        using var db = DBManager.Create();
        string sqlScript = """
                Select send.*, dict.Name As EmailStatusValue, etype.Name As EmailTypeValue
                From (
                    SELECT *
                    FROM [SendEmails]
                    Union All
                    SELECT *
                    FROM [SendEmailLogs]
                ) As send
                LEFT JOIN [Dicts] AS [dict] ON [dict].[Category] = 'EmailStatus' AND [dict].[Code] = [send].[EmailStatus]
                LEFT JOIN [Dicts] AS [etype] ON [etype].[Category] = 'EmailType' AND [etype].[Code] = [send].[EmailType]
                Order By send.CreateDate Desc
            """;
        return db.Fetch<SendEmail>(sqlScript);
    }
     
    public async Task<SendEmail?> GetSendEmailAsync()
    {
        using var db = DBManager.Create();

        //1, 不限制發送(不選用)
        //var sendEmail = await db.FirstOrDefaultAsync<SendEmail?>("Where EmailStatus = 0 Order By Priority, CreateDate");

        //2, 中華電信限定每天發送1500封Email
        var sendEmail = (await db.FetchProcAsync<SendEmail>("usp_GetSendEmail")).FirstOrDefault();

        if (sendEmail != null)
        {
            //將要發送的資料設定為發送中，以防止重復發送
            db.Execute("Update SendEmails Set EmailStatus = 1 Where Id = @0", sendEmail.Id);
        }
        return sendEmail;
    }

    public async Task<SendEmail?> GetByIdAsync(string? Id)
    {
        if (string.IsNullOrEmpty(Id)) return null;

        using var db = DBManager.Create();
        return await db.FirstOrDefaultAsync<SendEmail?>("Where Id = @0", Id);
    }

    public async Task SendEmail()
    {
        //發送Email
        var sendEmail = await GetSendEmailAsync();

        #region MailKit.Net.Smtp, 有些問題, 先保留
        //1, MailKit.Net.Smtp 
        //if (sendEmail != null)
        //{
        //    try
        //    {
        //        // sender
        //        // recipient
        //        var recipientEmail = sendEmail.Email; //csdndaolizhe@163.com

        //        var message = new MimeMessage();
        //        message.From.Add(new MailboxAddress(SenderName, SenderEmail));
        //        message.To.Add(new MailboxAddress(sendEmail.Email, recipientEmail));
        //        message.Subject = sendEmail.Title;

        //        // mail context
        //        var bodyBuilder = new BodyBuilder();
        //        bodyBuilder.TextBody = sendEmail.Content;
        //        message.Body = bodyBuilder.ToMessageBody();

        //        // set smtp client
        //        using (var client = new SmtpClient())
        //        {
        //            // smtp server
        //            client.Connect(SmtpServer, int.Parse(Port ?? "25"), false);
        //            client.Authenticate(SenderEmail, SenderPassword);
        //            // send email
        //            var ret = client.Send(message);
        //            // discount
        //            client.Disconnect(true);

        //            //
        //            RemoveSendEmail(sendEmail.Id!);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //要還原成未發送的狀態
        //        RestoreSendEmailStatus(sendEmail.Id!, ex.Message.ToString());
        //        throw;
        //    }
        //}
        #endregion

        //2, 用較早的 .Net Smtp 來發送
        if (sendEmail != null)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage(SenderEmail!, sendEmail!.Email!);

                //smtpClient
                smtpClient.Host = SmtpServer;
                smtpClient.Port = int.Parse(Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(SenderName, SenderPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpClient.EnableSsl = true;

                //message
                message.Subject = sendEmail.Title;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = sendEmail.Content;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                smtpClient.Send(message);
                //移除已經發送成功的資料
                RemoveSendEmail(sendEmail.Id!);
            }
            catch (Exception ex)
            {
                //還原成未發送的狀態
                RestoreSendEmailStatus(sendEmail.Id!, ex.Message.ToString());
            }
        }
    }

    /// <summary>
    /// 發送失敗: 要將狀態還原為待發送
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="failReason"></param>
    private void RestoreSendEmailStatus(string Id, string? failReason)
    {
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();

            //還原狀態
            db.Execute("Update SendEmails Set EmailStatus = 0, FailReason = @1, ErrorCount = ErrorCount + 1  Where Id = @0", Id, failReason);

            //如果發送失敗次數每疊加5次，就退一級發送，以免阻塞其他正常Email的發送
            db.Execute("Update SendEmails Set Priority = Priority + 1 Where ErrorCount % 5 = 0 And Id = @0", Id);

            //TODO, 總發送次數達到多少, 就不要發送?

            db.CompleteTransaction();
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
    }

    /// <summary>
    /// 發送成功: 將SendEmail資料復制一筆到SendEmailLog中, 并刪除SendEmail中的資料, 以免重復發送
    /// </summary>
    /// <param name="Id"></param>
    private void RemoveSendEmail(string Id)
    {
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("Insert Into SendEmailLogs Select * From SendEmails Where Id = @0", Id);
            db.Execute("Update SendEmailLogs Set EmailStatus = 2 , SendDate = GetDate() Where Id = @0", Id);
            db.Execute("Delete From SendEmails Where Id = @0", Id);
            db.CompleteTransaction();
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
    }
}
