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
            var proxies = proxys.SelectAll(database.getdatabase());
            var webhook = new WebhookTools(Environment.GetEnvironmentVariable("api_key").ToString());
            var dataRaw = await webhook.GetAllDataRaw();

            int count = 0;
            foreach (var data in dataRaw)
            {

            }
        }
    }
}
