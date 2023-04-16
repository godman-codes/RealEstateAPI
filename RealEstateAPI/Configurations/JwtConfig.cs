namespace RealEstateAPI.Configurations
{
    public class JwtConfig // must be that same with name used in app settings 
    {
        public string Secret { get; set; }
        public TimeSpan ExpiryTime { get; set; }
    }
}
