using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Dtos
{
    public class RegisterUsersDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "passwords must match")]
        public string ConfirmPassword { get; set; }
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
