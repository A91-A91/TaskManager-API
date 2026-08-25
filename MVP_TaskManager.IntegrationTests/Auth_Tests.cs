using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using MVP_TaskManager.DTO;
using System.Net.Http.Headers;

namespace MVP_TaskManager.IntegrationTests
{
    public class Auth_Tests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public Auth_Tests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact] // тестовый метод
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            var request = new
            {
                Login = "alice01",
                Password = "password123"
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Authorization/login",
                request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content // читаем ответ и преобразовываем
                                                // его в объект AuthResponseDTO
                .ReadFromJsonAsync<AuthResponseDTO>();

            Assert.NotNull(result); // проверяем, что результат не null
            Assert.False(string.IsNullOrEmpty(result!.Token));
            // получаем токен и проверяем, что он не пустой
        }

        [Fact]
        public async Task Login_WithInvalidCred_ReturnUnauth()
        {
            var request = new
            {
                Login = "WrongLogin",
                Password = "WrongPassword"
            };

            var response = await _client.PostAsJsonAsync(
                "/api/Authorization/login",
                request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetTask_WithoutJWT()
        {
            var response = await _client.GetAsync("/api/tasks");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_ByWrongRole_ReturnForbidden()
        {
            var request = new
            {
                Login = "alice01",
                Password = "password123"
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Authorization/login",
                request); //залогиниваемся

            var authResponse = await loginResponse.
                Content.ReadFromJsonAsync<AuthResponseDTO>(); //получаем токен

            _client.DefaultRequestHeaders.Authorization =
             new AuthenticationHeaderValue("Bearer", authResponse!.Token);
            //в заголовок запроса добавляем токен

            var response = await _client.GetAsync("/api/users");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateTask_ValidValues_ReternOK()
        {
            var request = new
            {
                Login = "alice01",
                Password = "password123"
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Authorization/login",
                request); //залогиниваемся

            var authResponse = await loginResponse.
                Content.ReadFromJsonAsync<AuthResponseDTO>(); //получаем токен

            _client.DefaultRequestHeaders.Authorization =
             new AuthenticationHeaderValue("Bearer", authResponse!.Token);
            //в заголовок запроса добавляем токен


            var query = await _client.PostAsJsonAsync(
                "/api/tasks",
                new
                {
                    Name = "Test Task",
                    Description = "Test Description",
                    IdStatus = 1
                });

            Assert.Equal(HttpStatusCode.OK, query.StatusCode);

        }

        [Fact]
        public async Task CreateTask_InvalidValues_ReturnBadRequest()
        {
            var request = new
            {
                Login = "alice01",
                Password = "password123"
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Authorization/login",
                request); //залогиниваемся

            var authResponse = await loginResponse.
                Content.ReadFromJsonAsync<AuthResponseDTO>(); //получаем токен

            _client.DefaultRequestHeaders.Authorization =
             new AuthenticationHeaderValue("Bearer", authResponse!.Token);
            //в заголовок запроса добавляем токен


            var query = await _client.PostAsJsonAsync(
                "/api/tasks",
                new
                {
                    Name = "",
                    Description = "Test Description",
                    IdStatus = 1
                });

            Assert.Equal(HttpStatusCode.BadRequest, query.StatusCode);

        }

        [Fact]
        public async Task GetTask_ValidValues_ReturnOK()
        {
            var request = new
            {
                Login = "alice01",
                Password = "password123"
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Authorization/login",
                request); //залогиниваемся

            var authResponse = await loginResponse.
                Content.ReadFromJsonAsync<AuthResponseDTO>(); //получаем токен

            _client.DefaultRequestHeaders.Authorization =
             new AuthenticationHeaderValue("Bearer", authResponse!.Token);
            //в заголовок запроса добавляем токен


            var query = await _client.GetAsync("/api/tasks/");
      
            Assert.Equal(HttpStatusCode.OK, query.StatusCode);

        }

        [Fact]
        public async Task GetAllUsers_WithoutToken_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task Admin_Can_GetAllUsers_ReturnsOk()
        {
            var loginRequest = new
            {
                Login = "U1",
                Password = "123"
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Authorization/login",
                loginRequest);

            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var authResponse = await loginResponse.Content
                .ReadFromJsonAsync<AuthResponseDTO>();

            Assert.NotNull(authResponse);
            Assert.False(string.IsNullOrEmpty(authResponse!.Token));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResponse.Token);

            var response = await _client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
