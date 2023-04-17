using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using System.Security.Claims;

namespace RealEstateAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrRealtorController : ControllerBase
    {
        private readonly IUserOrRealtorRepository _userOrReltorRepository;
        private readonly IMapper _mapper;

        public UserOrRealtorController(IUserOrRealtorRepository userOrReltorRepository, IMapper mapper)
        {
            _userOrReltorRepository = userOrReltorRepository;
            _mapper = mapper;
        }


        [HttpGet("GetRealtorListings")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<IActionResult> GetRealtorListings()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listings = _mapper.Map<List<ListingResponseDto>>(await _userOrReltorRepository.GetRealtorUserListings(userId));
            if (listings.Count == 0)
            {
                return Ok("you have no listings");
            }
            return Ok(listings);

        }
    }
}
