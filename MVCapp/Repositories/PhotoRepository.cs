using Microsoft.EntityFrameworkCore;
using MVCapp.Models;
using System;

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
    }
}
