using AmbevOrder.ProcessOrder.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmbevOrder.ProcessOrder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetAllAsync();

            if (orders == null || !orders.Any())
            {
                return NoContent();
            }

            return Ok(orders);
        }
    }
}
