﻿using Microsoft.AspNetCore.Mvc;
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
            if(item != null) return Ok(item);
            return NotFound();
        }
        [HttpPut]
        public ActionResult<ORDER> Save([FromBody]ORDER item)
        {
            try
            {
                if(item.Id == null)
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
                    var newItem = _orderService.Insert(item);
                    SubtractMaterialFromStorage(newItem);
                    return Created($"/order/details?id={newItem.Id}", JsonConvert.SerializeObject(newItem));
                }
                else
                {
                    SubtractMaterialFromStorage(item);
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
            catch(ApiException ex)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private bool SubtractMaterialFromStorage(ORDERDETAILS orderDetails)
        {
            var storageItem = _storageService.Get().FirstOrDefault(s=>s.Brand == orderDetails.Brand && s.Name == orderDetails.Name);
            if (storageItem == null)
                return false;
            storageItem.Remaining -= orderDetails.Volume/10;
            if(storageItem.Remaining < 0)
                return false;
            _storageService.Update(storageItem);
            return true;
        }
        private bool AddMaterialToStorage(ORDERDETAILS orderDetails)
        {
            var storageItem = _storageService.Get().FirstOrDefault(s=>s.Brand == orderDetails.Brand && s.Name == orderDetails.Name);
            if(storageItem == null)
                return false;
            storageItem.Remaining += orderDetails.Volume/10;
            _storageService.Update(storageItem);
            return true;
        }
    }
}
