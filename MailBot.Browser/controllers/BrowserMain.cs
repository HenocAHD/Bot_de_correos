using Linkedin.Net.Models;
using Linkedin.Net.Db;
using MySqlX.XDevAPI.Common;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;

namespace MailBot.Browser.controllers
{
    internal class BrowserMain : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var cliente = clients.SelectClientLinkNoUpdate(database.getdatabase());

            foreach (var client in cliente)
            {
                IJobDetail jobDetail = JobBuilder.Create<BrowserJob>()
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

                app.app.getSchenduler.ScheduleJob(jobDetail, trigger);
                app.app.getSchenduler.Start();
            }


        }
    }
}
