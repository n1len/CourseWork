using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string Password { get; set; }
    }
}
