using PhotoGO.BLL.DTO;

namespace PhotoGO.BLL.Interfaces
{
    public interface IAlbumService
    {
        /// <summary>
        /// Search album in database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns album if it exist or null if doesn't</returns>
        AlbumDTO GetAlbumById(int id);

        /// <summary>
        /// Creates new album
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True is operation success or false if don't</returns>
        bool AddAlbum(AlbumDTO item);

        /// <summary>
        /// Removes album and its images from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True is operation success or false if don't</returns>
        bool RemoveAlbum(int id);

        /// <summary>
        /// Shanges album name and/or description
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True is operation success or false if don't</returns>
        bool EditAlbum(AlbumDTO item);

        void Dispose();
    }
}
