using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Models;

namespace VacationRental.Controllers
{
    [Route("api/v1/employees")]
    [ApiController]
    public class EmployeesController : Controller
    {

        [HttpGet]
        [Route("{employeeId:int}")]
        public Employee Get(int employeeId)
        {
            List<Employee> employees = new List<Employee>();

            Employee employee1 = new Employee()
            {
                Id = 1,
                FirstName = "Jack",
                LastName = "Smith"
            };

            Employee employee2 = new Employee()
            {
                Id = 1,
                FirstName = "Tom",
                LastName = "Hanks"
            };

            employees.Add(employee1);
            employees.Add(employee2);

            var emp = employees.FirstOrDefault(x => x.Id == employeeId);

            if (emp == null)
            {
                throw new ApplicationException("Not Found");
            }

            return emp;
        }

        //[HttpPost]
        //// POST api/v1/employee
        //public async Task<IActionResult> Create(EmployeeBindingModel request)
        //{
        //    await _employeeService.CreateEmployee(_mapper.Map<Employee>(request));

        //    return Ok();
        //}
    }
}
