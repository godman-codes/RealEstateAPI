

namespace RealEstateAPI.Dtos
{
    public class RealtorsOfferResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsAccepted { get; set; }
        public UserOrRealtorDto Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateModified { get; set; }
    }
}
