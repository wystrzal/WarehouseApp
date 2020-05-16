using AutoMapper;
using SEIP.API.Dtos;
using SEIP.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEIP.API.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Plan, PlanForWarehouseDto>();
            CreateMap<PlanForCreationDto, Plan>();
            CreateMap<PlanForChangeStatusDto, Plan>();
        }
    }
}
