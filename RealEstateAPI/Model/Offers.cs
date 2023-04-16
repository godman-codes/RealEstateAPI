using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Model
{
    public class Offers
    {
        public Offers()
        {
            this.IsAccepted = false;
        }
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Listings Listing { get; set; }
        public UsersOrRealtors Owner { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateModified { get; set;  }
    }
}
