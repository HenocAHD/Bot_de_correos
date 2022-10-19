using dotenv.net;
using Serilog;
using Quartz;
using MailBot.Webhook.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Linkedin.Net.Models.Extenciones;

namespace MailBot.Webhook.app
{
    internal static class app
    {
        //variables
        private static Mutex mutexDatabase;
        private static IScheduler scheduler;


        public static Mutex getMutexDatabase
        {
            get
            {
                return mutexDatabase;
            }
        }

        public static IScheduler getScheduler
        {
            get
            {
                return scheduler;
            }
        }

        //metodos
        public static async void Run()
        {
            #region serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("logs/mailbot.webhook.information.log",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                rollingInterval: RollingInterval.Day)
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
                .WriteTo.File("logs/mailbot.webhook.error.log",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                rollingInterval: RollingInterval.Day)
                .CreateLogger();
            #endregion

            #region env
            Log.Information("Configurando variables de entorno");
            DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { Path.Combine(Directory.GetCurrentDirectory(), ".env") }));
            Log.Information("Variables cargadas");
            #endregion

            #region quartz
            Log.Information("Configurando Quartz");
            scheduler = await SchedulerBuilder.Create()
                .UseDefaultThreadPool(x => x.MaxConcurrency = 1)
                .UseInMemoryStore()
                .WithName("MainScheduler")
                .BuildScheduler();
            Log.Information("Quartz configurado correctamente");
            #endregion

            #region mutex_database
            Log.Information("Configurando mutex");
            mutexDatabase = new Mutex();
            Log.Information("Mutex configurado correctamente");
            #endregion
        }

        public async static void MainJob()
        {
            IJobDetail jobDetail = JobBuilder.Create<WebhookMain>()
                .WithIdentity("job", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Environment.GetEnvironmentVariable("hora_ejecucion").ToInt(), Environment.GetEnvironmentVariable("minutos_ejecucion").ToInt()))
                .StartNow()
                .Build();

            await app.scheduler.ScheduleJob(jobDetail, trigger);
            await app.scheduler.Start();
        }

    }
}
