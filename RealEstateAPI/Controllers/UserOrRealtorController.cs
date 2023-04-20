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
        private readonly IListingsRepository _listingRepository;

        public UserOrRealtorController(
            IUserOrRealtorRepository userOrReltorRepository,
            IMapper mapper,
            IListingsRepository listingRepository)
        {
            _userOrReltorRepository = userOrReltorRepository;
            _mapper = mapper;
            _listingRepository = listingRepository;
        }


        [HttpGet("Listings")]
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


        [HttpGet("Listing/{listingId}")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<IActionResult> GetRealtorListingById(int listingId)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _listingRepository.ListingExist(listingId))
            {
                return NotFound();
            }

            var listing = _mapper.Map<ListingResponseDto>(await _userOrReltorRepository.GetRealtorUserListingById(userId, listingId));

            if (listing == null)
            {
                return Unauthorized();
            }
            return Ok(listing);
        }

    }
}
