using ApiDeepSeek.Infrastructure.Reqments;
using GroupApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ApiDeepSeek.Infrastructure.Handlers
{
    public class FactOwnerHandler : AuthorizationHandler<FactOwnerRequirement>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FactOwnerHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            FactOwnerRequirement requirement)
        {
            var userId = context.User.FindFirst("userId")?.Value;
            var factId = _httpContextAccessor.HttpContext?.Request.RouteValues["id"]?.ToString();
            if (!int.TryParse(factId, out int id))
                return;
            var check = await  _context.Answers.FindAsync(id); ;
            if (check.UserId == userId) {
                context.Succeed(requirement);
            }

            

           
        }
    }
}
