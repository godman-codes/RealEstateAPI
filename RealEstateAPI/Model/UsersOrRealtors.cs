
using Microsoft.AspNetCore.Identity;

namespace RealEstateAPI.Model
{
    public class UsersOrRealtors : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsRealtor { get; set; }
        public ICollection<Listings> Listings { get; set; }
        public ICollection<Offers> Offers { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateModified { get; set; }
    }
}
