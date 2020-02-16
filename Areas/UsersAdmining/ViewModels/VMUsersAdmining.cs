using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Areas.UsersAdmining.VMModels
{
    public class VMUsersAdmining
    {
        public int ID { get; set; } = default;

        [Required(ErrorMessage ="Не введен e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="e-mail")]
        public string EMail { get; set; } = default;

        [Required(ErrorMessage = "Не введено имя пользователя")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя пользователя")]
        public string FName { get; set; } = default;

        [Required(ErrorMessage = "Не введена фамилия пользователя")]
        [DataType(DataType.Text)]
        [Display(Name = "Фимилия пользователя")]
        public string LName { get; set; } = default;

        public Dictionary<string, bool> Roles { get; set; } = new Dictionary<string, bool>();
    }
}
