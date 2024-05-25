using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using CQRS.Application.Mapping;
using CQRS.Infrastructure.Data.Context;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Configure services
MappingConfig.Configure();

//if (builder.Environment.IsEnvironment("Test"))
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
//else
//    builder.Services.AddDbContext<ApplicationDbContext>(
//        options => options.UseSqlServer(
//            builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)),
//            x => x.MigrationsAssembly("CQRS.Infrastructure")
//        )
//    );


builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

MigrateDatabase();

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

void MigrateDatabase()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    context!.Database.EnsureCreated();
    //context.Database.Migrate();
}