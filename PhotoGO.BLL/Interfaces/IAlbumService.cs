using System;
using PhotoGO.BLL.DTO;
using PhotoGO.BLL.Exceptions;

namespace PhotoGO.BLL.Interfaces
{
    /// <summary>
    /// Service for work with albums
    /// </summary>
    public interface IAlbumService : IDisposable
    {
        /// <summary>
        /// <summary>
        /// Search album in database by id
        /// </summary>
        /// <param name="id">Album id</param>
        /// <exception cref="TargetNotFoundException">When album not found</exception>
        AlbumDTO GetAlbumById(int id);

        /// <summary>
        /// Creates new album
        /// </summary>
        /// <param name="item">Album</param>
        /// <exception cref="ArgumentNullException">When input item is null</exception>
        /// <exception cref="UserNotFoundException">When user not found</exception>
        void AddAlbum(AlbumDTO item);

        /// <summary>
        /// Removes album and its images from database
        /// </summary>
        /// <param name="id">Album id</param>
        /// <exception cref="TargetNotFoundException">When album not found</exception>
        void RemoveAlbum(int id);

        /// <summary>
        /// Shanges album name and/or description
        /// </summary>
        /// <param name="item">Album with new data</param>
        /// <exception cref="TargetNotFoundException">When album not found</exception>
        void EditAlbum(AlbumDTO item);
    }
}
