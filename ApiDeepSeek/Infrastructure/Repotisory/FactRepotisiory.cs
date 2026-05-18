using ApiDeepSeek.Common;
using ApiDeepSeek.Doamin.Interfais;
using ApiDeepSeek.Models;
using GroupApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiDeepSeek.Infrastructure.Repotisory
{
    public class FactRepotisiory : IFactRepotisiory
    {
        private readonly AppDbContext _context;
        public FactRepotisiory(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Answer>>> GetFact(string userid)
        {
            var answers = await _context.Answers
         .Where(a => a.UserId == userid)
         .ToListAsync(); 

            return Result<List<Answer>>.Ok(answers);
        }

        public async Task SaveFact(Answer answer)
        {
            answer.CreatedAt = DateTime.UtcNow;
            _context.Answers.Add(answer);
            
            _context.SaveChanges();
        }
    }
}
