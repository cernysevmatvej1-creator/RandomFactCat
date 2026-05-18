using ApiDeepSeek.Common;
using ApiDeepSeek.Doamin.Efenties_Models;

namespace ApiDeepSeek.Doamin.Interfais
{
    public interface ITokenRepotisiory
    {
        Task<Result> SaveRefreshnToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshToken(string UserId);
        Task<Result<Role>> GetRoleId();
        Task<string> GetRole(int id);
    }
}
