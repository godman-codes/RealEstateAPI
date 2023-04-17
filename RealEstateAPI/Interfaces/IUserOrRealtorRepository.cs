using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IUserOrRealtorRepository
    {
        Task<UsersOrRealtors> GetUserOrRelator(int id);
        Task<bool> Save();
        Task<bool> DeleteUser(UsersOrRealtors usersOrRealtors);
        Task<bool> UserOrRelatorExist(int id);
        ICollection<UsersOrRealtors> GetUsersOrRealtors();
        Task<ICollection<Listings>> GetRealtorUserListings(string userid);
    }
}
