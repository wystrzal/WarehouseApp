using AutoMapper;
using Warehouse.API.Dtos;
using Warehouse.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.API.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Plan, WarehousePlanDto>();
            CreateMap<CreateOrderDto, Plan>();
            CreateMap<ChangeStatusDto, Plan>();
        }
    }
}
