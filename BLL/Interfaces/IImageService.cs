﻿using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IImageService
    {
        IEnumerable<PictureDTO> GetImages(int albumId);
        void AddImage(PictureDTO image, int albumId);
        void RemoveImage(int id);
        void AddTags(int imgId, params string[] tags);
        void LikeImage(int id, string userId);
        IEnumerable<PictureDTO> GetFavouritesForUser(string id);
        bool IsLikedBy(string id, int imgId);
        PictureDTO GetImageById(int id);
        IEnumerable<PictureDTO> SearchImages(params string[] tags);
        IEnumerable<PictureDTO> GetImages();

        void Dispose();
    }
}
