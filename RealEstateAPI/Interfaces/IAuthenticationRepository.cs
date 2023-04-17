using RealEstateAPI.Dtos;
using RealEstateAPI.Model;
using System.Security.Claims;

namespace RealEstateAPI.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<AuthResult> VerifyAndGenerate(TokenRequestDto tokenRequest);
        DateTime UnixTimeStampToDateTime(long unixTimeStamp);
        Task<bool> Save();
        Task<bool> CreateRefreshToken(RefreshToken refreshToken);
        Task<bool> VerifyuserCredentials(UserLoginDto userRequest);
        Task<AuthResult> GenerateJwtToken(UsersOrRealtors user, string role);
        string RandomStringGeneration(int length);
        Task<string> AddUserRoles(UsersOrRealtors usersOrRealtors);
        //Task<ICollection<Claim>> CreateUserClaims(UsersOrRealtors user);
    }
}
