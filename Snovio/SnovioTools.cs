using System;
using System.Threading.Tasks;
using Snovio;
using Flurl.Http;
using Snovio.Structs;

namespace Snovio;

public class SnovioTools
{
    private string _token;
    private string SnovioApiUrl = "https://api.snov.io/";
    private static int countRequest = 0;

    public SnovioTools(string userId, string userSecret)
    {
        var snov = new Authentication(userId, userSecret);
        this._token = snov._token;
    }

    //agraga al prospecto a una lista para poder ser encontrado
    public async Task<string> addUrlToSearchForProspect(string urlProspect)
    {
        var parametros = new
        {
            access_token = this._token,
            url = urlProspect + "$LinkedIn"
        };

        var response = await $"{SnovioApiUrl}v1/add-url-for-search"
            .PostJsonAsync(parametros).ReceiveString();
        Console.WriteLine(response + "siuuuu");
        return response;
    }

    //obtiene los datos del prospecto
    public async Task<ProspectStruct> getProspectFromUrL(string urlProspect)
    {

        if (countRequest >= 60)
        {
            await Task.Delay(90000);
            countRequest = 0;
        }

        await addUrlToSearchForProspect(urlProspect);

        var parametros = new
        {
            access_token = this._token,
            url = urlProspect + "&LinkedIn"
        };
        var response = await $"{SnovioApiUrl}v1/get-emails-from-url"
            .PostJsonAsync(parametros).ReceiveJson<ProspectStruct>();

        countRequest += 2;
        Console.WriteLine(countRequest);

        return response;
    }

    // obtiene el email del prospecto
    public async Task<ProspectEmailsStruct> getEmails(string urlProfileLInkedin)
    {
        try
        {
            

            var prospect = getProspectFromUrL(urlProfileLInkedin).GetAwaiter().GetResult();
            var parametros = new
            {
                access_token = this._token,
                domain = prospect.data.currentJob[0].site,
                firstName = prospect.data.firstName,
                lastName = prospect.data.lastName

            };

            var response = await $"{SnovioApiUrl}v1/get-emails-from-names"
                .PostJsonAsync(parametros).ReceiveJson<ProspectEmailsStruct>();

            

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ProspectEmailsStruct();
        }

    }
}