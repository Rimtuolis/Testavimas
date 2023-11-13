using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PSA.ClientTests
{
    public static class MockHttpClientBunitHelpers
    {
        public static MockHttpMessageHandler AddMockHttpClient(this TestServiceProvider services)
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            var httpClient = mockHttpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");
            services.AddSingleton<HttpClient>(httpClient);
            return mockHttpHandler;
        }

        public static MockedRequest RespondJson<T>(this MockedRequest request, T content)
        {
            StringContent bybys = new StringContent("");
            StringContent bybys2 = new StringContent("");
            request.Respond(req =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                bybys = new StringContent(JsonConvert.SerializeObject(content, settings));
                bybys2 = new StringContent(JsonSerializer.Serialize(content));
                response.Content = bybys2;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            });
            return request;
        }

        public static MockedRequest RespondJson<T>(this MockedRequest request, Func<T> contentProvider)
        {
            request.Respond(req =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                response.Content = new StringContent(JsonConvert.SerializeObject(contentProvider(), settings));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            });
            return request;
        }
    }
}
