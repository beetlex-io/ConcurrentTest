# ConcurrentTest
### Install Packet
`Install-Package BeetleX.ConcurrentTest -Version 0.2.8`
### Samples
```
     public class FastHttpClientTest
    {
        public FastHttpClientTest()
        {
            httpApiClient = new HttpApiClient(Host);
            clientApi = httpApiClient.CreateWebapi<IHttpClientApi>();
        }
        private string Host = "http://localhost:8007";
        private HttpApiClient httpApiClient;
        private IEmployeeApi employeeApi;
        [CTestCase]
        public void AddEmployee()
        {
            employeeApi.AddEmployee(Employee.GetEmployee());
        }
        [CTestCase]
        public void ListEmployees()
        {
            employeeApi.ListEmployees(2);
        }
        [JsonFormater]
        public interface IEmployeeApi
        {
            [Get(Route = "api/employee/{count}")]
            List<Employee> ListEmployees(int count);
            [Post(Route = "api/employee")]
            Employee AddEmployee(Employee item);
        }
    }
```
```
CTester.RunTest<FastHttpClientTest>(10, 500000);
```
### Report
```
***********************************************************************
* https://github.com/IKende/ConcurrentTest.git
* Copyright ? ikende.com 2018 email:henryfan@msn.com
* ServerGC:True
***********************************************************************
* AddEmployee test prepping completed
-----------------------------------------------------------------------
*                     [500000/500000]|threads:[10]
*       Success:[      0/s]|total:[      500000][min:23448/s  max:24561/s]
*         Error:[      0/s]|total:[           0][min:0/s  max:0/s]
-----------------------------------------------------------------------
*       0ms-0.1ms:[             ]    0.1ms-0.5ms:[      435,604]
*       0.5ms-1ms:[       59,863]        1ms-5ms:[        4,356]
*        5ms-10ms:[          142]      10ms-50ms:[           35]
*      50ms-100ms:[             ]   100ms-1000ms:[             ]
*   1000ms-5000ms:[             ] 5000ms-10000ms:[             ]
***********************************************************************

***********************************************************************
* ListEmployees test prepping completed
-----------------------------------------------------------------------
*                     [500000/500000]|threads:[10]
*       Success:[      0/s]|total:[      500000][min:28105/s  max:28829/s]
*         Error:[      0/s]|total:[           0][min:0/s  max:0/s]
-----------------------------------------------------------------------
*       0ms-0.1ms:[             ]    0.1ms-0.5ms:[      476,342]
*       0.5ms-1ms:[       20,641]        1ms-5ms:[        2,922]
*        5ms-10ms:[           80]      10ms-50ms:[           15]
*      50ms-100ms:[             ]   100ms-1000ms:[             ]
*   1000ms-5000ms:[             ] 5000ms-10000ms:[             ]
***********************************************************************


```
