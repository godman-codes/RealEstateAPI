using RealEstateAPI.Dtos;
using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IListingsRepository
    {
        Task<bool> CreateListing(Listings listing);
        Task<ICollection<Listings>> GetAvailableListings();
        Task<Listings> GetListing(int listingId);
        Task<bool> UpdateListing(CreateListingDto listingToUpdate, int listingsId);
        Task<bool> MakeListingAvailable(Listings listing);
       
        Task<bool> DeleteListing(Listings listing);
        Task<ICollection<Listings>> GetListingsByOwner(string ownerId);
        Task<ICollection<Offers>> GetListingOffers (int listingId);
        Task<bool> ListingExist(int Id);
        Task<bool> Save();
    }
}
