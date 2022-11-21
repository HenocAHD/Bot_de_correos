using Linkedin.Net.Estructuras;
using Linkedin.Net.objects;
using Linkedin.Net.Models;
using Google.Protobuf.WellKnownTypes;
using Linkedin.Net.Db;
using System.Reflection.Metadata;
using Quartz;

var cliente = clients.SelectClientLinkNoUpdate(database.getdatabase());
var lidi = new Linkedin.Net.Linkedin(new Login { email = "mariocasasmac@gmail.com", password = "mancha0708" }, true);
lidi.Login().Wait();

int obtenidos = 0;

foreach(var client in cliente)
{
    try
    {
        var clientUrn = client.client_url.Replace("https://www.linkedin.com/in/", "");
        var publicID = lidi.GetProfileIdentifier(clientUrn).GetAwaiter().GetResult();

        if (publicID != null)
        {
            var newClientUl = "https://www.linkedin.com/in/" + publicID + "/";
            Console.WriteLine(newClientUl);
            client.client_url = newClientUl;
            client.client_url_update = true;
            client.Update(database.getdatabase());
            obtenidos++;
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    Task.Delay(5000).Wait();
}
Console.WriteLine(obtenidos);




