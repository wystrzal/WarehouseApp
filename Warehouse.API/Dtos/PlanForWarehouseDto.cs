using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.API.Dtos
{
    public class PlanForWarehouseDto
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string TypeOfCasing { get; set; }
        public string Casing { get; set; }
        public string Cover { get; set; }
        public int Amount { get; set; }
        public string Chamber { get; set; }
        public string Line { get; set; }
        public string Status { get; set; }
    }
}
