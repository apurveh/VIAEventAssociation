using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Application.CommandDispatching.Dispatcher;
using ViaEventAssociation.Core.Application.Extensions;
using ViaEventAssociation.Core.Application.Features.Dispatcher;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Infrastructure.EfcQueries;
using ViaEventAssociation.Infrastructure.EfcQueries.Extensions;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<DmContext>(options =>
    options.UseSqlite(
        @"Data Source = C:\VIA University\Semester 6\DCA1\ViaEventAssociation\src\Infrastructure\ViaEventAssociation.Infrastructure.EfDmPersistence\DBProduction.db"));

builder.Services.AddScoped<IUnitOfWork, SqliteUnitOfWork>();

builder.Services.AddScoped<IEventRepository, EventEfRepository>();
builder.Services.AddScoped<IGuestRepository, GuestEfRepository>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();

builder.Services.AddScoped<DbproductionContext>(provider => DbproductionContext.SetupContext());
builder.Services.AddScoped<IMapper, ConcreteObjectMapper>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

builder.Services.AddCommandHandlers();
builder.Services.AddQueryHandlers();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}