using Microsoft.EntityFrameworkCore;
using Npgsql;
using Harkh_backend.src.Databases;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.Repositories;
using Harkh_backend.src.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using Harkh_backend.src.Enums;
using Harkh_backend.src.UnitOfWork;
using Harkh_app_production.src.Abstractions;
using Harkh_app_production.src.middleware;

var builder = WebApplication.CreateBuilder(args);


var _config = builder.Configuration;
var dataSourceBuilder = new NpgsqlDataSourceBuilder(@$"Host={_config["Db_Host"]};Username={_config["Db_Username"]};Database={_config["Db_Database"]};Password={_config["Db_Password"]};Port={_config["Db_Port"]}");
dataSourceBuilder.MapEnum<Status>();
dataSourceBuilder.MapEnum<Role>();
dataSourceBuilder.MapEnum<Priority>();
dataSourceBuilder.MapEnum<ProjectStatus>();

var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<DatabaseContext>((options) =>
{
    options.UseNpgsql(dataSource).UseSnakeCaseNamingConvention();
});

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
     options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Bearer token authentication",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        }
        );


        options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "API Key Authentication",
            Name = "X-Api-Key",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });


        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);

builder.Services.AddAutoMapper(typeof(Program).Assembly); // Find the mapper who inherited  from Profile(It built in class in .NET)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<DatabaseContext>();// inject the database context 

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(builder.Configuration["Cors_Origin"]!)
                          .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed((host) => true)
                            .AllowCredentials();
                      });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt_Issuer"],
            ValidAudience = builder.Configuration["Jwt_Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt_SigningKey"]!))
        };
    }
);


builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IDocumentService, DocumentService>();

builder.Services.AddScoped<IExperienceService, ExperienceService>();

builder.Services.AddScoped<IEducationService, EducationService>();

builder.Services.AddScoped<ISkillService, SkillService>();

builder.Services.AddScoped<IUserProjectService, UserProjectService>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserSkillService, UserSkillService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();


builder.Services.AddScoped<IMilestoneRepository, MilestoneRepository>();
builder.Services.AddScoped<IMilestoneService, MilestoneService>();

builder.Services.AddScoped<IInvitationService, InvitationService>();

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();// Authentication first then Authorization
app.UseAuthorization();


app.Run();