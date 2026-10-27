using Application.Repositories;
using Application.Services;
using Infrastructure.InMemoryRepositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
builder.Services.AddSingleton<ISessionRepository, InMemorySessionRepository>();
builder.Services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

string systemPassword = builder.Configuration["SystemPassword"] ?? "admin";
builder.Services.AddScoped<ISessionService>(provider =>
    new SessionService(
        provider.GetRequiredService<IAccountRepository>(),
        provider.GetRequiredService<ISessionRepository>(),
        systemPassword));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();