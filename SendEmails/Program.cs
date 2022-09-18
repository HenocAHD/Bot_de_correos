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
var htmlContent = File.ReadAllText("C:\\Users\\Henoc Hernandez\\Documents\\Bot_de_correos\\MailBot.SendGrid\\bin\\Debug\\net6.0\\templates\\jones Dixie.html");
htmlContent = htmlContent.Replace("--Nombre--", nombre);
var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
var response = await client.SendEmailAsync(msg);


Console.WriteLine(response.StatusCode);

// See https://aka.ms/new-console-template for more information
