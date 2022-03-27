var builder = WebApplication.CreateBuilder(args);

//Create Custom Configuration

string configFilePath = ".\\devSettings.json";
IConfiguration config = new ConfigurationBuilder().AddJsonFile(configFilePath)
    .Build();

string valueInConfigFile = config.GetRequiredSection("RandomText").GetValue<string>("Text1");
//string baseDirectory = AppContext.BaseDirectory;
//Console.WriteLine(valueInConfigFile + "\n" + baseDirectory);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(config);

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
