using System.Collections.Generic;
using System.Data.Entity;
using EldredBrown.ProFootballApplicationWPF.Models;

namespace EldredBrown.ProFootballApplicationWPF.Repositories
{
    public interface IRepository
    {
        Model AddEntity(Model entity);
        IEnumerable<Model> AddEntities(IEnumerable<Model> entities);
        Model CreateEntity();
        void EditEntity(Model entity);
        Model FindEntityByKeys(params object[] keyValues);
        IEnumerable<Model> GetEntities();
        void LoadEntities();
        Model RemoveEntity(Model entity);
        IEnumerable<Model> RemoveEntities(IEnumerable<Model> entities);
    }

    public class Repository : IRepository
    {
        public Model AddEntity(Model entity)
        {
            return DataAccess.DbContext.Models.Add(entity);
        }

        public IEnumerable<Model> AddEntities(IEnumerable<Model> entities)
        {
            return DataAccess.DbContext.Models.AddRange(entities);
        }

        public Model CreateEntity()
        {
            return DataAccess.DbContext.Models.Create();
        }

        public void EditEntity(Model entity)
        {
            DataAccess.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public Model FindEntityByKeys(params object[] keyValues)
        {
            return DataAccess.DbContext.Models.Find(keyValues);
        }

        public IEnumerable<Model> GetEntities()
        {
            return DataAccess.DbContext.Models;
        }

        public void LoadEntities()
        {
            DataAccess.DbContext.Models.Load();
        }

        public Model RemoveEntity(Model entity)
        {
            return DataAccess.DbContext.Models.Remove(entity);
        }

        public IEnumerable<Model> RemoveEntities(IEnumerable<Model> entities)
        {
            return DataAccess.DbContext.Models.RemoveRange(entities);
        }
    }
}
