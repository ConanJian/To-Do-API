var builder = WebApplication.CreateBuilder(args);

//Note: Create Custom Configuration Class that inherits from IConfiguration and uses it

string configFilePath = "devSettings.json";
IConfiguration config = new ConfigurationBuilder().AddJsonFile(configFilePath)
    .Build();

builder.Configuration.AddConfiguration(config);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
