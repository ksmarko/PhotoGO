﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class RegisterModel
    {
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter email in format qqq@qq.com")]
        [Required(ErrorMessage = "Please, enter email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, enter password")]
        [Range(6, 30, ErrorMessage = "Password must be greater than 6 and less than 30 symbols")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please, confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are different")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please, enter your name")]
        public string Name { get; set; }
    }
}