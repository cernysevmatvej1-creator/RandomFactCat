using ApiDeepSeek.Common;
using ApiDeepSeek
    .Models;

namespace ApiDeepSeek.Doamin.Interfais
{
    public interface IFactRepotisiory
    {
        Task SaveFact(Answer answer);
        Task<Result<List<Answer>>> GetFact(string userid);
    }
}
