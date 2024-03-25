using BloomersIntegrationsManager.Domain.Entities;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Hangfire.Logging;
using Hangfire.States;
using System.Net;
using System.Text.Json;

namespace BloomersIntegrationsManager.Domain.Filters
{
    public class WorkflowJobFailureAttribute : IElectStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public async void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                string[] subs = failedState.Exception.Message.Split(" - ");
                string rootPath = $@"C:\temp\logs";
                string filePath = $@"C:\temp\logs\{subs[0]} - {DateTime.Today.Date.ToString("yyyy-MM-dd")}.txt";
                var listLogs = new List<IntegrationLogModel>();

                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                if (File.Exists(filePath))
                {
                    var text = File.ReadAllText(filePath);
                    listLogs = JsonSerializer.Deserialize<List<IntegrationLogModel>>(text);

                    listLogs.Add(new IntegrationLogModel
                    {
                        lastupdateon = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        origem = subs[0],
                        metodo = subs[1],
                        mensagem = subs[2],
                        exception = subs[3]
                    });

                    var listMailSending = listLogs.Where(a => a.mailsendAt is not null);

                    if (listMailSending.Count() > 0)
                    {
                        if ((DateTime.Now - Convert.ToDateTime(listLogs.Where(a => a.mailsendAt is not null).Last().mailsendAt)).Minutes > 15)
                        {
                            var sender = new SmtpSender(() => new System.Net.Mail.SmtpClient(host: "smtp.office365.com")
                            {
                                EnableSsl = true,
                                Port = 587,
                                UseDefaultCredentials = false,
                                Credentials = new NetworkCredential("robot@newbloomers.com.br", "Qon45227")
                            });

                            Email.DefaultSender = sender;

                            var email = await Email
                                .From("robot@newbloomers.com.br")
                                .To("suporte@newbloomers.com.br")
                                .Subject($"[The job failed.] {listLogs.Last().origem}")
                                .Body($"JOB: {listLogs.Last().origem}\nMethod: {listLogs.Last().metodo}\nDate: {listLogs.Last().lastupdateon}\nStatus: Failed\nMessage: {listLogs.Last().mensagem}\nException: {listLogs.Last().exception}")
                                .SendAsync();

                            listLogs.Last().mailsendAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    File.Delete(filePath);
                }
                else
                {
                    listLogs.Add(new IntegrationLogModel
                    {
                        lastupdateon = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        origem = subs[0],
                        metodo = subs[1],
                        mensagem = subs[2],
                        exception = subs[3]
                    });

                    var sender = new SmtpSender(() => new System.Net.Mail.SmtpClient(host: "smtp.office365.com")
                    {
                        EnableSsl = true,
                        Port = 587,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("robot@newbloomers.com.br", "Qon45227")
                    });

                    Email.DefaultSender = sender;

                    var email = await Email
                        .From("robot@newbloomers.com.br")
                        .To("fabiano.rokutan@newbloomers.com.br")
                        .Subject($"[The job failed.] {listLogs.Last().origem}")
                        .Body($"JOB: {listLogs.Last().origem}\nMethod: {listLogs.Last().metodo}\nDate: {listLogs.Last().lastupdateon}\nStatus: Failed\nMessage: {listLogs.Last().mensagem}\nException: {listLogs.Last().exception}")
                        .SendAsync();

                    listLogs.Last().mailsendAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                string json = JsonSerializer.Serialize(listLogs);

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(json);
                }
            }
        }
    }
}
