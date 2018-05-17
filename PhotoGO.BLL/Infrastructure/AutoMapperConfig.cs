using PhotoGO.BLL.DTO;
using PhotoGO.DAL.Entities;
using AutoMapper;

namespace PhotoGO.BLL.Infrastructure
{
    /// <summary>
    /// AutoMapper configuration
    /// </summary>
    /// <remarks>Mapps entities between DAL and BLL</remarks>
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
