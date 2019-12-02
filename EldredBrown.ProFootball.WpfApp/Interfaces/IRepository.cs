using System.Collections.Generic;

namespace EldredBrown.ProFootball.WpfApp.Interfaces
{
    public interface IRepository<T> where T: class
    {
        T AddEntity(T entity);
        IEnumerable<T> AddEntities(IEnumerable<T> entities);
        T CreateEntity();
        void EditEntity(T entity);
        T FindEntity(params object[] args);
        IEnumerable<T> GetEntities();
        T RemoveEntity(T entity);
        IEnumerable<T> RemoveEntities(IEnumerable<T> entities);
    }
}
