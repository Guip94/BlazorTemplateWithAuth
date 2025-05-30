namespace BlazorApp2.AuthSystems.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
