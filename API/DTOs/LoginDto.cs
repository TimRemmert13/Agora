using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class LoginDto
    {
        [JsonPropertyName("response_type")]
        public string ResponseType { get; set; }
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("connection")]
        public string Connection { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}