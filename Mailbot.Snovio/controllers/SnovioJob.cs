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
            Log.Information("Obteniendo la url del cliente");
            var cliente = clients.SelectById(app.app.getMutexDatabase, context.MergedJobDataMap["client_id"].ToString());
            Log.Information("Creando el cliente de snovio");
            var snovio = new SnovioTools(Environment.GetEnvironmentVariable("user_id").ToString(), Environment.GetEnvironmentVariable("user_secret").ToString());
            try
            {
                if(cliente.client_email == String.Empty)
                {
                    Log.Information("Intentando conseguir el email del cliente");
                    var newEmail = await snovio.getProspectFromUrL(cliente.client_url);

                    if (newEmail.data.emails.Count > 0)
                    {
                        cliente.client_email = newEmail.data.emails[0].email;
                        cliente.Update(app.app.getMutexDatabase);
                        Log.Information("Emails actuializados correctamente");
                    }
                    else
                    {
                        Log.Information("Snovio no pudo encontrar un email para el cliente");
                    }
                }
                else
                {
                    Log.Information("El cliente ya tiene un email asignado");
                }
                
            }
            catch (Exception ex)
            {
                Log.Fatal("Ocurrio un error al intentar actualizar los emails de los clientes");
                Log.Fatal(ex.Message);
            }
            finally
            {
                
            }
        }
    }
}
