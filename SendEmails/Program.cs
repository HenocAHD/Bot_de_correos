using System;
using SendGrid;
using SendGrid.Helpers.Mail;

var data = new {};
var from = new EmailAddress("karla.florence@panamify.com", "Panamify");
var to = new EmailAddress("henoc.hernandez@uph.edu.hn", "Henoc Hernandez");
var client = new SendGridClient("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");
var nombre = "Henoc Hernandez";
var subject = $"Hi {nombre}";
var plainTextContent = "";
var htmlContent = File.ReadAllText("C:/Users/Henoc Hernandez/Documents/BotSpam/SendEmails/templates/heno/index.html");
htmlContent = htmlContent.Replace("--Nombre--", nombre);
var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
var response = await client.SendEmailAsync(msg);


Console.WriteLine(response.StatusCode);

// See https://aka.ms/new-console-template for more information
