using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;
using System.Collections;
using System.Collections.Generic;

namespace RealEstateAPI.Controllers
{
    [Route(template: "api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<UsersOrRealtors> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationparameter;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(
          UserManager<UsersOrRealtors> userManager,
          RoleManager<IdentityRole> roleManager,
          IConfiguration configuration,
          TokenValidationParameters tokenValidationParameter,
          IAuthenticationRepository authenticationRepository
          )
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenValidationparameter = tokenValidationParameter;
            _authenticationRepository = authenticationRepository;
            _roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserOrRealtor([FromBody] RegisterUsersDto registerUsers)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (registerUsers == null)
            {
                return BadRequest(ModelState);
            }
            var user_exist = await _userManager.FindByEmailAsync(registerUsers.Email);
            if (user_exist != null)
            {
                return BadRequest(error: new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email already exist"
                    }
                });

            }
            if (registerUsers.Password != registerUsers.ConfirmPassword)
            {

                return BadRequest(error: new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Password Must match"
                    }
                });
            }
            //create user
            var new_user = new UsersOrRealtors()
            {
                Email = registerUsers.Email,
                UserName = registerUsers.Email,
                EmailConfirmed = false,
                PhoneNumber = registerUsers.PhoneNumber,
                FirstName = registerUsers.FirstName,
                LastName = registerUsers.LastName,
                DateCreated = DateTime.UtcNow,
                LastDateModified = DateTime.UtcNow,
                IsAdmin = registerUsers.IsAdmin,
                IsRealtor = registerUsers.IsRealtor,
            };
            var is_created = await _userManager.CreateAsync(new_user, registerUsers.Password);
            if (!is_created.Succeeded)
            {
                return BadRequest(

                    error: new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Server Error"
                        }
                    });
            }

            //get user from database 
            new_user = await _userManager.FindByEmailAsync(new_user.Email);

            // add roles
            string roleName = await _authenticationRepository.AddUserRoles(new_user);

            if (roleName == null)
            {
                return BadRequest(
                    error: new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Something went wrong while assigning roles"
                        }
                    }
                    );
            }

            //token 
            var token = await _authenticationRepository.GenerateJwtToken(new_user, roleName);
            if (token.Result == true)
            {
                return Ok(token);
            }
            return BadRequest(
                error: new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Something went wrong"
                    }
                });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (loginRequest == null)
            {
                return BadRequest(ModelState);
            }
            var existing_user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (existing_user == null)
            {
                return BadRequest(
                    error: new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "User does not exist"
                        },
                        Result = false
                    }
                    );
            }
            var isCorrrect = await _userManager.CheckPasswordAsync(existing_user, loginRequest.Password);
            if (!isCorrrect)
            {
                return BadRequest(error: new AuthResult()
                {
                    Errors = new List<string>()
                        {
                            "Invalid credentials"
                        },
                    Result = false
                });
            }
            ICollection<string> roleName = await _userManager.GetRolesAsync(existing_user);
            var jwtToken = await _authenticationRepository.GenerateJwtToken(existing_user, roleName.First());
            return Ok(jwtToken);
        }


        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid Parameters"
                    },
                    Result = false
                });
            }

            var result = await _authenticationRepository.VerifyAndGenerate(tokenRequest);
            if (result == null)
            {
                return BadRequest(
                       new AuthResult()
                       {
                           Errors = new List<string>()
                       {
                            "Invalid parameters"
                       },
                           Result = false
                       });
            }
            return Ok(result);
        }
    }
}
