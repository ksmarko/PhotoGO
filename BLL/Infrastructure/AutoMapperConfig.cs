using BLL.DTO;
using DAL.Entities;
using AutoMapper;
using System.Collections.Generic;

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
