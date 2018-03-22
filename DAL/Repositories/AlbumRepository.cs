using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class AlbumRepository : IRepository<Album>
    {
        private DataContext db;

        public AlbumRepository(DataContext context)
        {
            this.db = context;
        }

        public IEnumerable<Album> GetAll()
        {
            return db.Albums;
        }

        public Album Get(int id)
        {
            return db.Albums.Find(id);
        }

        public void Create(Album album)
        {
            db.Albums.Add(album);
        }

        public void Update(Album album)
        {
            db.Entry(album).State = EntityState.Modified;
        }

        public IEnumerable<Album> Find(Func<Album, Boolean> predicate)
        {
            return db.Albums.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Album album = db.Albums.Find(id);
            if (album != null)
                db.Albums.Remove(album);
        }
    }
}
