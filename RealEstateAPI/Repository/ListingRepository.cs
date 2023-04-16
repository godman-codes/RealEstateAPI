using RealEstateAPI.Data;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;

namespace RealEstateAPI.Repository
{
    public class ListingRepository : IListingsRepository
    {
        private readonly ApiDbContext _context;

        public ListingRepository(ApiDbContext context)
        {
            _context = context;
        }
        async public Task<bool> CreateListing(Listings listing)
        {
            await _context.AddAsync(listing);
            return await Save();
        }

        async public Task<bool> DeleteListing(Listings listing)
        {
            throw new NotImplementedException();
        }

        async public Task<ICollection<Listings>> GetAvailableListings()
        {
            throw new NotImplementedException();
        }

        async public Task<Listings> GetListing(int listingId)
        {
            throw new NotImplementedException();
        }

        async public Task<ICollection<Offers>> GetListingOffers(int listingId)
        {
            throw new NotImplementedException();
        }

        async public Task<ICollection<Listings>> GetListingsByOwner(int ownerId)
        {
            throw new NotImplementedException();
        }

        async public Task<bool> ListingExist(int Id)
        {
            throw new NotImplementedException();
        }

        async public Task<bool> MakeListingAvailable(Listings listing)
        {
            throw new NotImplementedException();
        }

        async public Task<bool> Save()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }

        async public Task<bool> UpdateListing(Listings listing)
        {
            throw new NotImplementedException();
        }

        async public Task<bool> UpdateListings(Listings listing)
        {
            throw new NotImplementedException();
        }
    }
}
