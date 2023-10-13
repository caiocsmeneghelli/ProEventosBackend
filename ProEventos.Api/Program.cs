using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ProEventos.Application.Interfaces;
using ProEventos.Application.Services;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = 
        Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<ProEventosContext>(opt => opt.UseMySql(connectionString, serverVersion));

builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<ILoteService, LoteService>();

builder.Services.AddScoped<IProEventosRepository, ProEventosRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<ILoteRepository, LoteRepository>();


builder.Services.AddCors(option => {
    option.AddPolicy("MyAllowedOrigins",
    policy => {
        //policy.WithOrigins("http://localhost:4200")        
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("MyAllowedOrigins");
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = new PathString("/Resources")
});

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
