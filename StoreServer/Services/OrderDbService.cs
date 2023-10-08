using System.Net;
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
            var list = _dbContext.Orders
                .Include(o => o.Details)
                .ToList();
            return list;
        }
        public ORDER? Get(int id)
        {
            return _dbContext.Orders
                .Include(o => o.Details)
                .FirstOrDefault(item => item.Id == id);
        }
        public ORDERDETAILS? GetOrderDetailsItem(int id)
        {
            return _dbContext.OrderDetails
                .FirstOrDefault(item => item.Id == id);
        }

        public ORDER Insert(ORDER item)
        {
            if (item.Id != null) throw new ApiException(HttpStatusCode.BadRequest, "Cannot insert Order with id.");
            var newItem = _dbContext.Orders.Add(item).Entity;
            _dbContext.SaveChanges();
            return newItem;
        }
        public ORDERDETAILS Insert(ORDERDETAILS item)
        {
            if (item.Id != null) throw new ApiException(HttpStatusCode.BadRequest, "Cannot insert OrderDetails with id.");
            var newItem = _dbContext.OrderDetails.Add(item).Entity;
            _dbContext.SaveChanges();
            return newItem;
        }

        public void Update(ORDER item)
        {
            if (item.Id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Id cannot be null in update Orders.");
            var dbItem = Get(item.Id.Value);
            if (dbItem == null)
                throw new ApiException(HttpStatusCode.BadRequest, $"Cannot find entry in Orders to update id: {item.Id}");
            _dbContext.Entry(dbItem).CurrentValues.SetValues(item);
            _dbContext.SaveChanges();
        }
        public void Update(ORDERDETAILS item)
        {
            if (item.Id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Id cannot be null in update OrderDetails.");
            var dbItem = _dbContext.OrderDetails.FirstOrDefault(x=> x.Id == item.Id);
            if (dbItem == null)
                throw new ApiException(HttpStatusCode.BadRequest, $"Cannot find entry in Orders to update id: {item.Id}");
            _dbContext.Entry(dbItem).CurrentValues.SetValues(item);
            _dbContext.SaveChanges();
        }

        public void DeleteOrder(int? id)
        {
            if (id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "You dumb fuck, id is null while deleting Orders item.");
            _dbContext.Orders.Where(item => item.Id == id).ExecuteDelete();
            _dbContext.SaveChanges();
        }
        public void DeleteOrderDetail(int? id)
        {
            if (id == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Id is null while deleting OrderDetails item.");
            _dbContext.OrderDetails.Where(item=>item.Id == id).ExecuteDelete();
            _dbContext.SaveChanges();
        }
    }
}
