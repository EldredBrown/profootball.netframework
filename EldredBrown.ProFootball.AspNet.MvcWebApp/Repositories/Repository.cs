using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EldredBrown.ProFootballApplicationWeb.Models;

namespace EldredBrown.ProFootballApplicationWeb.Repositories
{
    public interface IRepository
    {
        Model AddEntity(ProFootballDbEntities dbContext, Model entity);
        IEnumerable<Model> AddEntities(ProFootballDbEntities dbContext, IEnumerable<Model> entities);
        Model CreateEntity(ProFootballDbEntities dbContext);
        void EditEntity(ProFootballDbEntities dbContext, Model entity);
        Model FindEntity(ProFootballDbEntities dbContext, params object[] keyValues);
        Task<Model> FindEntityAsync(ProFootballDbEntities dbContext, params object[] keyValues);
        IEnumerable<Model> GetEntities(ProFootballDbEntities dbContext);
        Task<IEnumerable<Model>> GetEntitiesAsync(ProFootballDbEntities dbContext);
        void LoadEntities(ProFootballDbEntities dbContext);
        Task LoadEntitiesAsync(ProFootballDbEntities dbContext);
        Model RemoveEntity(ProFootballDbEntities dbContext, Model entity);
        IEnumerable<Model> RemoveEntities(ProFootballDbEntities dbContext, IEnumerable<Model> entities);
    }

    public class Repository : IRepository
    {
        public Model AddEntity(ProFootballDbEntities dbContext, Model entity)
        {
            return dbContext.Entities.Add(entity);
        }

        public IEnumerable<Model> AddEntities(ProFootballDbEntities dbContext, IEnumerable<Model> entities)
        {
            return dbContext.Entities.AddRange(entities);
        }

        public Model CreateEntity(ProFootballDbEntities dbContext)
        {
            return dbContext.Entities.Create();
        }

        public void EditEntity(ProFootballDbEntities dbContext, Model entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public Model FindEntity(ProFootballDbEntities dbContext, params object[] keyValues)
        {
            return dbContext.Entities.Find(keyValues);
        }

        public async Task<Model> FindEntityAsync(ProFootballDbEntities dbContext, params object[] keyValues)
        {
            return await dbContext.Entities.FindAsync(keyValues);
        }

        public IEnumerable<Model> GetEntities(ProFootballDbEntities dbContext)
        {
            return dbContext.Entities;
        }

        public async Task<IEnumerable<Model>> GetEntitiesAsync(ProFootballDbEntities dbContext)
        {
            return await dbContext.Entities.ToListAsync();
        }

        public void LoadEntities(ProFootballDbEntities dbContext)
        {
            dbContext.Entities.Load();
        }

        public async Task LoadEntitiesAsync(ProFootballDbEntities dbContext)
        {
            await dbContext.Entities.LoadAsync();
        }

        public Model RemoveEntity(ProFootballDbEntities dbContext, Model entity)
        {
            return dbContext.Entities.Remove(entity);
        }

        public IEnumerable<Model> RemoveEntities(ProFootballDbEntities dbContext, IEnumerable<Model> entities)
        {
            return dbContext.Entities.RemoveRange(entities);
        }
    }
}
