using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;

namespace RealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrRelatorController : ControllerBase
    {
        private readonly IUserOrRelatorRepository _userOrReltorRepository;

        public UserOrRelatorController(IUserOrRelatorRepository userOrReltorRepository)
        {
            _userOrReltorRepository = userOrReltorRepository;
        }


        //[HttpPost]
        //public async Task<IActionResult> CreateUserorRelator([FromBody] CreateUsersOrRealtorDtos userOrRealtorToCteate) 
        //{
        //    if (userOrRealtorToCteate == null)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    UsersOrRealtors usersOrRealtors = new UsersOrRealtors()
        //    {
        //        FirstName = userOrRealtorToCteate.FirstName,
        //        LastName = userOrRealtorToCteate.LastName,
        //        PhoneNumber = userOrRealtorToCteate.PhoneNumber,
        //        IsRealtor = userOrRealtorToCteate.IsRealtor,
        //        IsAdmin = userOrRealtorToCteate.IsAdmin,
        //        DateCreated = DateTime.UtcNow,
        //        LastDateModified = DateTime.UtcNow
        //    };

        //    if (!await _userOrReltorRepository.CreateUser(usersOrRealtors))
        //    {
        //        ModelState.AddModelError("", "something went wrong while saving");
        //        return StatusCode(500, ModelState);
        //    }
        //    return Ok("created");
        //}
    }
}
