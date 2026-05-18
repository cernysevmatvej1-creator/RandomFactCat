using ApiDeepSeek.Common;
using ApiDeepSeek.Doamin.Efenties_Models.ApiDeepSeek.Doamin.Efenties_Models;
using ApiDeepSeek.Doamin.Interfais;
using ApiDeepSeek.models;
using GroupApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiDeepSeek.Infrastructure.Repotisory
{
    public class UserRepotisiory : IUserRepotisory
    {
        private readonly AppDbContext _context;
        public UserRepotisiory(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return Result.Ok(); 
            }
            catch (Exception ex) {
                return Result.Fail(ex.Message);
            }
            
        }

        public async Task<Result<User>> GetUserId(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            return Result<User>.Ok(user);
        }
        public async Task <Result<User>> GetUser(string userId)
        {
            var user = await _context.Users
              .FirstOrDefaultAsync(u => u.UserId == userId);

            return Result<User>.Ok(user);
        }
        public void SaveUserRoles(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            _context.SaveChanges(); 
        }
        public async Task<List<UserRole>> FirstUserRoles(string userId) 
        {
            return await _context.UserRoles
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }

    }
}
