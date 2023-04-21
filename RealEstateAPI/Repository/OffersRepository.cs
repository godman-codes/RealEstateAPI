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

        public async Task<bool> AcceptOffer(int offerId, string userId)
        {
            var offer = await _context.Offers.Where(x => x.Id == offerId && x.Listing.Owner.Id == userId).FirstOrDefaultAsync();

            if (offer == null)
            {
                return false;
            }

            offer.IsAccepted = true;

            return await Save();
        }

        public async Task<bool> RejectOffer(int offerId, string userId)
        {
            var offer = await _context.Offers.Where(x => x.Id == offerId && x.Listing.Owner.Id == userId).FirstOrDefaultAsync();

            if (offer == null)
            {
                return false;
            }

            offer.IsAccepted = false;

            return await Save();
        }

        public async Task<bool> CreateOffer(Offers Offer)
        {
            await _context.AddAsync(Offer);
            return await Save();
        }

        public async Task<bool> DeleteOffer(Offers offer)
        {
            _context.Remove(offer);
            return await Save();
        }

        public async Task<ICollection<Offers>> GetListingOffers(int listingId)
        {
            return await _context.Offers.Where(x => x.Listing.Id == listingId).Include(x => x.Owner).ToListAsync();
        }

        public async Task<Offers> GetOffer(int offerId, string userId)
        {
            return await _context.Offers.Where(x => x.Id == offerId && x.Owner.Id == userId)
                .Include(x => x.Listing).IgnoreAutoIncludes().FirstOrDefaultAsync();

        }

        public async Task<Offers> GetOfferInformation(int offerId, string userId)
        {
            return await _context.Offers.Where(x => x.Id == offerId && x.Listing.Owner.Id == userId)
                .Include(x => x.Listing)
                .Include(x => x.Owner).IgnoreAutoIncludes().FirstOrDefaultAsync();
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


        public async Task<bool> UpdateOffer(int offerId, decimal amount, string userId)
        {
            var offer = await _context.Offers.Where(x => x.Id == offerId && x.Owner.Id == userId).FirstOrDefaultAsync();
            if (offer == null)
            {
                return false;
            }

            offer.Amount = amount;
            offer.LastDateModified = DateTime.UtcNow;
            return await Save();
        }


    }
}
