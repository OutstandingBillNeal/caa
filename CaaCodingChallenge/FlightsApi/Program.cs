using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ardalis.GuardClauses;
using FlightsData;
using UnitsOfWork;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// adding jwt auth
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtKey = builder.Configuration["Jwt:Key"];
Guard.Against.Null(jwtIssuer);
Guard.Against.Null(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // define which claims should be checked
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IFlightsContext, FlightsContext>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateFlightValidator>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetFlightsHandler>();
});

var app = builder.Build();

// Configure these two as desired
var useSwagger = true || app.Environment.IsDevelopment();
var useExceptionHandler = true || !app.Environment.IsDevelopment();

if (useSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (useExceptionHandler)
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
