
using System.Net.Http.Headers;
using manage_auth.src.models;
using manage_auth.src.models.requests;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1;

namespace manage_auth.src.clients
{
    public class UsersClient : IUsersClient
    {
        private IConfiguration _configuration;
        private readonly HttpClient _client;

        public UsersClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetConnectionString("ManageUsersLocalConnection"))
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> CreateUser(CreateUserRequest createUserRequest, string bearerToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.ToString());
            HttpResponseMessage response = await _client.PostAsJsonAsync<CreateUserRequest>($"/api/Users", createUserRequest);

            if (response.IsSuccessStatusCode)
            {
                // var responseData = await response.Content.ReadAsStringAsync();
                // return responseData;
                return response;
            }
            else 
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}