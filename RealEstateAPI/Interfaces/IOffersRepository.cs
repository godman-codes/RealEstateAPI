using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IOffersRepository
    {
        Task<bool> CreateOffer(Offers Offer);
        Task<ICollection<Offers>> GetOffers();
        Task<UsersOrRealtors> GetOfferOwner(int offerId);
        Task<Offers> GetOffer(int offerId, string userId);
        Task<bool> UpdateOffer(int offerId, decimal amount, string userId);
        Task<bool> DeleteOffer(Offers offer);
        Task<bool> Save();
        Task<bool> OfferExists();
        Task<Offers> GetOfferInformation(int offerId, string userId);
        Task<bool> AcceptOffer(int offerId, string userId);
        Task<bool> RejectOffer(int offerId, string userId);
    }
}
