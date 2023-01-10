using Microsoft.EntityFrameworkCore;
using MVCapp.Interfaces;
using MVCapp.Models;

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
            User user = await appContext.Users.FirstOrDefaultAsync(u => u.Login == loginModel.Login && u.Password == loginModel.Password);
            appContext.Users.ToList();
            return user;
        }
        public async Task<bool> UserIsInDatabase(RegisterModel registerModel)
        {
            User user = await appContext.Users.FirstOrDefaultAsync(u => u.Login == registerModel.Login);
            if (user == null)
            {
                return false;
            }
            appContext.Users.ToList();
            return true;
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await appContext.FindAsync<User>(userId);
            return user;
        }
    }
}
