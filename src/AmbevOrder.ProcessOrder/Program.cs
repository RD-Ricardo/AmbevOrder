using AmbevOrder.ProcessOrder.Consumers;
using AmbevOrder.ProcessOrder.Data;
using AmbevOrder.ProcessOrder.Repositories;
using AmbevOrder.ProcessOrder.Services;
using AmbevOrder.ProcessOrder.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProcessOrderService, ProcessOrderService>();
builder.Services.AddHostedService<ProcessOrderConsumer>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITransaction, AmbevOrderProcessOrderDbContext>();
builder.Services.AddDbContext<AmbevOrderProcessOrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionDatabase"]));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
