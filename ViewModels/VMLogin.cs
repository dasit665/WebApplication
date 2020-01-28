using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class VMLogin
    {
        [Required(ErrorMessage ="Не введен e-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage ="e-mail не валидный")]
        public string EMail { get; set; }

        [Required(ErrorMessage ="Не введен пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
