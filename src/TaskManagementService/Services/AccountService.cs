using IdentityModel.Client;
using Lionize.TaskManagement.ApiModels.V1;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
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

        public async Task<RefreshTokenResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.RequestRefreshTokenAsync(new IdentityModel.Client.RefreshTokenRequest
            {
                Address = $"{serviceDiscoveryOptions.Value.Identity.BaseAddress}/connect/token",

                ClientId = accountOptions.Value.ClientId,
                ClientSecret = accountOptions.Value.ClientSecret,

                RefreshToken = refreshToken
            }).ConfigureAwait(false);

            return new RefreshTokenResponse
            {
                IsError = response.IsError,
                ErrorMessage = response.Error,
                AccessToken = response.AccessToken,
                IdentityToken = response.IdentityToken,
                RefreshToken = response.RefreshToken,
                TokenType = response.TokenType,
                DisplayName = await GetDisplayNameAsync(response.AccessToken, cancellationToken)
            };
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
            }).ConfigureAwait(false);

            return new SignInResponse
            {
                IsError = response.IsError,
                ErrorMessage = response.Error,
                AccessToken = response.AccessToken,
                IdentityToken = response.IdentityToken,
                RefreshToken = response.RefreshToken,
                TokenType = response.TokenType,
                DisplayName = await GetDisplayNameAsync(response.AccessToken, cancellationToken)
            };
        }

        public async Task<SignOutResponse> SignOutAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = $"{serviceDiscoveryOptions.Value.Identity.BaseAddress}/connect/revocation",

                ClientId = accountOptions.Value.ClientId,
                ClientSecret = accountOptions.Value.ClientSecret,

                Token = refreshToken
            }).ConfigureAwait(false);

            return new SignOutResponse
            {
                IsError = response.IsError,
                ErrorMessage = response.Error,
            };
        }

        private async Task<string> GetDisplayNameAsync(string accessToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return null;

            var client = new HttpClient();
            var userInfo = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = $"{serviceDiscoveryOptions.Value.Identity.BaseAddress}/connect/userinfo",

                Token = accessToken,

                ClientId = accountOptions.Value.ClientId,
                ClientSecret = accountOptions.Value.ClientSecret,
            }, cancellationToken).ConfigureAwait(false);

            return userInfo.TryGet("preferred_username");
        }
    }
}