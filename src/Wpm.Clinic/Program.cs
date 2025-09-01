using Microsoft.EntityFrameworkCore;
using Wpm.Clinic.Application;
using Wpm.Clinic.Data_Access;
using Wpm.Clinic.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ManagementService>();
builder.Services.AddScoped<ClinicApplicationService>();
builder.Services.AddDbContext<ClinicDbContext>(options =>
{
    options.UseInMemoryDatabase("WpmClinic");
});

builder.Services.AddHttpClient("ManagementAPI", client =>
{
    var uri = builder.Configuration.GetValue<string>("Wpm__ManagementUri");
    client.BaseAddress = new Uri(uri);
});


var app = builder.Build();
app.EnsureDbIsCreated();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
