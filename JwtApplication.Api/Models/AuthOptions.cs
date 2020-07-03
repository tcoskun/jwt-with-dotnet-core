namespace JwtApplication.Models
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecureKey { get; set; }
    }
}
