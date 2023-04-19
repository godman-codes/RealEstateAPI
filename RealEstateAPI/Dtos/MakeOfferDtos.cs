using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Dtos
{
    public class MakeOfferDtos
    {
        [Required]
        public decimal amount { get; set; }
    }
}
