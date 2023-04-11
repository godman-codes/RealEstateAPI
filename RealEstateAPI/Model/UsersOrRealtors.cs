
using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Model
{
    public class UsersOrRealtors
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsRealtor { get; set; }
        public ICollection<Listings> Listings { get; set; }
        public ICollection<Offers> Offers { get; set; }
    }
}
