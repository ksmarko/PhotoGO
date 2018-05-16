using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoGO.WEB.Models
{
    public class AddImageModel
    {
        //TODO: add size and extension validation
        [Required(ErrorMessage = "Please select file (-s)")]
        public HttpPostedFileBase[] Files { get; set; }

        [RegularExpression("(\\w+\\s?)+", ErrorMessage = "Please input tags in format tag1 tag2 tag3")]
        public string Tags { get; set; }
    }
}