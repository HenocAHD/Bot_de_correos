using System;
using Quartz;
using Linkedin.Net.Models;
using Linkedin.Net.Db;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBot.Seguimiento.controllers
{
    internal class SeguimientoMain : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var mails = email_sent.SelectAll(app.app.getMutexDatabase);
            foreach(var mail in mails)
            {
                IJobDetail jobDetail = JobBuilder.Create<SeguimientoJob>()
                .WithIdentity($"job_{mail.id_email_sent}_{Guid.NewGuid()}", $"group_{mail.id_email_sent}_{Guid.NewGuid()}")
                .SetJobData(new JobDataMap
                {
                    {"id_email_sent",mail.id_email_sent}
                })
                .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger_{mail.id_email_sent}_{Guid.NewGuid()}", $"group_{mail.id_email_sent}_{Guid.NewGuid()}")
                    //schedule
                    .StartNow()
                    .Build();

                app.app.getSchenduler.ScheduleJob(jobDetail, trigger);
                app.app.getSchenduler.Start();
            }
        }
    }
}
