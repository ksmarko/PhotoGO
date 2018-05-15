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
            cfg.CreateProfile("DALtoBLL", prf =>
            {
                prf.CreateMap<Album, AlbumDTO>();
                prf.CreateMap<Picture, PictureDTO>();
                prf.CreateMap<User, UserDTO>();
                prf.CreateMap<Tag, TagDTO>();
            });
        }
    }
}
