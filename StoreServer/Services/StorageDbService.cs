using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StoreServer.DatabaseModels;
using StoreServer.Utils;
using System.Net;

namespace StoreServer.Services
{
    public class StorageDbService
    {
        private readonly StoreDbContext _dbContext;
        public StorageDbService(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<STORAGE> Get()
        {
            return _dbContext.Storage.ToList();
        }

        public STORAGE? Get(int id)
        {
            return _dbContext.Storage.FirstOrDefault(item => item.Id == id);
        }
        public void Update(STORAGE item)
        {
            if (item.Id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Id cannot be null in update Storage.");
            var dbItem = Get(item.Id.Value);
            if (dbItem == null)
                throw new ApiException(HttpStatusCode.BadRequest, $"Cannot find entry in Storage to update id: {item.Id}");
            _dbContext.Entry(dbItem).CurrentValues.SetValues(item);
            _dbContext.SaveChanges();
        }
        public void Delete(int? id)
        {
            if (id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "You dumb fuck, id is null while deleting Storage item.");
            _dbContext.Storage.Where(item => item.Id == id).ExecuteDelete();
            _dbContext.SaveChanges();
        }
        public void Insert(STORAGE item)
        {
            if (item.Id != null) throw new ApiException(HttpStatusCode.BadRequest, "Cannot insert Storage with id.");
            _dbContext.Storage.Add(item);
            _dbContext.SaveChanges();
        }
    }
}
