using System.Text.Json.Serialization;

namespace Webhook.Structs
{
    public struct SengridContentStruct
    {
       public List<ContentStruct> content { get; set; }
    }

    public struct ContentStruct
    {
        public string email { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }
    }
}