using CourseWork.Data;
using CourseWork.Infrastructure.Interfaces;
using CourseWork.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly ApplicationContext _context;

        public CartController(ICartRepository cartRepository, UserManager<User> userManager, ApplicationContext context)
        {
            _context = context;
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public IActionResult AddItemInCart(int id)
        {
            Item item = _context.Item.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemInCart(int id, string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            _cartRepository.AddItemInCart(id, user.Id);
            return View();
        }
    }
}
