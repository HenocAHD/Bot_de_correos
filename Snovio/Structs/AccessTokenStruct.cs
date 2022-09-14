using System;
namespace Snovio.Structs;

public struct AccessTokenStruct
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public DateTime expires { get; set; }
}