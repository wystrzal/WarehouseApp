using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Data;
using Warehouse.API.Dtos;
using Warehouse.API.Helpers;
using Warehouse.API.Models;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IWarehouseRepository seipRepository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public PlanController(IWarehouseRepository seipRepository, IMapper mapper, UserManager<User> userManager)
        {
            this.seipRepository = seipRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        //View orders for user with warehouse role.
        [Authorize(Roles = "Warehouse")]
        [HttpGet("warehouse/{line}")]
        public async Task<IActionResult> GetOrdersForWarehouse(string line)
        {
            var orders = await seipRepository.GetOrders(line);

            var ordersForWarehouse = mapper.Map<IEnumerable<PlanForWarehouseDto>>(orders);

            return Ok(ordersForWarehouse);
        }

        //View orders for user with production role.
        [Authorize(Roles = "Production")]
        [HttpGet("production/{line}")]
        public async Task<IActionResult> GetOrdersForProduction(string line)
        {
            var orders = await seipRepository.GetOrders(line);

            return Ok(orders);
        }


        //Create that many orders as we have in repeatAmount variable for user line.
        [Authorize(Roles = "Production")]
        [HttpPost("{line}/{id}")]
        public async Task<IActionResult> AddOrder(string line, int id,
            PlanForReferenceToCreationDto planForReferenceToCreationDto, [FromQuery]int repeatAmount)
        {
            var currentUser = await userManager.FindByIdAsync(id.ToString());

            var order = AdjustReference.CreateNewOrder(planForReferenceToCreationDto.Reference);

            if (order.Reference == null)
                return BadRequest("Podana referencja jest nieprawidłowa");

            order.Status = "Nowe";
            order.Line = currentUser.UserName;
            order.Amount = planForReferenceToCreationDto.Amount;

            var orders = await seipRepository.GetOrders(line);

            for (int j = 0; j < repeatAmount; j++)
            {
                var position = orders.Select(o => o.Position).ToList();

                //If entered position is greater than zero, all other orders positions add plus one.
                if (planForReferenceToCreationDto.Position > 0)
                {
                    var positionToCheck = position.Where(p => p >= planForReferenceToCreationDto.Position).ToList();

                    for (int i = 0; i < positionToCheck.Count(); i++)
                    {
                        var currentPosition = orders.Where(o => o.Position == positionToCheck[i]).LastOrDefault();

                        currentPosition.Position += 1;
                    }
                 
                    //Add to new position entered position and current j value.
                    order.Position = planForReferenceToCreationDto.Position + j;
                }
                else
                {
                    var positionToCheck = position.FirstOrDefault();

                    //If orders have any data, then new position is equal last position + 1 (and current j value).
                    if (positionToCheck > 0)
                    {
                        var positionToAdd = position.OrderByDescending(p => p).FirstOrDefault();

                        order.Position = positionToAdd + 1 + j;
                    }
                    else
                    {
                        order.Position = 1 + j;
                    }
                }

                var newOrder = mapper.Map<Plan>(order);

                seipRepository.Add(newOrder);             
            }

            if (await seipRepository.SaveAll())
                return NoContent();

    
            return BadRequest("Nie udało się dodać zamówienia.");
        }

        //Change status for selected order.
        [Authorize(Roles = "Warehouse")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeStatus(int id, PlanForChangeStatusDto planForChangeStatusDto)
        {
            var order = await seipRepository.GetOrder(id);

            if (order == null)
                return NotFound();
            
            mapper.Map(planForChangeStatusDto, order);

            if (await seipRepository.SaveAll())
                return Ok();

            return BadRequest("Nie udało się zmienić statusu.");        
        }

        //Delete selected order, then change other orders position with position greater than selected order by -1.
        [Authorize(Roles = "Production")]
        [HttpPost("{line}/delete/{id}")]
        public async Task<IActionResult> DeleteOrder(string line, int id)
        {
            var order = await seipRepository.GetOrder(id);

            if (order == null)
                return NotFound("Nie znaleziono.");

            var orders = await seipRepository.GetOrders(line);

            var orderPosition = order.Position;

            var positionToCheck = orders.Where(o => o.Position > orderPosition).Select(o => o.Position).ToList();

            for (int i = 0; i < positionToCheck.Count(); i++)
            { 
                var currentPosition = orders.Where(o => o.Position == positionToCheck[i]).FirstOrDefault();
                currentPosition.Position -= 1;
            }

            seipRepository.Delete(order);

            if (await seipRepository.SaveAll())
                return NoContent();

            return BadRequest("Nie udało się usunąć zamówienia.");   
        }
    }
}