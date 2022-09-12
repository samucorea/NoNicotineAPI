using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoNicotine_Data.Context;
using NoNicotineAPI;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//MediatR
builder.Services.AddMediatR(typeof(Program));

string sqlServerConnectionString = builder.Configuration.GetConnectionString("local");
//builder.Services.AddDbContext<NoNicotineContext>(opt => opt.UseSqlServer(sqlServerConnectionString));
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer("Data Source=localhost;Initial Catalog=test1;Integrated Security=True"));


var logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: sqlServerConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "LogEvents",
                        AutoCreateSqlTable = true
                    }
                    )
                .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

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
