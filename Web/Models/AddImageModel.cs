﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class AddImageModel
    {
        //TODO: add size and extension validation
        [Required(ErrorMessage = "Please select file (-s)")]
        public HttpPostedFileBase[] files { get; set; }
        
        [Required(ErrorMessage = "Please input tags")]
        public string tags { get; set; }
    }
}