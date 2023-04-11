using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Model
{
    public class Offers
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Listings Listing { get; set; }
        public UsersOrRealtors Owner { get; set; }
    }
}
