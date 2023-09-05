using ApiTuto;
using ApiTuto.Services;
using Microsoft.AspNetCore.SpaServices.AngularCli;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientFront/dist";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("HeroesPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IHerosProvider, DbHerosProvider>();
//builder.Services.AddScoped<IHerosProvider, FileHerosProvider>();




var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
{
    builder.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientFront";
        if (app.Environment.IsDevelopment())
        {
            spa.UseAngularCliServer(npmScript: "start");
        }
    });
});



if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

