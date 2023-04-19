using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Data;
using RealEstateAPI.Dtos;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;

namespace RealEstateAPI.Repository
{
    public class OffersRepository : IOffersRepository
    {
        private readonly ApiDbContext _context;

        public OffersRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOffer(Offers Offer)
        {
            await _context.AddAsync(Offer);
            return await Save();
        }

        public Task<bool> DeleteOffer(Offers offer)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Offers>> GetListingOffers(int listingId)
        {
            return await _context.Offers.Where(x => x.Listing.Id == listingId).Include(x => x.Owner).ToListAsync();
        }

        public async Task<Offers> GetOffer(int offerId, string userId)
        {
            return await _context.Offers.Where(x => x.Id == offerId && x.Owner.Id == userId).Include(x => x.Listing).IgnoreAutoIncludes().FirstOrDefaultAsync();

        }

        public Task<UsersOrRealtors> GetOfferOwner(int offerId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Offers>> GetOffers()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OfferExists()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var save =  await _context.SaveChangesAsync();
            return save > 0;
        }

        public Task<bool> UpdateOffer(Offers offer)
        {
            throw new NotImplementedException();
        }

    }
}
