using AutoMapper;
using PhotoGO.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Binding
{
    public class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AlbumModel, AlbumDTO>()
                .ForMember(x => x.Pictures, opt => opt.Ignore())
                .ForMember(x => x.UserId, opt => opt.Ignore());

            cfg.CreateMap<AlbumDTO, AlbumModel>()
                .ForMember(dst => dst.Img, map => map.MapFrom(src => src.Pictures.Count > 0? src.Pictures.Last().Img : null));

            cfg.CreateMap<PictureDTO, ImageModel>();
            cfg.CreateMap<UserDTO, UserModel>();
        }
    }
}