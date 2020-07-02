using System.Collections.Generic;
using System.Data.Entity;

namespace DataAccess.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        void Add(T obj);
        List<T> GetAll();
        T GetById(int id);
        void Remove(T obj);
        void Remove(int id);
        void Update(T obj);
        void AddCollection(T obj, string NavgationPropertyName);
        void AddReference(T obj, string NavgationPropertyName);
        void Attach(T obj);
        void Detach(T obj);
        EntityState GetState(T obj);

        void Dispose();
    }
}
