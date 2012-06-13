using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace MongoDemo.Models
{
    public abstract class MongoModelBase
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        #region Server

        protected static string ConnectionString
        {
            get { return "mongodb://localhost/"; }
        }
        protected static MongoServer Server
        {
            get { return MongoServer.Create(ConnectionString); }
        }
        protected static MongoDatabase TestDatabase
        {
            get { return Server.GetDatabase("test"); }
        }

        #endregion
    }

    public abstract class MongoModelBase<T> : MongoModelBase
    {
        #region Collection

        public static IQueryable<T> AsQueryable
        {
            get { return Collection.AsQueryable(); }
        }
        public static IList<T> All()
        {
            return AsQueryable.ToList();
        }
        public static T Find(ObjectId id)
        {
            return Collection.FindOneById(id);
        }
        public static void EnsureIndex(string propertyName)
        {
            Collection.EnsureIndex(IndexKeys.Ascending(propertyName));
        }

        #endregion


        #region Entity

        public void Save()
        {
            if (Id == ObjectId.Empty)
                Insert();
            else
                Update();
        }
        public void Delete()
        {
            Collection.Remove(Query.EQ("_id", Id));
        }

        #endregion


        #region Private

        private static MongoCollection<T> Collection
        {
            get { return TestDatabase.GetCollection<T>(typeof(T).Name, SafeMode.False); }
        }
        private void Insert()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Collection.Insert(this);
        }
        private void Update()
        {
            UpdatedAt = DateTime.UtcNow;
            Collection.Save(this);
        }

        #endregion
    }
}
