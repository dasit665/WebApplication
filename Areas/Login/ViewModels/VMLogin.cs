using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Areas.Login.ViewModels
{
    public class VMLogin
    {
        [Required(ErrorMessage ="Не введен e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="e-mail:")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Не введен пароль")]
        [DataType(DataType.Password)]
        [RegularExpression(pattern:@"[A-Za-z_0-9]{6,32}", ErrorMessage ="Латинские печатные символы, подчеркивания и цифры, от 6 до 32 символов")]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }
    }
}
