using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace API.Policy
{
    public class TokenPolicyHandler : AuthorizationHandler<TokenPolicy>
    {
        private readonly IDistributedCache _cache;

        public TokenPolicyHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenPolicy requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            if(userId == null)
                throw new UnauthorizedAccessException();
            
            var accessTokenKey = userId.ToString() + "_accesstoken";
            var cashAccessToken = _cache.GetString(accessTokenKey);
            
            if(cashAccessToken == null)
                throw new UnauthorizedAccessException();
            context.Succeed(requirement); 
            
            return Task.CompletedTask;
        }
    }
}