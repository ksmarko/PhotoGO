using BLL.DTO;
using DAL.Entities;

using AutoMapper;

namespace BLL.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                //create map for user
                cfg.CreateMap<Album, AlbumDTO>();
                cfg.CreateMap<Picture, PictureDTO>();
            });
        }
    }
}
