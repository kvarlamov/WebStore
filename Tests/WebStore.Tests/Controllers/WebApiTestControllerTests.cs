using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Interfaces.Api;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiTestControllerTests
    {
        [TestMethod]
        public async Task Index_Returns_ViewWithValues()
        {
            var expectedValues = new[] { "1", "2", "3" };

            var valueService = new Mock<IValuesService>();
            valueService.Setup(service => service.GetAsync()).ReturnsAsync(expectedValues);

            var controller = new WebApiTestController(valueService.Object);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.Model);

            Assert.Equal(expectedValues.Length, model.Count());

            valueService.Verify(service => service.GetAsync());
            valueService.VerifyNoOtherCalls();
        }
    }
}
