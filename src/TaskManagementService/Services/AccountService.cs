using IdentityModel.Client;
using Lionize.TaskManagement.ApiModels.V1;
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
        private readonly IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions;

        public AccountService(IOptions<AccountOptions> accountOptions, IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions)
        {
            this.accountOptions = accountOptions ?? throw new ArgumentNullException(nameof(accountOptions));
            this.serviceDiscoveryOptions = serviceDiscoveryOptions ?? throw new ArgumentNullException(nameof(serviceDiscoveryOptions));
        }

        public async Task<SignInResponse> SignInAsync(string username, string password, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"{serviceDiscoveryOptions.Value.Identity.BaseAddress}/connect/token",

                ClientId = accountOptions.Value.ClientId,
                ClientSecret = accountOptions.Value.ClientSecret,

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