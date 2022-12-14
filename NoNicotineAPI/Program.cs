using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Repositories;
using NoNicotine_Business.Services;
using NoNicotine_Data.Context;
using NoNicotineAPI;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//MediatR
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly(),
//    typeof(CreatePatientCommand).Assembly);
var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.Contains("NoNicotine_Business")).First();

if (assembly != null)
{
    builder.Services.AddMediatR(assembly);
}

var connectionStringKey = builder.Environment.IsDevelopment() ? "local" : "AZURE_SQL_CONNECTIONSTRING";

string sqlServerConnectionString = builder.Configuration.GetConnectionString(connectionStringKey);

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseSqlServer(sqlServerConnectionString);
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                                       options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<AppDbContext>();


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

builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IEntryRepository, EntryRepository>();


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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Audience"],
        ValidIssuer = builder.Configuration["Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Key"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


