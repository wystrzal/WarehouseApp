using Warehouse.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.API.Helpers
{
    public static class AdjustReference
    {
        public static CreateOrderDto CreateNewOrder(string reference)
        {
            var order = new CreateOrderDto();

            switch (reference)
            {
                case "430000":
                    order.TypeOfCasing = "Argos";
                    order.Casing = "3pM8";
                    order.Cover = "GHD";
                    order.Reference = reference;
                    order.Saucepan = "B";
                    break;
                case "431000":
                    order.TypeOfCasing = "Argos";
                    order.Casing = "4pM8";
                    order.Cover = "GHD";
                    order.Reference = reference;
                    order.Saucepan = "A";
                    break;
                case "432000":
                    order.TypeOfCasing = "Katun";
                    order.Casing = "3pM6";
                    order.Cover = "EXL";
                    order.Reference = reference;
                    order.Saucepan = "B";
                    order.Chamber = "X123";
                    break;
                default:
                    order.Reference = null;
                    break;
            }

            return order;
        }
    }
}
