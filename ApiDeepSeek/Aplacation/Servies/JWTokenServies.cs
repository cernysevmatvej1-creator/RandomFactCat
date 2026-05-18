using ApiDeepSeek.Aplacation.InterfaceServies;
using ApiDeepSeek.Common;
using ApiDeepSeek.Doamin.Efenties_Models;
using ApiDeepSeek.Doamin.Interfais;
using ApiDeepSeek.Infrastructure.Repotisory;
using ApiDeepSeek.models;
using GroupApi.Controllers;
using GroupApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiDeepSeek.Doamin.Efenties_Models.ApiDeepSeek.Doamin.Efenties_Models;
using ApiDeepSeek.Migrations;
namespace ApiDeepSeek.Aplacation.Servies
{
    public class JWTokenServies : IJWTokenServiescs
    {
        private readonly IConfiguration _configuration;
        private IUserRepotisory _userRepotisory;
        private ITokenRepotisiory _tokenRepotisiory;
        private readonly AppDbContext _context;
        public JWTokenServies(IConfiguration configuration,IUserRepotisory userRepotisory,ITokenRepotisiory tokenRepotisiory,AppDbContext appDbContext)
        {
            _configuration = configuration;
            _userRepotisory = userRepotisory;
            _tokenRepotisiory = tokenRepotisiory;
            _context = appDbContext;
        }

        // Потом в методе:
     
        public async Task<AutrResult> SignAnonimal(User user)
        {
            var listroles = await _tokenRepotisiory.GetRoleId();
            if (!listroles.Success)
                return new AutrResult { };
            var existingUser = await _userRepotisory.GetUserId(user.UserName);

            if (existingUser.Data == null)
            {
                // первый раз - генерируем новый id и сохраняем
                user.UserId = Guid.NewGuid().ToString();
                await _userRepotisory.AddUser(user);
                _userRepotisory.SaveUserRoles(new UserRole
                {
                    UserId = user.UserId,
                    RoleId = listroles.Data.Id
                });

            }
            else
            {
                
                user.UserId = existingUser.Data.UserId;
            }
            var jwtKey = _configuration["Jwt:Key"];
            RefreshToken refreshToken = new RefreshToken()
            {
                UserId = user.UserId,
                Expires = DateTime.UtcNow.AddDays(30),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                IsRevoked = false
            };

            
            var rolePermissions = await _context.RolePermissions
                .Where(x => x.RoleId == listroles.Data.Id) 
                .ToListAsync();

            var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

            
            var permissions = await _context.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .ToListAsync();
            await _tokenRepotisiory.SaveRefreshnToken(refreshToken);  
            var claimsList = new List<Claim>
{
    new Claim("userId", user.UserId),
    new Claim("username", user.UserName),
    new Claim(ClaimTypes.Role, listroles.Data.Name)
};

    
            foreach (var permission in permissions)
            {
                claimsList.Add(new Claim("permission", permission.Name));
               _context.UserPermissions.Add(new UserPermission { UserId  =  user.UserId, PermissionId = permission.Id });
            }

            
            var claims = claimsList.ToArray();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(15),
    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
); return new AutrResult { AssetsToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), RefreshToken = refreshToken.Token };
          

        }
        public async Task<Result<AutrResult>> GetNewToken(string refreshToken)
        {
            if (refreshToken == null)
                return Result<AutrResult>.Fail("Пустой токен");

            var token = await _tokenRepotisiory.GetRefreshToken(refreshToken);

            if (token == null )
                return Result<AutrResult>.Fail("Токен недействителен");

            var user = await _userRepotisory.GetUser(token.UserId);

            var userRoles = await _userRepotisory.FirstUserRoles(user.Data.UserId);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

            
            var rolePermissions = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            var userPermissions = await _context.UserPermissions
                .Where(up => up.UserId == user.Data.UserId)
                .Select(up => up.PermissionId)
                .ToListAsync();

            
            var allPermissionIds = rolePermissions
                .Union(userPermissions)
                .Distinct()
                .ToList();

       
            var permissions = await _context.Permissions
                .Where(p => allPermissionIds.Contains(p.Id))
                .ToListAsync();


            var claims = new List<Claim>
    {
        new Claim("userId", user.Data.UserId),
        new Claim("username", user.Data.UserName)
    };

            foreach (var ur in userRoles)
            {
                var roleName = await _tokenRepotisiory.GetRole(ur.RoleId);
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

           
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission.Name));
            }

            var jwtKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var jwtToken = new JwtSecurityToken(
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Result<AutrResult>.Ok(new AutrResult
            {
                AssetsToken = newAccessToken,
            });
        }

    }
}
