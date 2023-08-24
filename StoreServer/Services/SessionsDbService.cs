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
    public class SessionsDbService
    {
        private readonly StoreDbContext _dbContext;
        public SessionsDbService(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            GetRidOfExpiredSessions();
        }
        public SESSION? Get(string username)
        {
            return _dbContext.Sessions
                .Include(o =>o.User)
                .FirstOrDefault(x => x.User.Name == username);
        }
        public void Delete(int? id)
        {
            if (id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "You dumb fuck, id is null while deleting Session item.");
            _dbContext.Users.Where(item => item.Id == id).ExecuteDelete();
            _dbContext.SaveChanges();
        }
        public void Insert(SESSION item)
        {
            if (item.Id != null) throw new ApiException(HttpStatusCode.BadRequest, "Cannot insert Session with id.");
            _dbContext.Sessions.Add(item);
            _dbContext.SaveChanges();
        }
        public void GetRidOfExpiredSessions()
        {
            _dbContext.Sessions.Where(item => item.ExpirationDate < DateTime.Now).ExecuteDelete();
        }
    }
}
