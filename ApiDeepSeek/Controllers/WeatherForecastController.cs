using ApiDeepSeek.Aplacation.InterfaceServies;
using ApiDeepSeek.Doamin.Efenties_Models;
using ApiDeepSeek.models;
using ApiDeepSeek.Models;
using GroupApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IFactService _factService;
        private IJWTokenServiescs _jWTokenServiescs;
        private readonly IAuthorizationService _authorizationService;
        public AnswersController(AppDbContext context, IFactService factService, IJWTokenServiescs jWTokenServiescs,IAuthorizationService authorizationService)
        {
            _context = context;
            _factService = factService;
            _jWTokenServiescs = jWTokenServiescs;
            _authorizationService = authorizationService;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> NewRefresh([FromBody] AutrResult autrResult)
        {
            var check = await _jWTokenServiescs.GetNewToken(autrResult.RefreshToken);
            return Ok(check.Data);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var check = await _jWTokenServiescs.SignAnonimal(user);
            return Ok(check);
        }
        [Authorize(Roles = "User")]
        [HttpPost("save-otvet")]
        public async Task<IActionResult> Create([FromBody] Answer answer)
        {
            var userId = User.FindFirst("userId")?.Value;
            answer.UserId = userId;
            await _factService.FactSave(answer);
            return Ok(answer);
        }
        [Authorize]
        [HttpGet("facts-user")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirst("userId")?.Value;
            var answers = await _factService.GetAnswers(userId);
            return Ok(answers);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-test")]
        public IActionResult TestAdmin()
        {
            return Ok("Ты админ!");
        }
        [Authorize(Policy = "CatDeleteFact")]
        [HttpDelete("factdelete/{id}")]
        public async Task<IActionResult> DeleteFact(int id)
        {
            var fact = await _context.Answers.FindAsync(id);

            
            _context.Answers.Remove(fact);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Факт удалён" });
        }

        [HttpPost("edit-fact/{id}")]
        public async Task<IActionResult> EditFact(int id, [FromBody] string newText)
        {
            // 1. Загружаем ресурс (факт) из БД
            var fact = await _context.Answers.FindAsync(id);
            if (fact == null)
                return NotFound($"Факт с id {id} не найден");

            // 2. Вызываем авторизацию вручную, передавая ресурс
            var authResult = await _authorizationService.AuthorizeAsync(
                User,           // ClaimsPrincipal (из HttpContext)
                fact,           // Ресурс (объект Answer)
                "CatEditFact"   // Имя политики
            );

            // 3. Если авторизация не пройдена — 403 Forbidden
            if (!authResult.Succeeded)
                return Forbid();

            // 4. Редактируем факт
            fact.Text = newText;
            await _context.SaveChangesAsync();

            return Ok(fact);
        }


        [Authorize]
        [HttpPost("pay-prime")]
        public async Task<IActionResult> PayPrime([FromBody] AutrResult autr)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId == null)
                return Unauthorized();

           var check2 = await  _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (check2 == null)
                return NotFound();
            var check = await _context.Permissions.FirstOrDefaultAsync(x => x.Name == "edit_fact");
            _context.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = check.Id });
            await _context.SaveChangesAsync();
          var checkone = await _jWTokenServiescs.GetNewToken(autr.RefreshToken);
            if(checkone.Data != null )
               return Ok(checkone.Data.AssetsToken);
            else 
                return BadRequest(checkone.Message);    
        }
       
    }
}