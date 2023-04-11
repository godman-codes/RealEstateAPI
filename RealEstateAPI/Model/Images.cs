using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Model
{
    public class Images
    {
        [Key]
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public Listings Listing { get; set; }
    }
}
