using StoreServer.DatabaseModels;
using StoreServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Services
{
    public class UsersDbService
    {
        private readonly StoreDbContext _dbContext;
        public UsersDbService(StoreDbContext dbContext)
        {
             _dbContext = dbContext;
        }
        public List<USER> Get()
        {
            return _dbContext.Users.ToList();
        }
        public USER Get(string username)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Name == username);
        }
        public USER Get(int id)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id == id);
        }
        public void Update(USER item)
        {
            if (item.Id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Id cannot be null in update User.");
            var dbItem = Get(item.Id.Value);
            if (dbItem == null)
                throw new ApiException(HttpStatusCode.BadRequest, $"Cannot find entry in User to update id: {item.Id}");
            _dbContext.Entry(dbItem).CurrentValues.SetValues(item);
            _dbContext.SaveChanges();
        }
        public void Delete(int? id)
        {
            if (id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "You dumb fuck, id is null while deleting User item.");
            _dbContext.Users.Where(item => item.Id == id).ExecuteDelete();
            _dbContext.SaveChanges();
        }
        public void Insert(USER item)
        {
            if (item.Id != null) throw new ApiException(HttpStatusCode.BadRequest, "Cannot insert User with id.");
            _dbContext.Users.Add(item);
            _dbContext.SaveChanges();
        }
    }
}
