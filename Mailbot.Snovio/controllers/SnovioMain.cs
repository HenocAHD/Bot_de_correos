using Quartz;
using Linkedin.Net.Db;
using Linkedin.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBot.Snovio.controllers
{
    internal class SnovioMain : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var cliente = clients.SelectClientLinkUpdate(app.app.getMutexDatabase);

            foreach(var client in cliente)
            {
                IJobDetail jobDetail = JobBuilder.Create<SnovioJob>()
                .WithIdentity($"job_{client.client_name}_{Guid.NewGuid()}", $"group_{client.client_name}_{Guid.NewGuid()}")
                .SetJobData(new JobDataMap
                {
                    {"client_id",client.client_id}
                })
                .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger_{client.client_name}_{Guid.NewGuid()}", $"group_{client.client_name}_{Guid.NewGuid()}")
                    //schedule
                    .StartNow()
                    .Build();

                app.app.getScheduler.ScheduleJob(jobDetail, trigger);
                app.app.getScheduler.Start();
            }
        }
    }
}
