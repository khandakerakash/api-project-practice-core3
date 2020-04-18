using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Utility.Exceptions;
using Utility.Helpers;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BLL.Service
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task Test(ClaimsPrincipal cp);
        Task<ApiSuccessResponse> Logout(ClaimsPrincipal cp);
        Task<LoginResponse> RefreshToken(string refreshToken);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TaposRSA _taposRsa;
        private readonly IDistributedCache _cache;

        public AccountService(UserManager<AppUser> userManager, IConfiguration config, IUnitOfWork unitOfWork, TaposRSA taposRsa, IDistributedCache cache)
        {
            _userManager = userManager;
            _config = config;
            _unitOfWork = unitOfWork;
            _taposRsa = taposRsa;
            _cache = cache;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if(user == null)
                throw new MyAppException("The user not found!");

            var matchUser = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!matchUser)
                throw new MyAppException("The username and password don't match!");

            return await GenerateJsonWebToken(user);
        }

        public async Task Test(ClaimsPrincipal cp)
        {
            var userId = cp.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var userName = cp.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var email = cp.Claims.FirstOrDefault(c => c.Type == "emailaddress")?.Value;
            var role = cp.FindFirst(ClaimTypes.Role)?.Value;
            var tst = cp.Claims.FirstOrDefault(c => c.Type == "testtt")?.Value;

            var student = new Student()
            {
                Name = "Md. Emon",
                Email = "emon1234@gmail.com",
                RollNo = "CSE-22"
            };

            await _unitOfWork.StudentRepository.CreateAsync(student);
            await _unitOfWork.AppSaveChangesAsync();
            
            throw new MyAppException("Something Went Wrong!");
        }

        public async Task<ApiSuccessResponse> Logout(ClaimsPrincipal cp)
        {
            var userId = cp.Claims.FirstOrDefault(x => x.Type == "userId");
            
            var accessTokenKey = userId + "_accesstoken";
            var refreshTokenKey = userId + "_refreshtoken";
            
            await _cache.RemoveAsync(accessTokenKey);
            await _cache.RemoveAsync(refreshTokenKey);
            
            // if (accessTokenKey != null || refreshTokenKey != null)
            //     throw new MyAppException("Something went wrong");
            
            return new ApiSuccessResponse()
            {
                StatusCode = 200,
                Message = "Logout is done successfully."
            };
        }

        public async Task<LoginResponse> RefreshToken(string refreshToken)
        {
            var decryptRsa = _taposRsa.Decrypt(refreshToken, "v1");
            if(decryptRsa == null)
                throw new MyAppException("Refresh token is not found");

            var refreshTokenObject = JsonConvert.DeserializeObject<RefreshTokenResponse>(decryptRsa);
            var refreshTokenKey = refreshTokenObject.UserId + "_refreshtoken";
            
            var cashData = await _cache.GetStringAsync(refreshTokenKey);
            if(cashData == null || cashData != refreshToken)
                throw new MyAppException("Refresh token is not found");

            var user = await _userManager.FindByIdAsync(refreshTokenObject.UserId.ToString());
            return await GenerateJsonWebToken(user);
        }

        private async Task<LoginResponse> GenerateJsonWebToken(AppUser userInfo)
        {
            var response = new LoginResponse();
            var userRole = (await _userManager.GetRolesAsync(userInfo)).FirstOrDefault();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Id.ToString()),
                new Claim(CustomJwtClaimNames.UserId, userInfo.Id.ToString()),
                new Claim(CustomJwtClaimNames.UserName, userInfo.UserName ?? ""),
                new Claim(CustomJwtClaimNames.Email, userInfo.Email ?? ""),
                new Claim(ClaimTypes.Role, userRole)
            };
            var times = _config.GetValue<int>("Jwt:AccessTokenLifeTime");
            
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],  
                _config["Jwt:Issuer"],  
                claims,  
                expires: DateTime.Now.AddMinutes(times),  
                signingCredentials: credentials);  
            
            var refreshToken = new RefreshTokenResponse()
            {
                UserId = userInfo.Id,
                Id = Guid.NewGuid().ToString()
            };
            var rsaData = _taposRsa.EncryptData(JsonConvert.SerializeObject(refreshToken), "v1");
            
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.Expired = times * 60;
            response.RefreshToken = rsaData;

            await StoreTokenInfo(userInfo.Id, response.Token, response.RefreshToken);
            return response;
        }

        private async Task StoreTokenInfo(long userId, string accessToken, string refreshToken)
        {
            var accessTokenOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Jwt:AccessTokenLifeTime")));
            
            var refreshTokenOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Jwt:RefreshTokenLifeTime")));
            
            var accessTokenKey = userId.ToString() + "_accesstoken";
            var refreshTokenKey = userId.ToString() + "_refreshtoken";
            
            await _cache.SetStringAsync(accessTokenKey, accessToken, accessTokenOptions);
            await _cache.SetStringAsync(refreshTokenKey, refreshToken, refreshTokenOptions);
        }
    }
}