using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class PictureRepository : IRepository<Picture>
    {
        private DataContext db;

        public PictureRepository(DataContext context)
        {
            this.db = context;
        }

        public IEnumerable<Picture> GetAll()
        {
            return db.Pictures;
        }

        public Picture Get(int id)
        {
            return db.Pictures.Find(id);
        }

        public void Create(Picture picture)
        {
            db.Pictures.Add(picture);
        }

        public void Update(Picture picture)
        {
            db.Entry(picture).State = EntityState.Modified;
        }

        public IEnumerable<Picture> Find(Func<Picture, Boolean> predicate)
        {
            return db.Pictures.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture != null)
                db.Pictures.Remove(picture);
        }
    }
}
