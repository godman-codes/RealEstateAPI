using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Dtos
{
    public class CreateListingDto
    {
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal StartingPrice { get; set; }
        [Required]
        public bool IsListed { get; set; }
        [Required]
        public string Type { get; set; }
       
    }
}
