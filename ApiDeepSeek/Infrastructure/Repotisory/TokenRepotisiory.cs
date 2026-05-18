using ApiDeepSeek.Common;
using ApiDeepSeek.Doamin.Efenties_Models;
using ApiDeepSeek.Doamin.Interfais;
using GroupApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiDeepSeek.Infrastructure.Repotisory
{
    public class TokenRepotisiory : ITokenRepotisiory
    {
        private readonly AppDbContext _context;
        public TokenRepotisiory(AppDbContext context)
        {
            _context = context;
        }
       
        public async Task<RefreshToken> GetRefreshToken(string UserId)
        {
            var token  =  await  _context.RTokens.FirstOrDefaultAsync(x => x.Token == UserId);
            return token;

        }
        public async Task<string> GetRole(int id)
        {
            var tole = await  _context.Roles.FirstOrDefaultAsync(X => X.Id == id);
            return tole.Name;
        }
        public async Task<Result<Role>> GetRoleId()
        {
            try
            {
                var check = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "User");
                return Result<Role>.Ok(check);
            }
            catch (Exception ex) {
                return Result<Role>.Fail(ex.Message);
            }
       
        }
       

        public async Task<Result> SaveRefreshnToken(RefreshToken refreshToken)
        {
            try
            {
                _context.RTokens.Add(refreshToken); 
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex) {
                return Result.Fail(ex.Message);
            }

        }
       
    }
}
