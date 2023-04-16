using RealEstateAPI.Dtos;
using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<AuthResult> VerifyAndGenerate(TokenRequestDto tokenRequest);
        DateTime UnixTimeStampToDateTime(long unixTimeStamp);
        Task<bool> Save();
        Task<bool> CreateRefreshToken(RefreshToken refreshToken);
        Task<bool> VerifyuserCredentials(UserLoginDto userRequest);
        Task<AuthResult> GenerateJwtToken(UsersOrRealtors user);
        string RandomStringGeneration(int length);
    }
}
