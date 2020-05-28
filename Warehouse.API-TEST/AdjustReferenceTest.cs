using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.API.Dtos;
using Warehouse.API.Helpers;
using Xunit;

namespace Warehouse.API_TEST
{
    public class AdjustReferenceTest
    {
        [Fact]
        public void CreateNewOrderTest()
        {
            //Arrange
            string reference = "430000";

            //Act
            CreateOrderDto action = AdjustReference.CreateNewOrder(reference);

            //Assert
            Assert.Equal("Argos", action.TypeOfCasing);
            Assert.Equal("3pM8", action.Casing);
        }
    }
}
