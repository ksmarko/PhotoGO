using System;
using PhotoGO.BLL.DTO;

namespace PhotoGO.BLL.Interfaces
{
    /// <summary>
    /// Service for work with albums
    /// </summary>
    public interface IAlbumService : IDisposable
    {
        /// <summary>
        /// Search album in database by id
        /// </summary>
        /// <param name="id">Album id</param>
        /// <returns>Returns album if it exist or null if doesn't</returns>
        AlbumDTO GetAlbumById(int id);

        /// <summary>
        /// Creates new album
        /// </summary>
        /// <param name="item">Album</param>
        /// <returns>True is operation success or false if don't</returns>
        bool AddAlbum(AlbumDTO item);

        /// <summary>
        /// Removes album and its images from database
        /// </summary>
        /// <param name="id">Album id</param>
        /// <returns>True is operation success or false if don't</returns>
        bool RemoveAlbum(int id);

        /// <summary>
        /// Shanges album name and/or description
        /// </summary>
        /// <param name="item">Album with new data</param>
        /// <returns>True is operation success or false if don't</returns>
        bool EditAlbum(AlbumDTO item);
    }
}
