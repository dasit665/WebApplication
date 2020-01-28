using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class VMAddUser
    {
        [Required(ErrorMessage ="Не введен e-mail")]
        [Display(Name ="e-mail")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Не введено имя")]
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string FName { get; set; }

        [Required(ErrorMessage = "Не введена фамилия")]
        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string LName { get; set; }


        [Required(ErrorMessage = "Не введен пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [RegularExpression("[A-Za-z0-9_]{6,}", ErrorMessage ="Только латинские буквы, цыфры, и подчеркивания 6 символов и более")]
        public string Password { get; set; }

        
        [Display(Name = "Поддвердите пароль")]
        [Compare("Password", ErrorMessage ="Пароли не совподают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
