using Newtonsoft.Json;

namespace Webhook.Structs
{

    public struct SendgridWebhookStruct
    {
        public List<WebhookDataStruct> data { get; set; }
        public int total { get; set; }
        public int per_page { get; set; }
        public int current_page { get; set; }
        public bool is_last_page { get; set; }
        public int from { get; set; }
        public int to { get; set; }
    }

    public struct WebhookDataStruct 
    {
        public string uuid { get; set; }
        public string type { get; set; }
        public string token_id { get; set; }
        public string ip { get; set; }
        public string hostname { get; set; }
        public string method { get; set; }
        public string user_agent { get; set; }
        public string content { get; set; }
    }
}
