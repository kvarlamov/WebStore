using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Product
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _Logger;

        private readonly List<EmployeeView> _Employees = new List<EmployeeView>
        {
            new EmployeeView { Id = 1, SecondName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 35 },
            new EmployeeView { Id = 2, SecondName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 25 },
            new EmployeeView { Id = 3, SecondName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
        };

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> logger)
        {
            _Logger = logger;
        }

        public IEnumerable<EmployeeView> GetAll() => _Employees;

        public EmployeeView GetById(int id) => _Employees.FirstOrDefault(e => e.Id == id);

        public void Add(EmployeeView employee)
        {
            if(employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee) || _Employees.Any(e => e.Id == employee.Id))
            {
                _Logger.LogWarning("Attemp to add employee that already exist with id " + employee.Id);
                return;
            }
            
            employee.Id = _Employees.Count == 0 ? 1 : _Employees.Max(e => e.Id) + 1;
            _Employees.Add(employee);
            _Logger.LogInformation($"employee {employee.FirstName} with id={employee.Id} succesfully added");
        }

        public void Edit(int id, EmployeeView Employee)
        {
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            var db_employee = GetById(id);
            if (db_employee is null)
                throw new InvalidOperationException($"Employee with id {id} didn't find");

            db_employee.FirstName = Employee.FirstName;
            db_employee.SecondName = Employee.SecondName;
            db_employee.Patronymic = Employee.Patronymic;
            db_employee.Age = Employee.Age;
        }

        public bool Delete(int id)
        {
            var db_employee = GetById(id);
            return db_employee != null && _Employees.Remove(db_employee);
        }

        public void SaveChanges() {  }
    }
}
