using System.Reflection;
using Basket.API.Repositories;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddStackExchangeRedisCache(
    opt =>
    {
        opt.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    }
    );


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
// EventBus
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
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

app.UseAuthorization();

app.MapControllers();

app.Run();