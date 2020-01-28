using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class VMUserAndRoles
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Не введен адресс e-mail")]
        [Display(Name = "e-mail")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [Required(ErrorMessage ="Не введено имя пользователя")]
        [Display(Name ="Имя пользователя")]
        [DataType(DataType.Text)]
        public string FName { get; set; }

        [Required(ErrorMessage = "Не введена фамилия пользователя")]
        [Display(Name = "Фамилия пользователя")]
        [DataType(DataType.Text)]
        public string LName { get; set; }

        public Dictionary<string, bool> UserRoles { get; set; } = new Dictionary<string, bool>();
    }
}