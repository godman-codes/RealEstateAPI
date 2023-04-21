using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;
using System.Security.Claims;

namespace RealEstateAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingsRepository _listingRepository;
        private readonly UserManager<UsersOrRealtors> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserOrRealtorRepository _userOrrealtorRepository;

        public ListingController(
            IListingsRepository listingsRepository,
            UserManager<UsersOrRealtors> userManager,
            IMapper mapper,
            IUserOrRealtorRepository userOrRealtorRepository
            )
        {
            _listingRepository = listingsRepository;
            _userManager = userManager;
            _mapper = mapper;
            _userOrrealtorRepository = userOrRealtorRepository;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Realtor,Admin")]
        public async Task<IActionResult> CreateListings([FromBody] CreateListingDto listingToCreate)
        {
            if (listingToCreate == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            Listings listing = new Listings()
            {
                StreetAddress = listingToCreate.StreetAddress,
                City = listingToCreate.City,
                PostalCode = listingToCreate.PostalCode,
                Country = listingToCreate.Country,
                State = listingToCreate.State,
                Type = listingToCreate.Type,
                IsListed = listingToCreate.IsListed,
                StartingPrice = listingToCreate.StartingPrice,
                DateCreated = DateTime.UtcNow,
                LastDateModified = DateTime.UtcNow,
                Owner = user
            };
            if (! await _listingRepository.CreateListing(listing))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAvailableListings()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var availableListings = _mapper.Map<ICollection<ListingResponseDto>>(await _listingRepository.GetAvailableListings());

            if (availableListings.Count == 0)
            {
                return Ok("No avilable listings");
            }
            return Ok(availableListings);
        }

        [HttpGet("{listingId}")]
        [Authorize]
        public async Task<IActionResult> GetListing(int listingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (! await _listingRepository.ListingExist(listingId))
            {
                return NotFound();
            }

            var listing = _mapper.Map<ListingResponseDto>(await _listingRepository.GetListing(listingId));

            if (listing == null)
            {
                return NotFound();
            }
            return Ok(listing);

        }


        [HttpPut("{listingId}")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<IActionResult> UpdateListing([FromBody] CreateListingDto listingUpdate, int listingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (listingUpdate == null)
            {
                return BadRequest(ModelState);
            }
            
            if (! await _listingRepository.ListingExist(listingId))
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _listingRepository.verifyOwner(userId, listingId))
            {
                return Unauthorized();
            }

            if (! await _listingRepository.UpdateListing(listingUpdate, listingId))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpGet("{listingId}/offers")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<IActionResult> GetListingOffers(int listingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (listingId == 0)
            {
                return BadRequest(ModelState);
            }

            if (!await _listingRepository.ListingExist(listingId))
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _listingRepository.verifyOwner(userId, listingId))
            {
                return Unauthorized();
            }

            ICollection<RealtorsOfferResponseDto> offers = _mapper.Map<ICollection<RealtorsOfferResponseDto>>(await _listingRepository.GetListingOffers(listingId));

            if (offers == null)
            {
                ModelState.AddModelError("No Offer", "You dont have any Offers for this property");
                return Ok(ModelState);

            }
            return Ok(offers);
        }


        
        
    }
}
