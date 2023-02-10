using Microsoft.EntityFrameworkCore;
using MVCapp.Models;
using System.Drawing;
using System.Drawing.Imaging;
using File = MVCapp.Models.File;

namespace MVCapp.Repositories
{
    public class FileRepository
    {
        private readonly ApplicationContext applicationContext;

        public FileRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task AddFileAsync(File file)
        {
            applicationContext.Files.Add(file);
            await applicationContext.SaveChangesAsync();
        }

        public async Task<File?> GetFileAsync(string path)
        {
            File file = await applicationContext.Files.FirstOrDefaultAsync(f => f.Path == path);
            return file;
        }
        public async Task<bool> FileInDataBase(string path)
        {
            File file = await applicationContext.Files.FirstOrDefaultAsync(f => f.Path == path);
            return false;
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
        // Get content type
        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        public string GetContentKey(string path)
        {
            var types = GetMimeTypes();
            var myKey = types.FirstOrDefault(x => x.Key == Path.GetExtension(path).ToLowerInvariant()).Key;
            return myKey;
        }

        // Get mime types
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
