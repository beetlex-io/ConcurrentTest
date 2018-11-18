using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;

namespace FastHttpApiServer
{
    [BeetleX.FastHttpApi.Controller(BaseUrl = "Employee")]
    class Program
    {
        static HttpApiServer mApiServer;

        static void Main(string[] args)
        {
            mApiServer = new HttpApiServer();
            mApiServer.ServerConfig.WriteLog = true;
            mApiServer.ServerConfig.LogToConsole = true;
            mApiServer.ServerConfig.Port = 8007;
            mApiServer.ServerConfig.LogLevel = BeetleX.EventArgs.LogType.Warring;
            mApiServer.ServerConfig.UrlIgnoreCase = true;
            mApiServer.Register(typeof(Program).Assembly);
            mApiServer.Open();
            Console.Write(mApiServer.BaseServer);
            Console.WriteLine(Environment.ProcessorCount);
            Console.Read();
        }
        public object Get(int count)
        {
            return new JsonResult(Employee.GetEmployees(count));
        }
        [Post]
        public object Add(Employee item)
        {
            return new JsonResult(item);
        }
        public object GetTime()
        {
            return new JsonResult(DateTime.Now);
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
