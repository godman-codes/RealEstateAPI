using RealEstateAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Dtos
{
    public class CreateUsersOrRealtorDtos

    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        public bool IsRealtor { get; set; }
    }
}
