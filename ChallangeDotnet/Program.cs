using ChallangeDotnet.Application.Interface;
using ChallangeDotnet.Application.UseCase;
using ChallangeDotnet.Domain.Interface;
using ChallangeDotnet.Infraestructure.Data.AppData;
using ChallangeDotnet.Infrastructure.Data.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using ChallangeDotnet.ML.Sentiment;



var builder = WebApplication.CreateBuilder(args);

// ?? DbContext (Oracle)
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

// ?? Repositórios (Infra) + UseCases (Application)
builder.Services.AddTransient<IVagaRepository, VagaRepository>();
builder.Services.AddTransient<IUnidadeRepository, UnidadeRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IMotoRepository, MotoRepository>();
builder.Services.AddTransient<IUnidadeUseCase, UnidadeUseCase>();
builder.Services.AddTransient<IUsuarioUseCase, UsuarioUseCase>();
builder.Services.AddTransient<IMotoUseCase, MotoUseCase>();

builder.Services.AddSingleton<ISentimentService, SentimentService>();


// ?? Controllers + Swagger + Autenticação JWT
builder.Services.AddControllers();
// ?? Versionamento da API (padrão + explorer, no mesmo builder)
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.ExampleFilters();

    // ?? Configuração de autenticação JWT no Swagger
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Use o header Authorization com o esquema Bearer (ex: 'Bearer {token}').",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    });
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// ?? JWT Authentication
var secret = builder.Configuration["Secretkey"];
if (string.IsNullOrEmpty(secret) || secret.Length < 32)
    throw new InvalidOperationException("Chave JWT inválida ou muito curta.");

var key = Encoding.UTF8.GetBytes(secret);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        // Tolerância pequena para relógio fora de sincronia (opcional)
        ClockSkew = TimeSpan.FromSeconds(5)
    };

    // ?? Loga exatamente por que falhou (assinatura inválida, expirado, etc.)
    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"?? JWT Error: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});



// ?? Health Check
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationContext>("Database");

// ?? Rate Limiting
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

// ?? CORS (para permitir frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ?? Compressão de resposta (Brotli + Gzip)
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

// ?? Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication(); // importante: vem antes do UseAuthorization
app.UseAuthorization();
app.UseRateLimiter();
app.UseResponseCompression();

app.MapControllers();

// ?? Endpoint de Health Check
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

// ? Necessário para testes de integração com WebApplicationFactory (xUnit)
public partial class Program { }
