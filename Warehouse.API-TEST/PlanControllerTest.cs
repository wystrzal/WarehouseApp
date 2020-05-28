using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.API.Controllers;
using Warehouse.API.Data;
using Warehouse.API.Dtos;
using Warehouse.API.Models;
using Xunit;

namespace Warehouse.API_TEST
{
        public class PlanControllerTest
        {
            readonly Mock<IWarehouseRepository> warehouseMock;
            readonly Mock<IMapper> mapperMock;

            public PlanControllerTest()
            {
                warehouseMock = new Mock<IWarehouseRepository>();
                mapperMock = new Mock<IMapper>();
            }

            private Mock<UserManager<User>> GetMockUserManager()
            {
                var userStoreMock = new Mock<IUserStore<User>>();

                return new Mock<UserManager<User>>(
                   userStoreMock.Object, null, null, null, null, null, null, null, null);
            }

            [Fact]
            public async Task GetOrdersForProductionOkStatus()
            {
                //Arange
                var line = "bc1";

                warehouseMock.Setup(s => s.GetOrders(line)).Returns(Task.FromResult(GetPlans()));

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                   GetMockUserManager().Object);

                //Act
                var action = await controller.GetOrdersForProduction(line);
                var result = action as OkObjectResult;
                var items = result.Value as IEnumerable<Plan>;

                //Assert
                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);

