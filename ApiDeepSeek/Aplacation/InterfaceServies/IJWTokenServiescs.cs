using ApiDeepSeek.Common;
using ApiDeepSeek.Doamin.Efenties_Models;
using ApiDeepSeek.models;

namespace ApiDeepSeek.Aplacation.InterfaceServies
{
    public interface IJWTokenServiescs
    {
        Task<AutrResult> SignAnonimal(User user);
        Task<Result<AutrResult>> GetNewToken(string refreshToken); 
    }
}
