using IdentityModel.Client;
using Lionize.TaskManagement.ApiModels;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Options;

namespace TIKSN.Lionize.TaskManagementService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IOptions<AccountOptions> accountOptions;

        public AccountService(IOptions<AccountOptions> accountOptions)
        {
            this.accountOptions = accountOptions ?? throw new ArgumentNullException(nameof(accountOptions));
        }

        public async Task<SignInResponse> SignInAsync(string username, string password, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = "http://localhost:8081/connect/token",

                ClientId = accountOptions.Value.ClientId,
                ClientSecret = accountOptions.Value.ClientSecret,
                //Scope = "",

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