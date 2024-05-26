using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using CQRS.Application.Mapping;
using CQRS.Infrastructure.Data.Context;
using Microsoft.Extensions.Options;
using CQRS.Application.Handlers;
using CQRS.Infrastructure.Data.Repositories;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
        restrictedToMinimumLevel: LogEventLevel.Information,
        columnOptions: new ColumnOptions
        {
            AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn { ColumnName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 128 },
            }
        }
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Load configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<BankApiOptions>(builder.Configuration.GetSection("BankApi"));
// Configure services
MappingConfig.Configure();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)),
        x => x.MigrationsAssembly("CQRS.Infrastructure")
    )
);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBankCommandHandler).Assembly));
builder.Services.AddScoped<IBankRepository, BankRepository>();

builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

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

//MigrateDatabase();
app.Run();

void MigrateDatabase()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    context!.Database.EnsureCreated();
    //context.Database.Migrate();
}