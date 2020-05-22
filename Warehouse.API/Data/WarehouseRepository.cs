using Microsoft.EntityFrameworkCore;
using Warehouse.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.API.Data
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DataContext dataContext;

        public WarehouseRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Add<T>(T entity) where T : class
        {
            dataContext.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            dataContext.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            dataContext.Remove(entity);
        }

        public async Task<Plan> GetOrder(int id)
        {
            return await dataContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Plan>> GetOrders(string line)
        {
            return await dataContext.Plans.Where(p => p.Line == line).OrderBy(p => p.Position).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await dataContext.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
