﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase, IEmployeesData
    {
        private IEmployeesData _employeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData employeesData, ILogger<EmployeesController> logger)
        {
            _employeesData = employeesData;
            _Logger = logger;
        }

        [HttpPost, ActionName("Post")]
        public void Add(EmployeeView employee)
        {
            using (_Logger.BeginScope($"Adding new employee {employee.FirstName}"))
            {
                _employeesData.Add(employee);
            }
        }

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
