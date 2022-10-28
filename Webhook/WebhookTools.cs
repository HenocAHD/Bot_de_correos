using Flurl.Http;
using System.Net.Http;
using Flurl;
using WebhookApi.Structs;
using System.Text.Json;
using Linkedin.Net.objects;


namespace Webhook;

public class WebhookTools
{
    //variables
    private const string _url = "https://webhook.panamify.com/api/webhook";
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

    public async Task<List<webhookStruct>> GetWebhookData()
    {
        var response = await $"{_url}"
            .GetJsonAsync<List<webhookStruct>>();
        
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