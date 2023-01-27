using Microsoft.EntityFrameworkCore;
using MVCapp.Interfaces;
using System.Runtime.InteropServices;

namespace MVCapp.Repositories
{
    public class BaseRepository<TEntity> where TEntity : class, IEntity
    {
        /*private readonly ApplicationContext applicationContext;
        private readonly DbSet<TEntity> entities;

        public BaseRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
            entities = applicationContext.Set<TEntity>();
        }

        public virtual async Task AddToDatabase(TEntity entity)
        {
            entities.Add(entity);
            await applicationContext.SaveChangesAsync();
        }*/
    }
}
