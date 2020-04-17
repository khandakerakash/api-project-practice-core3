using Microsoft.Extensions.DependencyInjection;
using Utility.Helpers;

namespace Utility
{
    public class UtilityDependency
    {
        public static void RegisterUtilityDependency(IServiceCollection services)
        {
            services.AddSingleton(typeof(TaposRSA));
        }
    }
}