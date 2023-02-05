using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Event.Api.Filters;
using Event.Api.MappingProfiles;
using Event.Bll.MappingProfiles;
using Event.Bll.Options;
using Event.Bll.Services;
using Event.Bll.Services.Interfaces;
using Event.Dal.Repositories;
using Event.Dal.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var databaseConnectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString");

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    })
    .AddTransient<IEventUnitOfWork, EventUnitOfWork>()
    .AddTransient<IEventManager, EventManager>()
    .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
    .AddTransient<SecurityTokenHandler, JwtSecurityTokenHandler>()
    .AddTransient<IJwtTokenProvider, JwtTokenProvider>()
    .AddTransient<IUserService, UserService>()
    .AddDbContext<EventUnitOfWork>(optionsBuilder => optionsBuilder.UseNpgsql(databaseConnectionString));

builder.Services.AddAutoMapper(
    typeof(EventMappingProfile),
    typeof(OrganizerMappingProfile),
    typeof(SpeakerMappingProfile),
    typeof(EventMappingProfileApi),
    typeof(OrganizerMappingProfileApi),
    typeof(SpeakerMappingProfileApi));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(bearerOptions =>
    {
        var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        bearerOptions.SaveToken = true;

        bearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            ValidAlgorithms = jwtOptions.EncryptionAlgorithms
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
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