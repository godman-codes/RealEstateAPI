using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using System.Security.Claims;

namespace RealEstateAPI.Controllers
{

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImageController : ControllerBase
    {
        private readonly IUserOrRealtorRepository _userOrRealtorRepository;
        private readonly IImagesRepository _imagesRepository;
        private readonly IListingsRepository _listingsRepository;

        public ImageController(IUserOrRealtorRepository userOrRealtorRepository, IImagesRepository imagesRepository, IListingsRepository listingsRepository)
        {
            _userOrRealtorRepository = userOrRealtorRepository;
            _imagesRepository = imagesRepository;
            _listingsRepository = listingsRepository;
        }


        [HttpPost("{listingId}")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<IActionResult> Image(int listingId, [FromForm] PostImagesDto fileobj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (listingId == 0)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listing = await _userOrRealtorRepository.GetRealtorUserListingById(userId, listingId);

            if (listing == null)
            {
                ModelState.AddModelError("", "listing not found");
                return NotFound(ModelState);
            }

            if (fileobj.file.Length > 0)
            {
                if (!await _imagesRepository.AddImages(fileobj.file, listing))
                {
                    ModelState.AddModelError("Server Error", "Something went wrong whil uploading image");
                    return StatusCode(500, ModelState);
                }
                return Ok("ImageAdded");
            }
            return BadRequest();
        }

        [HttpGet("{ImageId}")]
        [Authorize]
        public async Task<IActionResult> GetImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (imageId == 0)
            {
                return BadRequest(ModelState);
            }

            var image = await _imagesRepository.GetImage(imageId);

            if (image == null)
            {
                return NotFound();
            }

            MemoryStream ms = new MemoryStream(image.Image);

            return new FileStreamResult(ms, "image/jpeg");
        }

        [HttpDelete("{ImageId}")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (imageId == 0)
            {
                return BadRequest(ModelState);
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var image = await _imagesRepository.GetImage(imageId);

            if (image == null)
            {
                return NotFound();
            }

            if (! await _listingsRepository.verifyOwner(userId, image.Listing.Id))
            {
                ModelState.AddModelError("Unauthorized", "you are not authorized to carry out this action");
                return Unauthorized(ModelState);
            }

            if (!await _imagesRepository.DeleteImage(image))
            {
                ModelState.AddModelError("Server Error", "Somthing went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
