using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;



namespace RealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingsRepository _listingRepository;

        public ListingController(IListingsRepository listingsRepository)
        {
               _listingRepository = listingsRepository;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
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
                LastDateModified = DateTime.UtcNow
            };
            if (! await _listingRepository.CreateListing(listing))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
    }
}
