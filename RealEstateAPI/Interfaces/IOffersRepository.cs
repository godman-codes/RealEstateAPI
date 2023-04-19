using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IOffersRepository
    {
        Task<bool> CreateOffer(Offers Offer);
        Task<ICollection<Offers>> GetOffers();
        Task<UsersOrRealtors> GetOfferOwner(int offerId);
        Task<Offers> GetOffer(int offerId, string userId);
        Task<bool> UpdateOffer(Offers offer);
        Task<bool> DeleteOffer(Offers offer);
        Task<bool> Save();
        Task<bool> OfferExists();
        Task<ICollection<Offers>> GetListingOffers(int listingId);
    }
}
