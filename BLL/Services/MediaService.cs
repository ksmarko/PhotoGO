using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MediaService : IMediaService
    {
        IUnitOfWork Database { get; set; }

        public MediaService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<AlbumDTO> GetAlbums()
        {
            return Mapper.Map<IEnumerable<Album>, List<AlbumDTO>>(Database.Albums.GetAll());
        }

        public void AddAlbum(AlbumDTO album)
        {
            throw new NotImplementedException();
        }

        public void RemoveAlbum(AlbumDTO album)
        {
            throw new NotImplementedException();
        }

        public void EditAlbum(AlbumDTO album)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PictureDTO> GetImages(AlbumDTO album)
        {
            throw new NotImplementedException();
        }

        public void AddImage(PictureDTO image, AlbumDTO album)
        {
            throw new NotImplementedException();
        }

        public void RemoveImage(PictureDTO image)
        {
            throw new NotImplementedException();
        }

        public void AddTags(params string[] tags)
        {
            throw new NotImplementedException();
        }

        public void LikeImage(PictureDTO picture)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
