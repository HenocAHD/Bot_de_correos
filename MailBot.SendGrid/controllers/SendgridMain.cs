using Quartz;
using Linkedin.Net.Db;
using Linkedin.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBot.SendGrid.controllers
{
    internal class SendgridMain : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var mails = client_connections.SelectAll(database.getdatabase());
            
            foreach(var mail in mails)
            {
                IJobDetail jobDetail = JobBuilder.Create<SendgridJob>()
                .WithIdentity($"job_{mail.client_id}_{Guid.NewGuid()}", $"group_{mail.client_id}_{Guid.NewGuid()}")
                .SetJobData(new JobDataMap
                {
                    {"client_id",mail.client_id},
                    {"account_id", mail.account_id }
                })
                .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger_{mail.client_id}_{Guid.NewGuid()}", $"group_{mail.client_id}_{Guid.NewGuid()}")
                    //schedule
                    .StartNow()
                    .Build();

                app.app.getSchenduler.ScheduleJob(jobDetail, trigger);
                app.app.getSchenduler.Start();
            }
        }
    }
}
