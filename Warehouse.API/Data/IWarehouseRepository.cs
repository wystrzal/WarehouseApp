using SEIP.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIP.API.Data
{
    public interface IWarehouseRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<Plan>> GetOrders(string line);
        Task<Plan> GetOrder(int id);
    }
}
