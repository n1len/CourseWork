using CourseWork.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
