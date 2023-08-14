using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreServer.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer
{
    internal class StoreDbService
    {
        private List<Type> _tableTypes;
        private List<PropertyInfo> _tablesPropoerties;
        private DatabaseContext DbContext { get; }
        public StoreDbService()
        {
            _tableTypes = new List<Type>();
            _tableTypes.Add(typeof(ORDER));
            _tableTypes.Add(typeof(ORDERDETAILS));
            _tableTypes.Add(typeof(STORAGE));
            var services = new ServiceCollection();

            var connectionString = "Server = localhost; Database = Store; User = root";
            services.AddDbContext<DatabaseContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            DbContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
            DbContext.Database.EnsureCreated();
            _tablesPropoerties = typeof(DatabaseContext).GetProperties().ToList();
        }
        public List<T> Get<T>() where T : class
        {
            string typeString = typeof(T).Name.ToLower();
            if (typeString == "order") typeString = "orders";
            return ((DbSet<T>)_tablesPropoerties.FirstOrDefault(prop => prop.Name.ToLower() == typeString).GetValue(DbContext)).ToList();
        }
        public void Update(object obj)
        {
            DbContext.Update(obj);
            DbContext.SaveChanges();
        }
        public void Delete(object obj)
        {

            DbContext.Remove(obj);
            DbContext.SaveChanges();

        }
        public void Insert(object obj)
        {
            DbContext.Add(obj);
            DbContext.SaveChanges();
        }
    }
}
