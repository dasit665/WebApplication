using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Areas.UsersAdmining.VMModels
{
    public class VMUserToAuthentication
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

        public List<string> Roles { get; set; } = new List<string>();
    }
}
