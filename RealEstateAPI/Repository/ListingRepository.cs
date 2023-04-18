using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Data;
using RealEstateAPI.Dtos;
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
            return await _context.Listings.Where(x => x.IsListed == true).ToListAsync();
        }

        async public Task<Listings> GetListing(int listingId)
        {
            return await _context.Listings.Where(
                x => x.Id == listingId && x.IsListed ==true
                ).FirstOrDefaultAsync();
        }

        async public Task<ICollection<Offers>> GetListingOffers(int listingId)
        {
            throw new NotImplementedException();
        }

        async public Task<ICollection<Listings>> GetListingsByOwner(string ownerId)
        {
            return _context.Listings.Where(x => x.Owner.Id == ownerId).ToList();
        }

        async public Task<bool> ListingExist(int Id)
        {
            return await _context.Listings.AnyAsync(x => x.Id == Id);
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

        async public Task<bool> UpdateListing(CreateListingDto listingToUpdate, int listingId)
        {
            var listing = _context.Listings.Where(x => x.Id == listingId).FirstOrDefault();

            listing.StreetAddress = listingToUpdate.StreetAddress;
            listing.City = listingToUpdate.City;
            listing.Country = listingToUpdate.Country;
            listing.State = listingToUpdate.State;
            listing.PostalCode = listingToUpdate.PostalCode;
            listing.StartingPrice = listingToUpdate.StartingPrice;
            listing.IsListed = listingToUpdate.IsListed;
            listing.Type = listingToUpdate.Type;
            listing.LastDateModified = DateTime.UtcNow;

            return await Save();
        }
    }
}
