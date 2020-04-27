using System;
using System.Threading.Tasks;
using DLL.Model;
using DLL.UnitOfWorks;
using Microsoft.AspNetCore.Identity;

namespace BLL.Service
{
    public interface ITestService
    {
        Task SaveTestData();
        Task UpdateCustomerBalanceTest();
    }

    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public TestService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SaveTestData()
        {
            var user = new AppUser()
            {
                UserName = "akash007",
                Email = "akash.cse10@gmail.com"
            };
            
            var result = await _userManager.CreateAsync(user, "Akash123$..");
            
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("staff");
            
                if (role == null)
                {
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = "staff"
                    });
                }
            
                await _userManager.AddToRoleAsync(user, "staff");
            }
        }

        public async Task UpdateCustomerBalanceTest()
        {
            Random rnd = new Random();
            int myNumber = rnd.Next(1, 100);
            
            Order order = new Order()
            {
                Amount = myNumber
            };

            await _unitOfWork.OrderRepository.CreateAsync(order);
            
            if (await _unitOfWork.AppSaveChangesAsync())
                await _unitOfWork.CustomerBalanceRepository.UpdateCustomerBalanceAsync(myNumber);
        }
    }
}