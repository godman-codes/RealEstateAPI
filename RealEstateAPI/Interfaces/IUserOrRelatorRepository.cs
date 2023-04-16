using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IUserOrRelatorRepository
    {
        Task<UsersOrRealtors> GetUserOrRelator(int id);
        //bool IsNormalUser(int id);
        //bool IsRealtorUser(int id);

        Task<bool> Save();
        Task<bool> CreateUser(UsersOrRealtors userOrRealtor);
        Task<bool> UpdateCountry(UsersOrRealtors usersOrRealtor);
        Task<bool> DeleteUser(UsersOrRealtors usersOrRealtors);
        Task<bool> UserOrRelatorExist(int id);
        ICollection<UsersOrRealtors> GetUsersOrRealtors();
    }
}
