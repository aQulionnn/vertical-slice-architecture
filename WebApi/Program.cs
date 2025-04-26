using Carter;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interceptors;
using WebApi.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddMediatR(configuration =>
{
    var assembly = typeof(Program).Assembly;
    configuration.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("Database");
    options.AddInterceptors(new SqlLoggingInterceptor());
});

builder.Services.AddControllers();
builder.Services.AddFastEndpoints();

HistoricalEventMapper.Register();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseFastEndpoints();

app.Run();