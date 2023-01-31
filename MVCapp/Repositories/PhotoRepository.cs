﻿using MVCapp.Models;

namespace MVCapp.Repositories
{
    public class PhotoRepository
    {
        private readonly ApplicationContext applicationContext;

        public PhotoRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task AddPhotoAsync(Photos photo)
        {
            await applicationContext.Photos.AddAsync(photo);
            await applicationContext.SaveChangesAsync();
        }
    }
}