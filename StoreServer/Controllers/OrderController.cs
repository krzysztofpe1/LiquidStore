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
        private readonly OrderDbService _orderService;
        private readonly StorageDbService _storageService;
        public OrderController(OrderDbService orderService, StorageDbService storageService)
        {
            _orderService = orderService;
            _storageService = storageService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ORDER>> Get([FromQuery] int? id)
        {
            if (id == null) return Ok(_orderService.Get());
            var item = _orderService.Get(id.Value);
            if (item != null) return Ok(new List<ORDER> { item });
            return NotFound();
        }
        [HttpGet]
        [Route("details")]
        public ActionResult<ORDERDETAILS> GetOrderDetailsItem([FromQuery] int? id)
        {
            if (id == null) return BadRequest();
            var item = _orderService.GetOrderDetailsItem(id.Value);
            if (item != null) return Ok(item);
            return NotFound();
        }
        [HttpPut]
        public ActionResult<ORDER> Save([FromBody] ORDER item)
        {
            try
            {
                if (item.Id == null)
                {
                    var newItem = _orderService.Insert(item);
                    return Created($"/order?id={newItem.Id}", JsonConvert.SerializeObject(newItem));
                }
                else
                {
                    _orderService.Update(item);
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
        [HttpPut]
        [Route("details")]
        public ActionResult<ORDERDETAILS> Save([FromBody] ORDERDETAILS item)
        {
            try
            {
                if (item.Id == null)
                {
                    if (item.Status == 3 || item.Status == 4)
                    {
                        item.DeliveredDate = DateTime.Now.ToString();
                    }
                    if (item.Status == 4)
                    {
                        item.SettledDate = DateTime.Now.ToString();
                    }
                    var newItem = _orderService.Insert(item);
                    SubtractMaterialFromStorage(newItem);
                    return Created($"/order/details?id={newItem.Id}", JsonConvert.SerializeObject(newItem));
                }
                else
                {
                    var oldItem = _orderService.GetOrderDetailsItem(item.Id.Value);
                    if (item.Brand != oldItem.Brand || item.Name != oldItem.Name)
                    {
                        AddMaterialToStorage(oldItem);
                        SubtractMaterialFromStorage(item);
                    }
                    else if (item.Volume != oldItem.Volume)
                    {
                        var tempItem = new ORDERDETAILS()
                        {
                            Volume = item.Volume - oldItem.Volume,
                            Brand = oldItem.Brand,
                            Name = oldItem.Name
                        };
                        SubtractMaterialFromStorage(tempItem);
                    }
                    var dbItem = _orderService.GetOrderDetailsItem(item.Id.Value);
                    if ((dbItem.DeliveredDate == null || dbItem.DeliveredDate == string.Empty) && (item.Status == 3 || item.Status == 4))
                    {
                        item.DeliveredDate = DateTime.Now.ToString();
                    }
                    else if (dbItem.DeliveredDate != null && dbItem.DeliveredDate != string.Empty)
                    {
                        item.DeliveredDate = dbItem.DeliveredDate;
                    }
                    if ((dbItem.SettledDate == null || dbItem.SettledDate == string.Empty) && item.Status == 4)
                    {
                        item.SettledDate = DateTime.Now.ToString();
                    }
                    else if (item.SettledDate != null && item.SettledDate != string.Empty)
                    {
                        item.SettledDate = dbItem.SettledDate;
                    }
                    _orderService.Update(item);
                    return Created($"/order/details?id={item.Id}", JsonConvert.SerializeObject(item));
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
        public ActionResult Delete([FromBody] ORDER item)
        {
            try
            {
                if (item.Id == null) return BadRequest();
                if (item.Details != null)
                {
                    foreach (var detail in item.Details)
                    {
                        Delete(detail);
                    }
                }
                _orderService.DeleteOrder(item.Id);
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
        [HttpDelete]
        [Route("details")]
        public ActionResult Delete([FromBody] ORDERDETAILS item)
        {
            try
            {
                if (item.Id == null) return BadRequest();
                AddMaterialToStorage(item);
                _orderService.DeleteOrderDetail(item.Id);
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
        private bool SubtractMaterialFromStorage(ORDERDETAILS orderDetails)
        {
            var storageItem = _storageService.Get().FirstOrDefault(s => s.Brand == orderDetails.Brand && s.Name == orderDetails.Name);
            if (storageItem == null)
                return false;
            storageItem.Remaining -= orderDetails.Volume / 10;
            if (storageItem.Remaining < 0)
                return false;
            _storageService.Update(storageItem);
            return true;
        }
        private bool AddMaterialToStorage(ORDERDETAILS orderDetails)
        {
            var storageItem = _storageService.Get().FirstOrDefault(s => s.Brand == orderDetails.Brand && s.Name == orderDetails.Name);
            if (storageItem == null)
                return false;
            storageItem.Remaining += orderDetails.Volume / 10;
            _storageService.Update(storageItem);
            return true;
        }
    }
}
