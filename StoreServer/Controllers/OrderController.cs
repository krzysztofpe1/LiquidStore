using Microsoft.AspNetCore.Mvc;
using StoreServer.DatabaseModels;
using StoreServer.Services;

namespace StoreServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderDbService _service;
        public OrderController(OrderDbService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ORDER>> Get([FromQuery] int? id)
        {
            if (id == null) return Ok(_service.Get());
            var item = _service.Get(id.Value);
            if (item != null) return Ok(new List<ORDER> { item });
            return NotFound();
        }

    }
}
