using BLL.DTO;
using PhotoGO.DAL.Entities;
using AutoMapper;

namespace BLL.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Album, AlbumDTO>();
            cfg.CreateMap<Picture, PictureDTO>();
            cfg.CreateMap<User, UserDTO>();
            cfg.CreateMap<Tag, TagDTO>();
        }
    }
}
