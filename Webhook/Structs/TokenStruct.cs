namespace Webhook.Structs;

public struct TokenStruct
{
    public string uuid { get; set; }
    public bool redirect { get; set; }
    public string alias { get; set; }
    public bool actions { get; set; }
    public bool cors { get; set; }
    public bool expiry { get; set; }
    public int timeout { get; set; }
    public bool premium { get; set; }
    public string user_id { get; set; }
    public bool password { get; set; }
    public string ip { get; set; }
    public string user_agent { get; set; }
    public string default_content { get; set; }
    public int default_status { get; set; }
    public string default_content_type { get; set; }
    public string premium_expires_at { get; set; }
    public string description { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
}