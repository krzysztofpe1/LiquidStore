using Microsoft.AspNetCore.Mvc;
using StoreServer.DatabaseModels;
using StoreServer.Services;

namespace StoreServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly StorageDbService _service;
        public StorageController(StorageDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<STORAGE>> Get([FromQuery] int? id)
        {
            if (id == null) return Ok(_service.Get());
            var item = _service.Get(id.Value);
            if (item != null) return Ok(new List<STORAGE> { item });
            return NotFound();
        }
    }
}
