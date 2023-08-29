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
                if (item.Id == null)
                {
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
        [HttpDelete]
        public ActionResult Delete([FromBody] STORAGE item)
        {
            try
            {
                if (item.Id == null) return BadRequest();
                _service.Delete(item.Id);
                return Ok();
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
