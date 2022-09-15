using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoNicotin_Business.Commands;
using NoNicotine_Data.Context;
using NoNicotineAPI;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly(),
    typeof(CreatePatientCommand).Assembly);

string sqlServerConnectionString = builder.Configuration.GetConnectionString("local");
//builder.Services.AddDbContext<NoNicotineContext>(opt => opt.UseSqlServer(sqlServerConnectionString));
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(sqlServerConnectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                                       options.SignIn.RequireConfirmedAccount = true)
       .AddEntityFrameworkStores<AppDbContext>()
       .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
        builder =>
        {
            builder
               .WithOrigins(Environment.GetEnvironmentVariable("URL_CORS"))
                .WithMethods("*")
                .DisallowCredentials()
                .WithHeaders("*");
        });
});

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
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
