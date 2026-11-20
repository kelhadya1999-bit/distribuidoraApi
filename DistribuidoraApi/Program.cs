using DistribuidoraApi.Data;
using Microsoft.EntityFrameworkCore;
using DistribuidoraApi.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DistribuidoraDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DistribuidoraConnectionString")));

builder.Services.AddScoped<IProductoRepository, SQLProductoRepository>();
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