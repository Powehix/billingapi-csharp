using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Services;
using BillingAPI.Infrastructure.Payments;
using BillingAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Billing API", Version = "v1" });

    // Add JWT Bearer Auth to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// DI: repositories
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<IReceiptRepository, InMemoryReceiptRepository>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

// DI: payment gateways + factory (Strategy + Factory)
builder.Services.AddSingleton<IPaymentGateway, PayPalGateway>();
builder.Services.AddSingleton<IPaymentGateway, StripeGateway>();
builder.Services.AddSingleton<IPaymentGatewayFactory, PaymentGatewayFactory>();

// DI: domain services
builder.Services.AddScoped<BillingService>();

// JWT auth
var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-secret-key-change-me";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "BillingApi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "BillingApiClients";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();