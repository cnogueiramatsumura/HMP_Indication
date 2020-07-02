using DataAccess.Context;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T>, IDisposable where T : class
    {
        protected CryptoContext db;
        public RepositoryBase()
        {
            db = new CryptoContext();
        }

        public void Add(T obj)
        {
            db.Set<T>().Add(obj);
            db.BulkSaveChanges();
        }

        public List<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return db.Set<T>().Find(id);
        }

        public void Remove(T obj)
        {
            db.Entry(obj).State = EntityState.Deleted;
            db.BulkSaveChanges();
        }

        public void Remove(int id)
        {
            T obj = GetById(id);
            db.Entry(obj).State = EntityState.Deleted;
            db.BulkSaveChanges();
        }

        public void Update(T obj)
        {
            db.Entry(obj).State = EntityState.Modified;
            db.BulkSaveChanges();
        }        

        public void AddCollection(T obj, string NavgationPropertyName)
        {
            db.Entry(obj).Collection(NavgationPropertyName).Load();
        }

        public void AddReference(T obj, string NavgationPropertyName)
        {
            db.Entry(obj).Reference(NavgationPropertyName).Load();
        }

        public void Attach(T obj)
        {
            db.Set<T>().Attach(obj);
        }

        public void Detach(T obj)
        {
            db.Entry(obj).State = EntityState.Detached;
        }

        public EntityState GetState(T obj)
        {
            return db.Entry(obj).State;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
