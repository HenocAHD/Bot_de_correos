using Quartz;
using Webhook;
using Linkedin.Net.Models;
using Linkedin.Net.Models.Extenciones;
using Linkedin.Net;
using Linkedin.Net.Db;
using Linkedin.Net;
using static MoreLinq.Extensions.LagExtension;
using static MoreLinq.Extensions.LeadExtension;
using dotenv;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using System.ComponentModel;
using MySqlX.XDevAPI.Common;
using System.Runtime.Serialization.Formatters;

namespace MailBot.Webhook.controllers
{
    internal class WebhookJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
            var webhook = new WebhookTools();
            Log.Information("Iniciando tarea del Webhook");
            try
            {
                Log.Information("Obteniendo los datos de la base de datos");
                var data = await webhook.getWebhookDataById(context.MergedJobDataMap["id_webhook_data"].ToString());

                Log.Information("Actualizando la base de datos");
                var mail = email_sent.SelectByClientEmail(app.app.getMutexDatabase, data.email);
                mail.status = Convert.ToInt32(status.open);
                mail.Update(app.app.getMutexDatabase);

                Log.Information("Actualizando el webhook");
                var update = await webhook.Actualizar(data.id_webhook_data.ToString());
                Log.Information(update);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error Obteniendo los datos del webhook");
                Log.Fatal(ex.Message);
            }
            finally { Log.Information("Tarea finalizada exito"); }


        }
    }
}
