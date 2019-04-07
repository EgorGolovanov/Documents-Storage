using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace DocStorage.Models
{
    public class AccountModel
    {
        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, MinimumLength = 2)]
        public virtual string name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, MinimumLength = 2)]
        public virtual string password { get; set; }
    }
}