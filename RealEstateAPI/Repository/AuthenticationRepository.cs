using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateAPI.Data;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateAPI.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<UsersOrRealtors> _userManager;

        public AuthenticationRepository(
            ApiDbContext context,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameter,
            UserManager<UsersOrRealtors> userManager
            )
        {
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameter;
            _userManager = userManager;
            


        }
        public Task<bool> CreateRefreshToken(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }

       

        public async Task<bool> Save()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
            
        }

        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(
                year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0, DateTimeKind.Utc
                );
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeVal;
        }

        public async Task<AuthResult> VerifyAndGenerate(TokenRequestDto tokenRequest)
        {
            // token handler 
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                _tokenValidationParameters.ValidateLifetime = false;

                //  validate the refresh token and return a validatedToken variable
                var tokenVerification = jwtTokenHandler.ValidateToken(
                    tokenRequest.Token, _tokenValidationParameters, out var validatedToken
                    );
                var utcExpiryDate = long.Parse(tokenVerification.Claims.FirstOrDefault(
                    x => x.Type == JwtRegisteredClaimNames.Exp
                    ).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Result = true,
                        Errors = new List<string>()
                        {
                            "Expired Token"
                        }
                    };

                }
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(
                    x => x.Token == tokenRequest.RefreshToken
                    );

                if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>
                            {
                                "Invalid token"
                            }
                    };
                }

                string jti = tokenVerification.Claims.FirstOrDefault(
                    x => x.Type == JwtRegisteredClaimNames.Jti
                    ).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>
                        {
                            "Invalid token"
                        }
                    };
                }

                if (storedToken.ExpiryDate < DateTime.UtcNow)
                    {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<String>()
                            {
                                "Expired token2"
                            }
                    };
                    }
                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await Save();
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);
                
            }
            catch (Exception)
            {

                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                            {
                                "Server Error"
                            }
                };
            }
        }

        public Task<bool> VerifyuserCredentials(UserLoginDto userRequest)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthResult> GenerateJwtToken(UsersOrRealtors user)
        {
            // token handler for generating token
            var jwtTokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection(key: "JwtConfig:Secret").Value);

            //token description
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        //claims
                        new Claim(type: "Id", value: user.Id),
                        new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
                        new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                        new Claim(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(type: JwtRegisteredClaimNames.Iat, value: DateTime.Now.ToUniversalTime().ToString())
                    }
                    ),
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeVFrame").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenhandler.CreateToken(tokenDescription);
            var jwtToken = jwtTokenhandler.WriteToken(token);

            // refresh generator
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = RandomStringGeneration(25),
                AddedData = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Result = true
            };
        }

        public string RandomStringGeneration(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";
            return new string(
                Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
                );
        }

    }
}
