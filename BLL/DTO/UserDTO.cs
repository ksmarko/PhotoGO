﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public IEnumerable<UserDTO> Followings { get; set; }
        public IEnumerable<AlbumDTO> Albums { get; set; }
        public IEnumerable<PictureDTO> Pictures { get; set; }
    }
}
