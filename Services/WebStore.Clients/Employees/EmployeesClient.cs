using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebStore.Clients.Base;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration config) : base(config, "api/employees")
        {

        }

        public void Add(EmployeeView Employee) => Post<EmployeeView>(_ServiceAddress, Employee);

        public bool Delete(int id) => Delete($"{_ServiceAddress}/{id}").IsSuccessStatusCode;

        public void Edit(int id, EmployeeView Employee) => Put($"{_ServiceAddress}/{id}", Employee);

        public IEnumerable<EmployeeView> GetAll() => Get<List<EmployeeView>>(_ServiceAddress);

        public EmployeeView GetById(int id) => Get<EmployeeView>($"{_ServiceAddress}/{id}");

        public void SaveChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}
