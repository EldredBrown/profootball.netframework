using System.Collections.Generic;
using System.Threading.Tasks;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Repositories
{
    public interface IRepository<T> where T: class
    {
        T AddEntity(ProFootballEntities dbContext, T entity);
        IEnumerable<T> AddEntities(ProFootballEntities dbContext, IEnumerable<T> entities);
        T CreateEntity(ProFootballEntities dbContext);
        void EditEntity(ProFootballEntities dbContext, T entity);
        T FindEntity(ProFootballEntities dbContext, params object[] args);
        Task<T> FindEntityAsync(ProFootballEntities dbContext, params object[] args);
        IEnumerable<T> GetEntities(ProFootballEntities dbContext);
        Task<IEnumerable<T>> GetEntitiesAsync(ProFootballEntities dbContext);
        void LoadEntities(ProFootballEntities dbContext);
        Task LoadEntitiesAsync(ProFootballEntities dbContext);
        T RemoveEntity(ProFootballEntities dbContext, T entity);
        IEnumerable<T> RemoveEntities(ProFootballEntities dbContext, IEnumerable<T> entities);
    }
}
