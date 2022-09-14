using MailBot.Webhook.app;
using MailBot.Webhook.controllers;
using Quartz;

app.Run();
app.MainJob();
Console.ReadLine();