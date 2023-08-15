﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StoreServer.DatabaseModels;
using StoreServer.Utils;

namespace StoreServer.Services
{
    public class OrderDbService
    {
        private readonly StoreDbContext _dbContext;
        public OrderDbService(StoreDbContext context)
        {
            _dbContext = context;
        }

        public List<ORDER> Get()
        {
            return _dbContext.Orders.ToList();
        }

        public ORDER? Get(int id)
        {
            return _dbContext.Orders.FirstOrDefault(item => item.Id == id);
        }

        public void Update(ORDER item)
        {
            if (item.Id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Id cannot be null in insert Orders.");
            var dbItem = Get(item.Id.Value);
            if (dbItem == null)
                throw new ApiException(HttpStatusCode.BadRequest, $"Cannot find entry in Orders to update id: {item.Id}");
            _dbContext.Entry(dbItem).CurrentValues.SetValues(item);
            _dbContext.SaveChanges();
        }

        public void Insert(ORDER item)
        {
            if (item.Id != null) throw new ApiException(HttpStatusCode.BadRequest, "Cannot insert Order with id.");
            _dbContext.Orders.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(int? id)
        {
            if (id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "You dumb fuck, id is null while deleting Orders item.");
            _dbContext.Orders.Where(item=>item.Id == id).ExecuteDelete();
        }
    }
}
