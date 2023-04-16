using RealEstateAPI.Model;

namespace RealEstateAPI.Interfaces
{
    public interface IImagesRepository
    {
        bool AddImages(Images image);
        ICollection<Images> GetListingImages(int listingId);
        bool DeleteImage(Images image);
        bool UpdateImage(Images image);
        Images GetImage(int id);
        bool ImageExists(int id);
        bool Save();
    }
}
