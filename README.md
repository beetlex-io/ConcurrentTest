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
![](https://i.imgur.com/NSrZVim.png)
