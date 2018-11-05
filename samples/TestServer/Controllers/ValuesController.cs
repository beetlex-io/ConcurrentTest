using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET api/json/5
        [HttpGet("{count}")]
        public JsonResult Get(int count)
        {
            return new JsonResult(Employee.GetEmployees(count));
        }
        [HttpPost]
        public JsonResult Post([FromBody]Employee value)
        {
            return new JsonResult(value);
        }
    }

    public class Employee
    {
        public int EmployeeID
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public Double RequestTime
        {
            get; set;
        }

        public static Employee GetEmployee()
        {
            Employee result = new Employee();
            result.EmployeeID = 1;
            result.LastName = "Davolio";
            result.FirstName = "Nancy";
            result.Address = "ja";
            result.City = "Seattle";
            result.Region = "WA";
            result.Country = "USA";
            return result;
        }

        public static List<Employee> GetEmployees(int count)
        {
            List<Employee> result = new List<Employee>();
            for (int i = 0; i < count; i++)
            {

                result.Add(GetEmployee());
            }
            return result;
        }
    }
}
