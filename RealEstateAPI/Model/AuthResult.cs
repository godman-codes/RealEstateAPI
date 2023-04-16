namespace RealEstateAPI.Model
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Errors { get; set; }
    }
}
