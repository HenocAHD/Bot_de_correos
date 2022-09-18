﻿using Quartz;
using Linkedin.Net.Models;
using System;
using SengridApi;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linkedin.Net.Db;
using Serilog;

namespace MailBot.Seguimiento.controllers
{
    internal class SeguimientoJob:IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var mail = email_sent.SelectById(app.app.getMutexDatabase, context.MergedJobDataMap["id_mail_sent"].ToString());

            if ((DateTime.Now - mail.date_sent).TotalDays >= 4)
            {
                Log.Information("Obteniendo datos de la cuenta...");
                var cuenta = accounts.SelectById(database.getdatabase(), mail.email_from.ToString());
                Log.Information("Datos obtenidos correctamente");

                Log.Information("Obteniendo datos del cliente...");
                var cliente = clients.SelectById(database.getdatabase(), mail.email_to.ToString());
                Log.Information("Datos obtenidos correctamente");

                Log.Information("Creando el cliente de Sendgrid...");
                var sendgrid_client = new SendGridTools(Environment.GetEnvironmentVariable("api_key").ToString());
                Log.Information("Cliente creado correctamente");

                Log.Information("Enviando el mensaje...");
                var account_mail = $"{cuenta.account_name.Replace(" ", ".")}@panamify.com";

                var templateUrl = Path.Combine(Directory.GetCurrentDirectory(), "templates", cuenta.account_name);
                var enviando = await sendgrid_client.SendEmail(cuenta.account_mail, account_mail, templateUrl, cliente.client_name);
                if (enviando != "accepted")
                {
                    Log.Fatal("Sengrid no acepto el envio del correo");
                }
                else
                {
                    Log.Information("Correo de seguimiento eniado correctamente");
                    //agregar seguimiento
                }
            }
        }
    }
}