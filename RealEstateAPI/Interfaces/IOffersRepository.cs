using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IOffersRepository
    {
        bool CreateOffer(Offers Offer);
        ICollection<Offers> GetOffers();
        UsersOrRealtors GetOfferOwner(int offerId);
        Offers GetOffer(int offerId);
        bool UpdateOffer(Offers offer);
        bool DeleteOffer(Offers offer);
        bool Save();
        bool OfferExists();
    }
}
