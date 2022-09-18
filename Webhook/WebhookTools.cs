using Flurl.Http;
using System.Net.Http;
using Flurl;
using WebhookApi.Structs;
using Linkedin.Net;
using System.Text.Json;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using Linkedin.Net.objects;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Linkedin.Net.Exceptions;
using Linkedin.Net.Estructuras;
using System.Collections.Generic;
using Serilog;
using MihaZupan;
using Google.Protobuf.WellKnownTypes;

namespace Webhook;

public class WebhookTools
{
    //variables
    private const string _url = "https://5923-45-170-32-96.ngrok.io/api/webhook";
    private string _token = "93a2ee7f-7bb6-4126-98f6-e3935ebdaab2";
    private string _apiKey;
    private Proxy _Proxy;
    private bool _UseProxy;
    private FlurlClient _httpClient;
    public List<string> _contents = new List<string>();
    private List<string> _uuids = new List<string>();
    public List<string> _contentes = new List<string>();

    //propiedades
    public FlurlClient HttpClient
    {
        get
        {
            return this._httpClient;
        }
    }

    public List<string> GetUuids
    {
        get
        {
            return this._uuids;
        }
    }

    //metodos
    public WebhookTools()
    {
        
    }

    public async Task<string> GetWebhookData()
    {
        var response = await $"{_url}"
            .GetStringAsync();
        
        return response;
    }

    public async Task<WebhookStructList> GetWebhookDataLikeList()
    {
        var listResponse = await GetWebhookData();

        var vlue3 = "{\"content\": " + listResponse + "}";
        var webhookreceiver = JsonSerializer.Deserialize<WebhookStructList>(vlue3);
        
        return webhookreceiver;
    }

    public async Task<webhookStruct> getWebhookDataById(string id)
    {
        var response = await $"{_url}/{id}"
            .GetJsonAsync<webhookStruct>();

        return response;
    }

    public async Task<string> Actualizar(string id)
    {
        var response = await $"{_url}"
            .PutJsonAsync(id);
        return response.StatusCode.ToString();
    }
}