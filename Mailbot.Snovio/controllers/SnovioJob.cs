using Linkedin.Net.Models;
using Linkedin.Net.Db;
using Snovio;
using dotenv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Serilog;

namespace MailBot.Snovio.controllers
{
    internal class SnovioJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var cliente = clients.SelectById(database.getdatabase(), context.MergedJobDataMap["client_id"].ToString());
            var snovio = new SnovioTools(Environment.GetEnvironmentVariable("user_id").ToString(), Environment.GetEnvironmentVariable("user_secret").ToString());
            try
            {
                var newEmail = await snovio.getEmails(cliente.client_url);
                cliente.client_email = newEmail.data.emails[0].email;
                cliente.Update(app.app.getMutexDatabase);
            }
            catch (Exception ex)
            {
                Log.Fatal("Ocurrio un error al intentar actualizar los emails de los clientes");
                Log.Fatal(ex.Message);
            }
            finally
            {
                Log.Information("Emails actuializados correctamente");
            }
        }
    }
}
