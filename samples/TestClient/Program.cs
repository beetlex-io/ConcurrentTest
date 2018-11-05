using BeetleX.ConcurrentTest;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Clients;
using System;
using System.Collections.Generic;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CTester.RunTest<FastHttpClientTest>(10, 500000);
            Console.Read();
        }
    }

    public class FastHttpClientTest
    {
        public FastHttpClientTest()
        {
            httpApiClient = new HttpApiClient(Host);
            clientApi = httpApiClient.CreateWebapi<IHttpClientApi>();
        }

        private string Host = "http://localhost:8007";

        private BeetleX.FastHttpApi.HttpApiClient httpApiClient;

        private IHttpClientApi clientApi;

        [CTestCase]
        public void AddEmployee()
        {
            clientApi.AddEmployee(Employee.GetEmployee());
        }
        [CTestCase]
        public void ListEmployees()
        {
            clientApi.ListEmployees(2);
        }

        [JsonFormater]
        public interface IHttpClientApi
        {
            [Get(Route = "api/employee/{count}")]
            List<Employee> ListEmployees(int count);
            [Post(Route = "api/employee")]
            Employee AddEmployee(Employee item);
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
