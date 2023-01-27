﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using MVCapp;
using MVCapp.Interfaces;
using MVCapp.Models;
using System.Reflection.Metadata.Ecma335;

namespace ProMVC.Repositories
{
    public class UserRepository: IUser
    {
        private readonly ApplicationContext appContext;

        public UserRepository(ApplicationContext appContext)
        {
            this.appContext = appContext;
        }
        public async Task AddNewUser(User user)
        {
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync();
        }
        public async Task<User> GetUserByLoginModelAsync(LoginModel loginModel)
        {
            byte[] salt = { 1, 2, 3 };
            loginModel.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: loginModel.Password!,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));
            User user = await appContext.Users.FirstOrDefaultAsync(u => u.Login == loginModel.Login && u.Password == loginModel.Password);
            return user;
        }
        public async Task<bool> UserIsInDatabase(RegisterModel registerModel)
        {
            User user = await appContext.Users.FirstOrDefaultAsync(u => u.Login == registerModel.Login);
            if (user == null)
            {
                return false;
            }
            return true;
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await appContext.FindAsync<User>(userId);
            return user;
        }
    }
}
