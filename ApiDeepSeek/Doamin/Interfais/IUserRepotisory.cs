using ApiDeepSeek.Common;
using ApiDeepSeek.Doamin.Efenties_Models.ApiDeepSeek.Doamin.Efenties_Models;
using ApiDeepSeek.models;

namespace ApiDeepSeek.Doamin.Interfais
{
    public interface IUserRepotisory
    {
        Task<Result<User>> GetUserId(string username);
        Task<Result> AddUser(User user);
        Task<Result<User>> GetUser(string userId);
        void SaveUserRoles(UserRole userRole);
        Task<List<UserRole>> FirstUserRoles(string userId);
    
    }
}
