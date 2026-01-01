using BloomTech.Api.Jobs;
using BloomTech.Api.Repositories;
using BloomTech.Core.Interfaces;
using BloomTech.Data.Context;
using BloomTech.Data.Repositories;
using BloomTech.Data.Services;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.EntityFrameworkCore;
using Hangfire.MemoryStorage;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("BloomTech.Data")
    ));

builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMemoryStorage());

builder.Services.AddHangfireServer();

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IInsiderRepository, InsiderRepository>();
builder.Services.AddHttpClient<IFinanceService, YahooFinanceService>();
builder.Services.AddScoped<INewsService, GoogleNewsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    //  dakikada bir çalýþýyor (Test için).
    recurringJobManager.AddOrUpdate<RecurringStockJob>(
        "fetch-mrna-price",
        job => job.ProcessStockData(),
        Cron.Minutely // Test için her dakika. Sonra 5 dakikada 1 olucak.
    );

    recurringJobManager.AddOrUpdate<RecurringNewsJob>(
        "fetch-mrna-news",
        job => job.ProcessNews(),
        Cron.MinuteInterval(15) // Her 15 dakikada bir
    );
}

app.Run();
