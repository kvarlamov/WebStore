using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase, IEmployeesData
    {
        private IEmployeesData _employeesData;
        public EmployeesController(IEmployeesData employeesData) => _employeesData = employeesData;
        [HttpPost, ActionName("Post")]
        public void Add(EmployeeView Employee) => _employeesData.Add(Employee);

        [HttpDelete("{id}")]
        public bool Delete(int id) => _employeesData.Delete(id);

        [HttpPut("{id}"), ActionName("Put")]
        public void Edit(int id, [FromBody] EmployeeView Employee) => _employeesData.Edit(id, Employee);

        [HttpGet, ActionName("Get")]
        public IEnumerable<EmployeeView> GetAll() => _employeesData.GetAll();

        [HttpGet("{id}"), ActionName("Get")]
        public EmployeeView GetById(int id) => _employeesData.GetById(id);

        [NonAction]
        public void SaveChanges() => throw new NotImplementedException();
    }
}
