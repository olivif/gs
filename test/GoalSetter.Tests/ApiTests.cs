namespace GoalSetter
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Xunit;

    public class ApiTests
    {
        private static TestServer server;
        private static HttpClient client;

        public ApiTests()
        {
            var baseAddress = "https://localhost/";

            var cert = new X509Certificate2();

            server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseUrls(baseAddress)
                .Configure((builder) =>
                {
                    var b = new ConfigurationBuilder();
                    b.AddEnvironmentVariables("Authentication:Microsoft:ClientId");
                })
                .UseKestrel(options => {
                    options.ThreadCount = 4;
                    options.UseHttps(cert);
                    options.UseConnectionLogging();
                }));

            client = server.CreateClient();
            client.BaseAddress = new Uri(baseAddress);
        }

        [Fact]
        public async Task Api()
        {
            // Act
            var response = await client.GetAsync("");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        }
    }
}
