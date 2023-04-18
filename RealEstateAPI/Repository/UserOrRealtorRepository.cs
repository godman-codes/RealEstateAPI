using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Data;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;

namespace RealEstateAPI.Repository
{
    public class UserOrRealtorRepository : IUserOrRealtorRepository
    {
        private readonly ApiDbContext _context;

        public UserOrRealtorRepository(ApiDbContext context)
        {
            _context = context;

        }
       

        public async Task<bool> DeleteUser(UsersOrRealtors usersOrRealtors)
        {
            throw new NotImplementedException();
        }

        public async Task<Listings> GetRealtorUserListingById(string userId, int listingId)
        {
            return await _context.Listings.Where(x => x.Id == listingId && x.Owner.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Listings>> GetRealtorUserListings(string userId)
        {
            return await _context.Listings.Where(x => x.Owner.Id == userId).ToListAsync();
        }

        public async Task<UsersOrRealtors> GetUserOrRelator(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<UsersOrRealtors> GetUsersOrRealtors()
        {
            return _context.Users.OrderBy(x => x.Id).ToList();
        }

        public async Task<bool> Save()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }


        public async Task<bool> UserOrRelatorExist(int id)
        {
            throw new NotImplementedException();
        }
    }
}
