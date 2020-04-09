using System.Threading.Tasks;
using DLL.Model;
using Microsoft.AspNetCore.Identity;

namespace BLL.Service
{
    public interface ITestService
    {
        Task SaveTestData();
    }

    public class TestService : ITestService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public TestService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
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
                var role = await _roleManager.FindByNameAsync("customer");
            
                if (role == null)
                {
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = "customer"
                    });
                }
            
                await _userManager.AddToRoleAsync(user, "customer");
            }
        }
    }
}