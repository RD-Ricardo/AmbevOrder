using AmbevOrder.InputOrder.Dtos;
using AmbevOrder.InputOrder.Services;
using Microsoft.AspNetCore.Mvc;

namespace AmbevOrder.InputOrder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderInputController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderCreateDto orderCreateDto, [FromServices] IOrderService orderService)
        {
            await orderService.CreateOrderAsync(orderCreateDto);
            return Ok(new { message = "Order created successfully" });
        }
    }
}
