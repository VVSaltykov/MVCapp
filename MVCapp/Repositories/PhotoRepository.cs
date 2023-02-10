using Microsoft.EntityFrameworkCore;
using MVCapp.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace MVCapp.Repositories
{
    public class PhotoRepository
    {
        private readonly ApplicationContext applicationContext;

        public PhotoRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task AddPhotoAsync(Photo photo)
        {
            applicationContext.Photos.Add(photo);
            await applicationContext.SaveChangesAsync();
        }

        public async Task<Photo?> GetFileAsync(string path)
        {
            Photo photo = await applicationContext.Photos.FirstOrDefaultAsync(p => p.Path == path);
            return photo;
        }
        public async Task<bool> FileInDataBase(string path)
        {
            Photo photo = await applicationContext.Photos.FirstOrDefaultAsync(p => p.Path == path);
            return true;
        }
        public async Task<byte[]> ResizeImage(Image image)
        {
            if (image.Width > 1920 && image.Height > 1080)
            {
                var resized = new Bitmap(image, new Size(1920, 1080));
                using var imageStream = new MemoryStream();
                resized.Save(imageStream, ImageFormat.Jpeg);
                var imageBytes = imageStream.ToArray();
                return imageBytes;
            }
            else
            {
                var resized = new Bitmap(image, new Size(image.Width, image.Height));
                using var imageStream = new MemoryStream();
                resized.Save(imageStream, ImageFormat.Jpeg);
                var imageBytes = imageStream.ToArray();
                return imageBytes;
            }
        }
    }
}
