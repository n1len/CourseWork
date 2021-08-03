using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
