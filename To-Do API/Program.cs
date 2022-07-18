using DatabaseConnection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Database Connections as Singletons
string configFilePath = "devSettings.json";
IConfiguration config = new ConfigurationBuilder().AddJsonFile(configFilePath)
    .Build();

var todoConnection = new ToDoSqlServerConnection(config.GetRequiredSection("ConnectionStrings").GetValue<String>("ToDoSqlServerConnection"));
builder.Services.AddSingleton(todoConnection);

var usersConnection = new AuthSqlServerConnection(config.GetRequiredSection("ConnectionStrings").GetValue<String>("ToDoUsersSqlServerConnection"));
builder.Services.AddSingleton(usersConnection);

builder.Configuration.AddConfiguration(config);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, 
    options => options.TokenValidationParameters = new TokenValidationParameters
    { 
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Auth:Jwt:Issuer"],
        ValidAudience = config["Auth:Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:Jwt:Key"]))
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo List with JWT",
        Description = "Using JWTs to authenticate and authorize my ToDo List"
    });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization using Bearer Scheme"
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
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
