﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PictureDTO> Pictures { get; set; }

        public TagDTO()
        {
            Pictures = new List<PictureDTO>();
        }
    }
}
