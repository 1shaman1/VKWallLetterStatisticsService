using Microsoft.EntityFrameworkCore;
using UDV_VK_Test_task.DAL;
using UDV_VK_Test_task.Services.VkAPIService;
using UDV_VK_Test_task.Services.VkAPIService.Interfaces;
using UDV_VK_Test_task.Services.WallService;
using UDV_VK_Test_task.Services.WallService.Interfaces;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Logging.ClearProviders();



builder.Services.AddControllers();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<VKWallDbContext>(opt => opt.UseNpgsql(connection));
builder.Services.AddTransient<IVkAPIService, VkAPIService>();
builder.Services.AddTransient<IWallStatisticsService, WallStatisticsService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
