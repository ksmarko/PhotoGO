using System.Linq;
using AutoMapper;
using PhotoGO.BLL.DTO;
using PhotoGO.WEB.Models;

namespace PhotoGO.WEB.Binding
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

            cfg.CreateMap<PictureDTO, ImageModel>()
                .ForMember(dst => dst.Likes, map => map.MapFrom(src => src.FavouritedBy.Count))
                .ForMember(dst => dst.Tags, map => map.MapFrom(src => src.Tags.Select(x => x.Name).ToList()));

            cfg.CreateMap<UserDTO, UserModel>();
        }
    }
}