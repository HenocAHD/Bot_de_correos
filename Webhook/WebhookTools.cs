using Flurl.Http;
using Flurl;
using Webhook.Structs;
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

namespace Webhook;

public class WebhookTools
{
    //variables
    private const string _url = "localhost:5160/api/webhook";
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

    public async Task<webhookListStruct> GetWebhookData()
    {
        var response = await $"{_url}"
            .GetJsonAsync<webhookListStruct>();
        return response;
    }
}