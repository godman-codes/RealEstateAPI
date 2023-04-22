using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IImagesRepository
    {
        Task<bool> AddImages(IFormFile image, Listings listing);
        Task<ICollection<Images>> GetListingImages(int listingId);
        Task<bool> DeleteImage(Images image);
        Task<bool> UpdateImage(Images image);
        Task<Images> GetImage(int id);
        Task<bool> ImageExists(int id);
        Task<bool> Save();
    }
}
