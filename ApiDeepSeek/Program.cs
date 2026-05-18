
using ApiDeepSeek.Aplacation.InterfaceServies;
using ApiDeepSeek.Aplacation.Servies;
using ApiDeepSeek.Doamin.Interfais;
using ApiDeepSeek.Infrastructure.Handlers;
using ApiDeepSeek.Infrastructure.Repotisory;
using ApiDeepSeek.Infrastructure.Reqments;
using GroupApi.Controllers;
using GroupApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFactRepotisiory, FactRepotisiory>();
builder.Services.AddScoped<IUserRepotisory, UserRepotisiory>();
builder.Services.AddScoped<IFactService, FactServicecs>();
builder.Services.AddScoped<IJWTokenServiescs, JWTokenServies>();
builder.Services.AddScoped<ITokenRepotisiory,TokenRepotisiory>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler, FactOwnerHandler>();
builder.Services.AddScoped<IAuthorizationHandler, FactEditHandler>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CatDeleteFact", policy =>
        policy.Requirements.Add(new FactOwnerRequirement()));

    options.AddPolicy("CatEditFact", policy =>
        policy.Requirements.Add(new FactEditReqments()));
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? "dGhpcyBpcyBteSBzdXBlciBzZWNyZXQga2V5IGZvciBKd3QhISEhISEhISEh";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.WebHost.UseUrls("http://0.0.0.0:5261");
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
