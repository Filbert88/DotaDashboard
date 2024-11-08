using Microsoft.EntityFrameworkCore;
using DotaDashboardAPI.Data;
using DotaDashboardAPI.Services;
var builder = WebApplication.CreateBuilder(args);

string corsPolicyName = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddHttpClient<HeroService>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
var app = builder.Build();
app.UseCors(corsPolicyName);
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
