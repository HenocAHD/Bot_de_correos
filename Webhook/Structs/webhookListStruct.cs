using System.Text.Json.Serialization;

namespace Webhook.Structs
{
    public struct webhookListStruct
    {
        public List<webhookStruct> content { get; set; }
    }

    public struct webhookStruct
    {
        private DateTime _create_at;

        public int id_webhook_data { get; set; }
        public string email { get; set; }
        public string @event { get; set; }
        public string ip { get; set; }
        public string sg_content_type { get; set; }
        public string sg_event_id { get; set; }
        public bool sg_machine_open { get; set; }
        public string sg_message_id { get; set; }
        public int timestamp { get; set; }
        public string useragent { get; set; }
        public bool required { get; set; }

        public DateTime create_at
        {
            get
            {
                if (_create_at == DateTime.MinValue)
                {
                    _create_at = DateTime.Now;
                }

                return _create_at;
            }
            set { _create_at = value; }
        }
    }
}