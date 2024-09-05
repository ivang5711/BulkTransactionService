using BulkTransactionServiceWebApi.Persistence.Database;
using BulkTransactionServiceWebApi.Persistence.Repositories;
using BulkTransactionServiceWebApi.Presentation;
using BulkTransactionServiceWebApi.Services;
using BulkTransactionServiceWebApi.Validations;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var defaultConnectionString = builder
    .Configuration.GetSection("Database")
    .GetConnectionString("DefaultConnection");

builder.Services.AddScoped<PaymentService>();
builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<BulkTransferRequest>, BulkTransferRequestValidator>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<IDbConnectionFactory>(connectionString => new NpgsqlConnectionFactory(
    defaultConnectionString!
));

var app = builder.Build();

app.MapControllers();
DbInitializer.Initialize(defaultConnectionString!);

app.Run();
