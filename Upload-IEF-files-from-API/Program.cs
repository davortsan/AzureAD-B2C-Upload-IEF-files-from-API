using Upload_IEF_files_from_API.Interfaces;
using Upload_IEF_files_from_API.Models;
using Upload_IEF_files_from_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CORS",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
}
);

builder.Services.AddScoped<IB2CPolicy, B2CPolicy>();
builder.Services.Configure<B2CTenantSettings>(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CORS");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
