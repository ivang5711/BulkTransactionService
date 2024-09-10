using BulkTransactionServiceWebApi.Persistence.Database;
using BulkTransactionServiceWebApi.Persistence.Repositories;
using BulkTransactionServiceWebApi.Presentation;
using BulkTransactionServiceWebApi.Services;
using BulkTransactionServiceWebApi.Validations;

using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// [Q]: why did you specify the same settings both in the appsettings.json & in the appsettings.Development.json files?
// Have a look at the Options Pattern in .NET

var defaultConnectionString = builder
    .Configuration.GetSection("Database")
    .GetConnectionString("DefaultConnection");

// [Q]: did you choose to not use interfaces on purpose? Or have you forgot about them?
// [Q]: The services lifetime are correct? As far as I remember, the validation ones should be Singleton
// have a refresh about the different lifetime of the services
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
