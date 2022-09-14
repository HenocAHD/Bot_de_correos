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
using Webhook.Structs;
using System.Runtime.Serialization.Formatters;

namespace MailBot.Webhook.controllers
{
    internal class WebhookJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
           

            Log.Information("Iniciando tarea del Webhook");
            try
            {
                //proxies
                Log.Information("Solicitando los proxies");
                var proxies = proxys.SelectProxysAvailable(database.getdatabase());
                List<string> emailsOpened = new List<string>();

                Log.Information("Solicitando los datos al webhook");
                foreach(var proxy in proxies)
                {
                    var webhook = new WebhookTools(Environment.GetEnvironmentVariable("api_key").ToString(), new Proxy(proxy.proxy_ip, proxy.proxy_port, proxy.proxy_user, proxy.proxy_pass));
                    await webhook.GetWebhookData();

                    int count = 0;
                    var proxiesSub = proxys.SelectProxysAvailable(database.getdatabase());
                    proxiesSub.GetRandom();

                    foreach(var uuid in webhook.GetUuids)
                    {
                        var webhookSub = new WebhookTools(Environment.GetEnvironmentVariable("api_key").ToString(), new Proxy(proxiesSub[count].proxy_ip, proxiesSub[count].proxy_port, proxiesSub[count].proxy_user, proxiesSub[count].proxy_pass));
                        var result = await webhookSub.GetDataRaw(uuid);

                        var raw = JsonSerializer.Deserialize<SengridContentStruct>(result);

                        if (raw.content[0].Event == "open")
                        {
                            emailsOpened.Add(raw.content[0].email);
                        }

                        await webhookSub.DeleteDataRaw(uuid);
                        if (count == proxiesSub.Count) count = 0;
                        count++;

                        
                    }
                }
                List<string> emailsOpenedClealy = emailsOpened.DistinctBy(x => x).ToList();
                Log.Information("Datos obtenidos correctamente");

                Log.Information("Actualizando la base de datos");
                foreach(var email in emailsOpenedClealy)
                {
                    var emailsSent = email_sent.SelectByClientEmail(app.app.getMutexDatabase, email);
                    emailsSent.status = 2; //suponiendo que el 2 es opened;
                    emailsSent.Update(app.app.getMutexDatabase);
                }
                Log.Information("Base de datos Actualizada correctamente");
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
