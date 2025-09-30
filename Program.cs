using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Application.UseCase;
using ChallangeDotnet.Domain.Interface;
using ChallangeDotnet.Infraestructure.Data.AppData;
using ChallangeDotnet.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// DbContext (Oracle) — usa a connection string "Oracle" do appsettings.json
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

// Repositórios (Infra) + UseCases (Application)
builder.Services.AddTransient<IVagaRepository, VagaRepository>();
builder.Services.AddTransient<IUnidadeRepository, UnidadeRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IMotoRepository, MotoRepository>();
//builder.Services.AddTransient<IVagaUseCase, VagaUseCase>();
builder.Services.AddTransient<IUnidadeUseCase, UnidadeUseCase>();
builder.Services.AddTransient<IUsuarioUseCase, UsuarioUseCase>();
builder.Services.AddTransient<IMotoUseCase, MotoUseCase>();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();   
    c.ExampleFilters();     
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "rateLimitePolicy", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(20);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
// Para integrar com o frontend 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Response Compression (Brotli + Gzip)
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseAuthorization();
app.UseRateLimiter();
app.UseResponseCompression();

app.MapControllers();
app.Run();
