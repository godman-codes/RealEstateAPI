namespace RealEstateAPI.Dtos
{
    public class OfferResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsAccepted { get; set; }
        public ListingResponseDto Listing { get; set; }
        // very important so you can amnipulate the nested responses
        public UserOrRealtorDto Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateModified { get; set; }
    }
}
