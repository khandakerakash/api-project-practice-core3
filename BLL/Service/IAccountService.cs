﻿using System;
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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utility.Exceptions;
using Utility.Helpers;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BLL.Service
{
    public interface IAccountService
    {
        Task<string> Login(LoginRequest request);
        Task Test(ClaimsPrincipal cp);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(UserManager<AppUser> userManager, IConfiguration config, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Login(LoginRequest request)
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

        private async Task<string> GenerateJsonWebToken(AppUser userInfo)
        {
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
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],  
                _config["Jwt:Issuer"],  
                claims,  
                expires: DateTime.Now.AddMinutes(10),  
                signingCredentials: credentials);  
  
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }  
    }
}