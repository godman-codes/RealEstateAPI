using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Data;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;

namespace RealEstateAPI.Repository
{
    public class UserOrRealtorRepository : IUserOrRelatorRepository
    {
        private readonly ApiDbContext _context;

        public UserOrRealtorRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateUser(UsersOrRealtors userOrRealtor)
        {
            await _context.AddAsync(userOrRealtor);
            return await Save();
        }

        public async Task<bool> DeleteUser(UsersOrRealtors usersOrRealtors)
        {
            throw new NotImplementedException();
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

        public async Task<bool> UpdateCountry(UsersOrRealtors usersOrRealtor)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserOrRelatorExist(int id)
        {
            throw new NotImplementedException();
        }
    }
}
