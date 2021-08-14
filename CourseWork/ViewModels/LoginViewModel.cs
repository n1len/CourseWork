using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace CourseWork.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
