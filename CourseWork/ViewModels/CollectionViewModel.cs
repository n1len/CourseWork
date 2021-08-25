using CourseWork.Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace CourseWork.ViewModels
{
    public class CollectionViewModel
    {
        public CustomCollection CustomCollection { get; set; }
        public string UserName { get; set; }
        public IFormFile CollectionImage { get; set; }
    }
}
