using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Data;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;

namespace RealEstateAPI.Repository
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly ApiDbContext _context;

        public ImagesRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddImages(IFormFile image, Listings listing)
        {

            using (var im = new MemoryStream())
            {
                image.CopyTo(im);
                var fileBytes = im.ToArray();
                Images imageToAdd = new Images()
                {
                    Image = fileBytes,
                    Listing = listing
                };

                await _context.AddAsync(imageToAdd);
                return await Save();
            }
        }

        public async Task<bool> DeleteImage(Images image)
        {
            _context.Remove(image);
            return await Save();

        }

        public async Task<Images> GetImage(int id)
        {
            return await _context.Images.Where(x => x.Id == id && x.Listing.IsListed).Include(x => x.Listing).FirstOrDefaultAsync();
        }

        public Task<ICollection<Images>> GetListingImages(int listingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ImageExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var save = await  _context.SaveChangesAsync();
            return save > 0;
        }

        public Task<bool> UpdateImage(Images image)
        {
            throw new NotImplementedException();
        }
    }
}
