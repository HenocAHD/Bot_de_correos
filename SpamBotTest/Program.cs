﻿// See https://aka.ms/new-console-template for more information

using Snovio;
using Webhook;
using Linkedin.Net.objects;


const string userId = "e1254fd5a3dc7facd8fc51dc8b0bf154";
const string userSecret = "61628d97decab9d4c528946bffcebec0";

/*var snov = new SnovioTools(userId, userSecret);
var apikey = Environment.GetEnvironmentVariable("");
//snov.getEmailsFromUrL("https://www.linkedin.com/in/karnib/").GetAwaiter().GetResult();
var result = snov.getEmails("https://www.linkedin.com/in/davesaenz/").GetAwaiter().GetResult();
Console.WriteLine(result.data.emails[0].email);*/

/*var sendgrid = new SendGridClient("sdsad", "sdsda");*/


const string apikey = "f9a9f249-6f5c-447d-ae20-d93bfd64bf60";
var webhook = new WebhookTools();
var result = webhook.Actualizar("8").GetAwaiter().GetResult();

Console.WriteLine(result);
//Console.WriteLine(await webhook.GetIp());
