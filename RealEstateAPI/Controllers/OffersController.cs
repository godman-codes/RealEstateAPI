using AutoMapper;
using AutoMapper.Configuration.Annotations;
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

    public class OffersController : ControllerBase
    {
        private readonly IOffersRepository _offersRepository;
        private readonly IMapper _mapper;
        private readonly IListingsRepository _listingRepository;
        private readonly UserManager<UsersOrRealtors> _userManager;

        public OffersController(
            IOffersRepository offersRepository,
            IMapper mapper,
            IListingsRepository listingsRepository,
            UserManager<UsersOrRealtors> userManager
            )
        {
            _offersRepository = offersRepository;
            _mapper = mapper;
            _listingRepository = listingsRepository;
            _userManager = userManager;
            
        }


        [HttpPost("Listing/{listingId}")]
        [Authorize]
        public async Task<IActionResult> MakeOfferForListing([FromBody] MakeOfferDtos offerToMake, int listingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (offerToMake== null)
            {
                return BadRequest(ModelState);
            }

            if (listingId==0)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(ModelState);
            }

            Listings listing = await _listingRepository.GetListing(listingId);

            if ((listing.StartingPrice * (decimal)0.2) > offerToMake.amount)
            {
                ModelState.AddModelError("Invalid Amount", "Amoount offered must be more than 20% the starting price");
                return BadRequest(ModelState);
            }

            if (listing == null)
            {
                return NotFound();
            }

            Offers offer = new Offers()
            {
                Amount = offerToMake.amount,
                DateCreated = DateTime.UtcNow,
                LastDateModified = DateTime.UtcNow,
                Listing = listing,
                Owner = user
            };

            if (! await _offersRepository.CreateOffer(offer))
            {
                ModelState.AddModelError("", "Something went weong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("offer made successfully ");
        }

        [HttpGet("{offerId}")]
        [Authorize]
        public async Task<IActionResult> GetOffer(int offerId)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState); 
            }

            if (offerId == 0)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var offer = _mapper.Map<OfferResponseDto>(await _offersRepository.GetOffer(offerId, userId));
            if (offer == null)
            {
                return NotFound();
            }
            return Ok(offer);
        }

        [HttpGet("OfferInformation/{offerId}")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<IActionResult> GetOfferinformation(int offerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (offerId == 0)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            OfferResponseDto offer = _mapper.Map<OfferResponseDto>(await _offersRepository.GetOfferInformation(offerId, userId));

            if (offer == null)
            {
                return NotFound();
            }

            return Ok(offer);
        }


        [HttpPut("{offerId}")]
        [Authorize]
        public async Task<IActionResult> UpdateOffer(int offerId, [FromBody] MakeOfferDtos offerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (offerId == 0)
            {
                return BadRequest(ModelState);
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var offer = await _offersRepository.GetOffer(offerId, userId);

            if ((offer.Listing.StartingPrice * (decimal)0.2) > offerToUpdate.amount)
            {
                ModelState.AddModelError("Invalid Amount", "Amoount offered must be more than 20% the starting price");
                return BadRequest(ModelState);
            }

            if (offer == null)
            {
                return NotFound();
            }

            if (! await _offersRepository.UpdateOffer(offerId, offerToUpdate.amount, userId))
            {
                ModelState.AddModelError("UpdateError", "Something went wrong while updateing");
            }
            return NoContent();
            

        }

        [HttpDelete("{offerId}")]
        [Authorize]
        public async Task<IActionResult> DeleteOffer(int offerId)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (offerId == 0)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var offerToDelete = await _offersRepository.GetOffer(offerId, userId);

            if (offerToDelete == null)
            {
                return NotFound();
            }

            if (!await _offersRepository.DeleteOffer(offerToDelete))
            {
                ModelState.AddModelError("deleteError", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
