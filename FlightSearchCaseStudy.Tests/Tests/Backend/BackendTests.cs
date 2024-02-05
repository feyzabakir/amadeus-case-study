using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using FlightSearchCaseStudy.Tests.Models;

namespace FlightSearchCaseStudy.Tests.Tests.Backend
{
    public class BackendTests
    {
        private readonly string _apiUrl = "https://flights-api.buraky.workers.dev/";

        private HttpClient GetMockHttpClient(HttpStatusCode statusCode, string content = "")
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json"), 
                });

            return new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri(_apiUrl),
            };
        }


        [Fact]
        public async Task CheckStatusCode()
        {
            // Arrange
            var expectedStatusCode = HttpStatusCode.OK;
            var httpClient = GetMockHttpClient(expectedStatusCode);

            // Act
            var response = await httpClient.GetAsync(_apiUrl);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task CheckResponseContent()
        {
            // Arrange
            var mockContent = new { data = new[] { new { id = 1, from = "IST", to = "LAX", date = "2022-12-13" } } };
            var jsonContent = JsonConvert.SerializeObject(mockContent);
            var httpClient = GetMockHttpClient(HttpStatusCode.OK, jsonContent);

            // Act
            var response = await httpClient.GetAsync(_apiUrl);
            var content = await response.Content.ReadAsStringAsync();
  
            var json = JObject.Parse(content);
            var flightResponse = JsonConvert.DeserializeObject<Flight[]>(json["data"]!.ToString());

            // Assert
            Assert.NotNull(flightResponse);
            Assert.NotEmpty(flightResponse);
            foreach (var flight in flightResponse)
            {
                Assert.True(flight.Id > 0, "Flight Id should be greater than 0.");
                Assert.NotEmpty(flight.From);
                Assert.NotEmpty(flight.To);
                Assert.NotEmpty(flight.Date);
            }
        }

        [Fact]
        public async Task CheckHeader()
        {
            // Arrange
            var expectedHeader = "application/json";
            var httpClient = GetMockHttpClient(HttpStatusCode.OK);

            // Act
            var response = await httpClient.GetAsync(_apiUrl);
            var contentType = response.Content.Headers.ContentType?.MediaType;

            // Assert
            Assert.Equal(expectedHeader, contentType, StringComparer.OrdinalIgnoreCase);
        }

    }
}