                Assert.NotNull(items);
                Assert.Equal(3, items.Count());
                Assert.Equal(line, items.First().Line);
            }

            [Fact]
            public async Task GetOrdersForWarehouseOkStatus()
            {
                //Arrange
                var line = "bc1";

                warehouseMock.Setup(s => s.GetOrders(line)).Returns(Task.FromResult(GetPlans()));

                mapperMock.Setup(s => s.Map<IEnumerable<WarehousePlanDto>>(It.IsAny<IEnumerable<Plan>>()))
                    .Returns(GetPlansForWarehouse());

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                    GetMockUserManager().Object);

                //Act
                var action = await controller.GetOrdersForWarehouse(line);
                var result = action as OkObjectResult;
                var items = result.Value as IEnumerable<WarehousePlanDto>;

                //Assert
                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);

                Assert.NotNull(items);
                Assert.Equal(3, items.Count());
                Assert.Equal(line, items.First().Line);
            }

            [Fact]
            public async Task ChangeStatusNotFoundStatus()
            {
                //Arrange

                warehouseMock.Setup(s => s.GetOrder(It.IsAny<int>())).Returns(() => Task.Run(() =>
                { return (Plan)null; }));

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                    GetMockUserManager().Object);


                //Act
                var action = await controller.ChangeStatus(It.IsAny<int>(), It.IsAny<ChangeStatusDto>()) as NotFoundResult;

                //Assert
                Assert.NotNull(action);
                Assert.Equal(404, action.StatusCode);
            }

            [Fact]
            public async Task ChangeStatusBadRequestStatus()
            {
                //Arrange
                var id = 1;
                Plan plan = new Plan { Id = id };

                warehouseMock.Setup(s => s.GetOrder(id)).Returns(() => Task.Run(() =>
                { return plan; }));

                mapperMock.Setup(s => s.Map(It.IsAny<ChangeStatusDto>(), plan));

                warehouseMock.Setup(s => s.SaveAll()).Returns(() => Task.Run(() => { return false; }));

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                    GetMockUserManager().Object);

                //Act
                var action = await controller.ChangeStatus(id, It.IsAny<ChangeStatusDto>()) as BadRequestObjectResult;

                //Assert
                Assert.NotNull(action);
                Assert.Equal(400, action.StatusCode);
                Assert.Equal("Nie udało się zmienić statusu.", action.Value);
            }

            [Fact]
            public async Task ChangeStatusOkStatus()
            {
                //Arrange
                var id = 1;

                Plan plan = new Plan { Id = id };

                warehouseMock.Setup(s => s.GetOrder(id)).Returns(() => Task.Run(() =>
                { return plan; }));

                mapperMock.Setup(s => s.Map(It.IsAny<ChangeStatusDto>(), plan));

                warehouseMock.Setup(s => s.SaveAll()).Returns(() => Task.Run(() => { return true; }));

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                    GetMockUserManager().Object);

                //Act
                var action = await controller.ChangeStatus(id, It.IsAny<ChangeStatusDto>()) as OkResult;

                //Assert
                Assert.NotNull(action);
                Assert.Equal(200, action.StatusCode);
            }

            [Fact]
            public async Task DeleteOrderNotFoundStatus()
            {
                //Arrange
                var id = 1;
                var line = "bc1";

                Plan plan = new Plan { Id = id, Line = line };

                warehouseMock.Setup(s => s.GetOrder(id)).Returns((Task.FromResult((Plan)null)));

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                    GetMockUserManager().Object);

                //Act 
                var action = await controller.DeleteOrder(line, id) as NotFoundObjectResult;

                //Assert
                Assert.NotNull(action);
                Assert.Equal(404, action.StatusCode);
                Assert.Equal("Nie znaleziono.", action.Value);
            }

            [Fact]
            public async Task DeleteOrderBadRequestStatus()
            {
                //Arrange
                var id = 1;
                var line = "bc1";

                Plan plan = new Plan { Id = id, Line = line, Position = 1 };

                warehouseMock.Setup(s => s.GetOrder(id)).Returns(() => Task.Run(() =>
                { return plan; }));

                warehouseMock.Setup(s => s.GetOrders(line)).Returns(Task.FromResult(GetPlans()));

                warehouseMock.Setup(s => s.SaveAll()).Returns(() => Task.Run(() => { return false; }));

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                    GetMockUserManager().Object);

                //Act 
                var action = await controller.DeleteOrder(line, id) as BadRequestObjectResult;

                //Assert
                Assert.NotNull(action);
                Assert.Equal(400, action.StatusCode);
                Assert.Equal("Nie udało się usunąć zamówienia.", action.Value);
            }

            [Fact]
            public async Task DeleteOrderNoContentStatus()
            {
                //Arrange
                var id = 1;
                var line = "bc1";

                Plan plan = new Plan { Id = id, Line = line, Position = 1 };

                warehouseMock.Setup(s => s.GetOrder(id)).Returns(() => Task.Run(() =>
                { return plan; }));

                warehouseMock.Setup(s => s.GetOrders(line)).Returns(Task.FromResult(GetPlans()));

                warehouseMock.Setup(s => s.SaveAll()).Returns(() => Task.Run(() => { return true; }));

                PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object,
                    GetMockUserManager().Object);

                //Act 
                var action = await controller.DeleteOrder(line, id) as NoContentResult;

                //Assert
                Assert.NotNull(action);
                Assert.Equal(204, action.StatusCode);
            }

        [Fact]
        public async Task AddOrderBadRequestStatus()
        {
            //Arrange
            var userManager = GetMockUserManager();
            User user = new User { Id = 1, UserName = "test" };
            ReferenceToAddOrderDto reference = new ReferenceToAddOrderDto { Reference = "1" };

            userManager.Setup(x => x.FindByIdAsync("1")).Returns(Task.FromResult(user));

            PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object, userManager.Object);

            //Act
            var action = await controller.AddOrder("bc1", 1, reference, 1) as BadRequestObjectResult;

            //Arrange
            Assert.Equal(400, action.StatusCode);
            Assert.NotNull(action.Value);

        }

        [Fact]
        public async Task AddOrderOkStatus()
        {
            //Arrange
            var userManager = GetMockUserManager();

            User user = new User { Id = 1, UserName = "test" };
            CreateOrderDto createOrder = new CreateOrderDto { Reference = "430000" };
            ReferenceToAddOrderDto reference = new ReferenceToAddOrderDto { Reference = "430000" };
            Plan plan = new Plan { Id = 5, Reference = "430000"};

            userManager.Setup(x => x.FindByIdAsync("1")).Returns(Task.FromResult(user));
            warehouseMock.Setup(x => x.GetOrders("bc1")).Returns(Task.FromResult(GetPlans()));
            mapperMock.Setup(x => x.Map<Plan>(createOrder)).Returns(plan);
            warehouseMock.Setup(x => x.Add(plan));
            warehouseMock.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object, userManager.Object);

            //Act
            var action = await controller.AddOrder("bc1", 1, reference, 2) as OkResult;

            //Arrange
            Assert.Equal(200, action.StatusCode);
        }

        [Fact]
        public async Task AddBadRequestStatus()
        {
            //Arrange
            var userManager = GetMockUserManager();

            User user = new User { Id = 1, UserName = "test" };
            CreateOrderDto createOrder = new CreateOrderDto { Reference = "430000" };
            ReferenceToAddOrderDto reference = new ReferenceToAddOrderDto { Reference = "430000" };
            Plan plan = new Plan { Id = 5, Reference = "430000" };

            userManager.Setup(x => x.FindByIdAsync("1")).Returns(Task.FromResult(user));
            warehouseMock.Setup(x => x.GetOrders("bc1")).Returns(Task.FromResult(GetPlans()));
            mapperMock.Setup(x => x.Map<Plan>(createOrder)).Returns(plan);
            warehouseMock.Setup(x => x.Add(plan));
            warehouseMock.Setup(x => x.SaveAll()).Returns(Task.FromResult(false));

            PlanController controller = new PlanController(warehouseMock.Object, mapperMock.Object, userManager.Object);

            //Act
            var action = await controller.AddOrder("bc1", 1, reference, 2) as BadRequestObjectResult;

            //Arrange
            Assert.Equal(400, action.StatusCode);
        }

        private static IEnumerable<Plan> GetPlans()
            {
                List<Plan> plans = new List<Plan>();

                for (int i = 0; i < 3; i++)
                {
                    plans.Add(new Plan
                    {
                        Amount = 60,
                        Id = i + 1,
                        Line = "bc1",
                        Casing = "3pM8",
                        Cover = "EXL",
                        Position = i + 1,
                        Reference = "430000",
                        Status = "Nowe",
                        TypeOfCasing = "Argos",
                        Saucepan = "B",
                        Chamber = "X123"
                    });
                }

                return plans;
            }

            private static IEnumerable<WarehousePlanDto> GetPlansForWarehouse()
            {
                List<WarehousePlanDto> plans = new List<WarehousePlanDto>();

                for (int i = 0; i < 3; i++)
                {
                    plans.Add(new WarehousePlanDto
                    {
                        Amount = 60,
                        Id = i + 1,
                        Line = "bc1",
                        Casing = "3pM8",
                        Cover = "EXL",
                        Position = i + 1,
                        Status = "Nowe",
                        TypeOfCasing = "Argos",
                        Chamber = "X123"
                    });
                }

                return plans;
            }
        }
}
