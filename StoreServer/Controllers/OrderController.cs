using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreServer.DatabaseModels;
using StoreServer.Services;
using StoreServer.Utils;

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
        [HttpPut]
        public ActionResult<ORDER> Save([FromBody]ORDER item)
        {
            try
            {
                if(item.Id == null)
                {
                    var newItem = _service.Insert(item);
                    return Created($"/order?id={newItem.Id}", JsonConvert.SerializeObject(newItem));
                }
                else
                {
                    _service.Update(item);
                    return Created($"/order?id={item.Id}", JsonConvert.SerializeObject(item));
                }
            }
            catch(ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("details")]
        public ActionResult<ORDERDETAILS> Save([FromBody] ORDERDETAILS item)
        {
            try
            {
                if (item.Id == null)
                {
                    var newItem = _service.Insert(item);
                    return Created($"/order?id={newItem.Id}", JsonConvert.SerializeObject(newItem));
                }
                else
                {
                    _service.Update(item);
                    return Created($"/order?id={item.Id}", JsonConvert.SerializeObject(item));
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
        [Route("details")]
        public ActionResult Delete([FromBody] ORDERDETAILS item)
        {
            try
            {
                if (item.Id == null) return BadRequest();
                _service.DeleteOrder(item.Id);
                return Ok();
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
