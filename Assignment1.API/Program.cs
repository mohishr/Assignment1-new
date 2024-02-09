using System.Reflection;
using Assignment1.API.Repositories;
using Assignment1.API.Services;
using System.Runtime;
using Assignment1.API;
using Assignment1.API.Settings;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>

{

    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlCommentFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);

    option.IncludeXmlComments(xmlCommentFilePath);

});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddSingleton<IMachineRepo, MongoMachineRepo>();
builder.Services.AddScoped<IMachineService, MachineService>();
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

