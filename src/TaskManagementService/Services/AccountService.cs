using IdentityModel.Client;
using Lionize.TaskManagement.ApiModels;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Services
{
    public class AccountService : IAccountService
    {
        public async Task<SignInResponse> SignInAsync(string username, string password, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = "http://localhost:8081/connect/token",

                ClientId = "client",
                ClientSecret = "secret",
                //Scope = "api1",

                UserName = username,
                Password = password
            });

            return new SignInResponse
            {
                IsError = response.IsError,
                Error = response.Error,
                AccessToken = response.AccessToken,
                IdentityToken = response.IdentityToken,
                RefreshToken = response.RefreshToken,
                TokenType = response.TokenType
            };
        }
    }
}