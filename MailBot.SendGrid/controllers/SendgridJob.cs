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

        public async Task Execute(IJobExecutionContext context)
        {
            string account_mail;
            Log.Information("Iniciando la tarea de enviar los correos");
            try
            {
                

                Log.Information("Obteniendo datos de la cuenta...");
                Log.Information(context.MergedJobDataMap["account_id"].ToString());
                var cuenta = accounts.SelectById(database.getdatabase(), context.MergedJobDataMap["account_id"].ToString());
                Log.Information("Datos obtenidos correctamente");

                Log.Information("Obteniendo datos del cliente...");
                var cliente = clients.SelectById(database.getdatabase(), context.MergedJobDataMap["client_id"].ToString());
                Log.Information("Datos obtenidos correctamente");

                if(email_sent.SelectByClientEmail(app.app.getMutexDatabase, cliente.client_email) == null)
                {
                    Log.Information("Creando el cliente de Sendgrid...");
                    Console.WriteLine(Environment.GetEnvironmentVariable("api_key").ToString());
                    var sendgrid_client = new SendGridTools("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");
                    Log.Information("Cliente creado correctamente");

                    Log.Information("Enviando el mensaje...");

                    if(cuenta.account_virtual_email == null)
                    {
                        cuenta.account_virtual_email = $"{cuenta.account_name.Replace(" ", ".")}@panamify.com".ToLower();
                        cuenta.Update(app.app.getMutexDatabase);
                    }
    
                    var templateUrl = Path.Combine(Directory.GetCurrentDirectory(), "templates", "Plantilla.html");
                    Console.WriteLine(templateUrl);
                    Console.WriteLine(cliente.client_email);
                    var enviando = await sendgrid_client.SendEmail(cuenta.account_virtual_email, cliente.client_email, templateUrl, cuenta.account_name);
                    if (enviando.ToString() != "Accepted")
                    {
                        Log.Fatal("Sengrid no acepto el envio del correo");
                        Log.Fatal(enviando);
                    }
                    else
                    {
                        //agregar los registros a emails_sent;
                        var cliente1 = new email_sent
                        {
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
                else
                {
                    Log.Information("Ya se envio un correo anteriormente");
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
