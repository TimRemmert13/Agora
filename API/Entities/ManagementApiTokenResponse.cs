namespace API.Entities
{
    public class ManagementApiTokenResponse
    {
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
    }
}