using BeetleX.ConcurrentTest;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Clients;
using System;
using System.Collections.Generic;

namespace TestClient
{
    class Program
    {
        //public static string Host = "http://172.16.0.8:8007";
        public static string Host = "http://192.168.2.19:8007";
        static void Main(string[] args)
        {
            BeetleX.FastHttpApi.HttpClientPoolFactory.SetPoolSize(Host, 20, 50);
            CTester.RunTest<FastHttpClientTest>(20, 1000000);
            Console.Read();
        }
    }

    public class FastHttpClientTest
    {
        public FastHttpClientTest()
        {
            httpApiClient = new HttpApiClient(Program.Host);
            clientApi = httpApiClient.CreateWebapi<IHttpClientApi>();
        }

        private BeetleX.FastHttpApi.HttpApiClient httpApiClient;
        private IHttpClientApi clientApi;
        [CTestCase]
        public void AddEmployee()
        {
            clientApi.Add(Employee.GetEmployee());
        }
        [CTestCase]
        public void ListEmployees()
        {
            clientApi.Get(2);
        }
        [CTestCase]
        public void GetTime()
        {
            clientApi.GetTime();
        }

        [JsonFormater]
        [Controller(BaseUrl = "Employee")]
        public interface IHttpClientApi
        {
            [Get]
            List<Employee> Get(int count);
            [Post]
            Employee Add(Employee item);
            [Get]
            DateTime GetTime();
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
