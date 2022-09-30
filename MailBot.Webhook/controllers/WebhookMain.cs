using Quartz;
using Webhook;
using Linkedin.Net.Models;
using Linkedin.Net.Db;
using MailBot.Webhook.app;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBot.Webhook.controllers
{
    internal class WebhookMain : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

            var webhook = new WebhookTools();
            var datos = await webhook.GetWebhookData();
            
            foreach (var data in datos)
            {
                IJobDetail jobDetail = JobBuilder.Create<WebhookJob>()
                .WithIdentity($"job_{data.email}_{Guid.NewGuid()}", $"group_{data.email}_{Guid.NewGuid()}")
                .SetJobData(new JobDataMap
                {
                    {"id_webhook_data",data.id_webhook_data}
                })
                .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"job_{data.email}_{Guid.NewGuid()}", $"group_{data.email}_{Guid.NewGuid()}")
                    //schedule
                    .StartNow()
                    .Build();

                app.app.getScheduler.ScheduleJob(jobDetail, trigger);
                app.app.getScheduler.Start();
            }
        }
    }
}
