using ApiDeepSeek.Infrastructure.Reqments;
using ApiDeepSeek.Infrastructure.Reqments;
using ApiDeepSeek.Models;
using GroupApi.Data;
using GroupApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ApiDeepSeek.Infrastructure.Handlers
{
    public class FactEditHandler : AuthorizationHandler<FactEditReqments, Answer>  
    {
      
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            FactEditReqments requirement,
            Answer resource)  
        {
            var hasEditFact = context.User.HasClaim("permission", "edit_fact");
            if (!hasEditFact)
                return Task.CompletedTask;

           
            var userId = context.User.FindFirst("userId")?.Value;
            if (resource.UserId != userId)
                return Task.CompletedTask;

            var hoursSinceCreated = (DateTime.UtcNow - resource.CreatedAt).TotalHours;
            if (hoursSinceCreated > 24)  
                return Task.CompletedTask;

         
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
