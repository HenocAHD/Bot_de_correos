namespace Webhook_Api.Models
{
    public class UserDto
    {
        public int id_user { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
