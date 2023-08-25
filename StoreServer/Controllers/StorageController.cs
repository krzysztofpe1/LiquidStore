using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreServer.DatabaseModels;
using StoreServer.Services;
using StoreServer.Utils;

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
        [HttpPut]
        public ActionResult Save([FromBody] STORAGE item)
        {
            try
            {
                if (item.Id.Value == 0)
                {
                    item.Id = null;
                    if (item.Volume == null) item.Volume = 0;
                    if (item.Cost == null) item.Cost = 0;
                    if (item.Remaining == null) item.Remaining = 0;
                    var newItem = _service.Insert(item);
                    return Created($"/storage?id={newItem.Id}", JsonConvert.SerializeObject(newItem));
                }
                else
                {
                    _service.Update(item);
                    return Created($"/storage?id={item.Id}", JsonConvert.SerializeObject(item));
                }
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
