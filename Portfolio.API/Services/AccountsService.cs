﻿namespace Portfolio.API.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Contracts;
    using Portfolio.API.Services.Dtos;
    using Portfolio.API.Services.Dtos.AccountsDtos;
    using Portfolio.API.Services.Models;
    using Portfolio.Data.Repositories;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class AccountsService : IAccountsService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IGitHubApiService gitHubApiService;

        public AccountsService
            (
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IRepository<ApplicationUser> userRepository,
            IGitHubApiService gitHubApiService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.gitHubApiService = gitHubApiService;
        }

        public async Task<ApplicationUserLoginResponseDto> AuthenticateUserAsync(ApplicationUserLoginDto user)
        {
            //I don't need to check if the user is null because, in case they are, the boolean "IsAuthenticated" will automatically throw an exception for invalid email or password.

            var findUser = await userManager.FindByEmailAsync(user.Email);

            //var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(findUser);

            //if (!isEmailConfirmed)
            //{
            //    throw new InvalidOperationException(EMAIL_NOT_CONFIRMED);
            //}

            var isAuthenticated = await userManager.CheckPasswordAsync(findUser, user.Password);

            if (!isAuthenticated)
            {
                throw new MemberAccessException("Invalid email or password!");
            }

            await GenerateTokens(findUser);

            var authenticatedUser = new ApplicationUserLoginResponseDto()
            {
                Id = findUser.Id,
                Email = findUser.Email,
                Username = findUser.UserName,
                AccessToken = findUser.AccessToken,
                RefreshToken = findUser.RefreshToken
            };

            return authenticatedUser;
        }

        public async Task<IdentityResult> RegisterUserAsync(ApplicationUserRegisterDto applicationUser)
        {
            var user = new ApplicationUser()
            {
                UserName = applicationUser.UserName,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                JobTitle = applicationUser.JobTitle,
                Email = applicationUser.Email
            };

            bool isEmailTaken = await userRepository.AllAsNoTracking().AnyAsync(x => x.Email == user.Email);

            if (isEmailTaken == true)
            {
                throw new InvalidOperationException("Email address is already taken.");
            }

            await gitHubApiService.HasUserAccountInGitHubAsync(user.UserName);

            var registeredUser = await userManager.CreateAsync(user, applicationUser.Password);

            return registeredUser;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<ApplicationUserTokensDto> RefreshAccessTokenAsync(string refreshToken, string userId)
        {
            //var decodedUserId = dataProtector.Unprotect(userId);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NullReferenceException("The user is not found.");
            }

            if (refreshToken != user.RefreshToken || DateTime.UtcNow > user.RefreshTokenExpirationDate)
            {
                throw new MemberAccessException("Wrong credentials");
            }

            await GenerateTokens(user);

            var tokensResponse = new ApplicationUserTokensDto()
            {
                AccessToken = user.AccessToken,
                RefreshToken = user.RefreshToken,
            };

            return tokensResponse;
        }
        public async Task<ApplicationUserDto> GetUserAsync(string userId)
        {
            //var unprotectedUserId = dataProtector.Unprotect(userId);
            var user = await userManager.FindByIdAsync(userId);

            var resultUser = new ApplicationUserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JobTitle = user.JobTitle
            };

            return resultUser;
        }
        private async Task GenerateTokens(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["JWTKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.UtcNow,

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.AccessToken = tokenHandler.WriteToken(token);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpirationDate = DateTime.UtcNow.AddSeconds(30);
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
