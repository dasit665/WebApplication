using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class VMAddUser
    {
        [Required(ErrorMessage = "Не введен e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "e-mail:")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Не введено имя")]
        [DataType(DataType.Text)]
        [RegularExpression(pattern: @"[А-Я]{1,1}[а-я]{1,31}", ErrorMessage = "Кирилические символы начиная с заглавной буквы длиной 32 символа")]
        [Display(Name = "Имя пользователя:")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Не введено имя")]
        [DataType(DataType.Text)]
        [RegularExpression(pattern: @"[А-Я]{1,1}[а-я]{1,31}", ErrorMessage = "Кирилические символы начиная с заглавной буквы длиной 32 символа")]
        [Display(Name = "Фамилия пользователя:")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Не введен пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        [RegularExpression(pattern:@"[A-Za-z_0-9]{6,}", ErrorMessage ="Не соответствует шаблону латинские печатные символы, цифры и подчеркивания, длиной от 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не введено подтверждение пароля")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля:")]
        [Compare(otherProperty:"Password", ErrorMessage ="Пароли не одиноковы")]
        public string ConfirmPassword { get; set; }
    }
}
