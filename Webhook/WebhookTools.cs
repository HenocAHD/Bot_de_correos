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
    private const string _url = "https://webhook.site/";
    private string _token = "93a2ee7f-7bb6-4126-98f6-e3935ebdaab2";
    private string _apiKey;
    private Proxy _Proxy;
    private bool _UseProxy;
    private FlurlClient _httpClient;
    public List<string> _contents = new List<string>();
    private SengridContentStruct _content;
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
    public WebhookTools(string apiKey, Proxy proxy)
    {
        this._apiKey = apiKey;
        this._Proxy = proxy;
        this._httpClient = new FlurlClient(new HttpClient(this._Proxy.HttpHandler));
        this._UseProxy = true;

    }
    public WebhookTools(string apiKey)
    {
        this._apiKey = apiKey;
        this._httpClient = new FlurlClient();
        this._UseProxy = false;

    }

    public async Task<string> GetIp()
    {
        var r = await "https://api.ipify.org"
            .WithClient(this._httpClient)
            .GetAsync();
        return await r.ResponseMessage.Content.ReadAsStringAsync();
    }

    public async Task GetWebhookData()
    {
        int pages = 1;
        int total = 0;
        bool stop = false;
        //if (this._UseProxy)
        //{
        //    var proxy = new HttpToSocks5Proxy(this._Proxy.Ip, Convert.ToInt32(this._Proxy.Port), this._Proxy.User, this._Proxy.Password);
        //    proxy.ResolveHostnamesLocally = true;
        //    var httpclient = new HttpClient(
        //        new HttpClientHandler()
        //        {
        //            Proxy = proxy,
        //            UseProxy = true
        //        });
        //    this._httpClient = new FlurlClient(httpclient);
        //}
        do
        {
            var response = await $"{_url}token/{this._token}/requests?per_page=100&page={pages}"
                .WithClient(this._httpClient)
                .WithHeader("api-key", _apiKey)
                .GetJsonAsync<SendgridWebhookStruct>();

        foreach (var item in response.data)
        {
            total++;
            _uuids.Add(item.uuid);
        }

        if (response.is_last_page == false)
        {
            pages++;
        }
        else
        {
            stop = true;
        }
    } while (stop == false);


    Console.WriteLine("uuids ingresados: "+total);
    }

    public async Task<string> GetDataRaw(String uuid)
    {

        var response = await $"{_url}token/{this._token}/request/{uuid}/raw"
            .WithClient(this._httpClient)
            .GetStringAsync();

        response = "{\"content\": " + response +"}";
 

        return response;
    }

    public async Task<List<string>> GetAllDataRaw()
    {
        var emailsOpened = new List<string>();
        await GetWebhookData();
        foreach (var item in _uuids){
            var result = await GetDataRaw(item);
            var raw = JsonSerializer.Deserialize<SengridContentStruct>(result);

            if (raw.content[0].Event == "open")
            {
                emailsOpened.Add(raw.content[0].email);
            }

            await DeleteDataRaw(item);
        }
        return emailsOpened;
    }

    public async Task DeleteDataRaw(string uuid)
    {
        if(uuid != String.Empty | uuid == "")
        {
            var response = await $"{_url}token/{this._token}/request/{uuid}"
            .WithClient(this._httpClient)
            .DeleteAsync();
        }
        else
        {
            Console.WriteLine("No se ingreso ningun id");
        }
    }
}