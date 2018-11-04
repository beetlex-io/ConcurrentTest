# ConcurrentTest
### Install Packet
`Install-Package BeetleX.ConcurrentTest -Version 0.1.0`
### Samples
```
    public class FastHttpClientTest
    {
        public FastHttpClientTest()
        {
            httpApiClient = new BeetleX.FastHttpApi.HttpApiClient(Host);
            clientApi = httpApiClient.CreateWebapi<IHttpClientApi>();
        }

        private string Host = "http://localhost:8007";

        private BeetleX.FastHttpApi.HttpApiClient httpApiClient;

        private IHttpClientApi clientApi;

        public void Run()
        {
            clientApi.ListEmployees(10);
        }

        [JsonFormater]
        public interface IHttpClientApi
        {
            [BeetleX.FastHttpApi.Get(Route = "api/json/{count}")]
            List<Employee> ListEmployees(int count);
            [BeetleX.FastHttpApi.Post(Route = "api/json")]
            Employee AddEmployee(Employee item);
        }
    }
```
```
BeetleX.ConcurrentTest.CTester cTester = new BeetleX.ConcurrentTest.CTester();
FastHttpClientTest fastHttpClient = new FastHttpClientTest();
cTester.Run(5, fastHttpClient.Run);
cTester.Report();
```
### Report
```
***********************************************************************
* https://github.com/IKende/ConcurrentTest.git
* Copyright © ikende.com 2018 email:henryfan@msn.com
* ServerGC:True
* prepping completed                                        
****************ConcurrentTest[11/4/18 8:34:19 PM]*********************
*                     1000000/1000000|threads:20
*       Success:     0/s total:     1000000|[min:14967/s  max:31804/s]
*         Error:     0/s total:           0|[min:0/s  max:0/s]
-----------------------------------------------------------------------
*          [0ms-0.1ms]:
*        [0.1ms-0.5ms]:148,995
*          [0.5ms-1ms]:846,691
*            [1ms-5ms]:4,232
*           [5ms-10ms]:72
*          [10ms-50ms]:10
*         [50ms-100ms]:
*       [100ms-1000ms]:
*    [1000ms-100000ms]:
***********************************************************************

```
