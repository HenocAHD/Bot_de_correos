using Quartz;
using Linkedin.Net.Db;
using Linkedin.Net.Models;
using SengridApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Linkedin.Net.Estructuras;
using EllipticCurve;

namespace MailBot.SendGrid.controllers
{
    internal class SendgridJob : IJob
    {
        private accounts cuenta = new accounts();

        public async Task Execute(IJobExecutionContext context)
        {
            
            Log.Information("Iniciando la tarea de enviar los correos");
            try
            {
                Log.Information("Obteniendo datos de la cuenta...");
                Log.Information(context.MergedJobDataMap["account_id"].ToString());
                cuenta = accounts.SelectById(database.getdatabase(), context.MergedJobDataMap["account_id"].ToString());
                Log.Information("Datos obtenidos correctamente");

                Log.Information("Obteniendo datos del cliente...");
                var cliente = clients.SelectById(database.getdatabase(), context.MergedJobDataMap["client_id"].ToString());
                Log.Information("Datos obtenidos correctamente");

                Log.Information("Creando el cliente de Sendgrid...");
                var sendgrid_client = new SendGridTools(Environment.GetEnvironmentVariable("api_key").ToString());
                Log.Information("Cliente creado correctamente");

                Log.Information("Enviando el mensaje...");
                
                var account_mail = $"{cuenta.account_name.Replace(" ", ".")}@panamify.com".ToLower();

                var templateUrl = Path.Combine(Directory.GetCurrentDirectory(), "templates", $"{cuenta.account_name}.html");
                var enviando = await sendgrid_client.SendEmail(cuenta.account_mail, account_mail, templateUrl, cliente.client_name);
                if (enviando != "accepted")
                {
                    Log.Fatal("Sengrid no acepto el envio del correo");
                    Log.Fatal(enviando);
                }
                else
                {
                    //agregar los registros a emails_sent;
                    var cliente1 = new email_sent {
                        create_at = DateTime.Now,
                        email_from = cuenta.account_id,
                        email_to = cliente.client_id,
                        date_sent = DateTime.Now,
                        status = 1
                    };
                    cliente1.Create(app.app.getMutexDatabase);
                    Log.Information("Correo enviado correctamente");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("Ocurrio un error al momento de enviar el correo");
                Log.Fatal(ex.Message);
            }
            finally
            {
                Log.Information("Tarea terminada");
            }


        }
    }
}
