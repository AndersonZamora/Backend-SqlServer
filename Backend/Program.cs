using Backend.DB;
using Capa_Datos;
using Capa_Entidada;
using Capa_Negocio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Configuration.AddJsonFile("appsettings.json");
var code = builder.Configuration.GetSection("AppSettings:Dev").Get<Dev>();
var keyBytes = Encoding.UTF8.GetBytes(code.Token);

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
             policy.WithOrigins("https://backend-sqlserver-anderson.netlify.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBD_Usuario, SBD_Usuario>();
builder.Services.AddScoped<INR_Usuario, SNR_Usuario>();
builder.Services.AddScoped<IDBConexion, SDBConexion>();
builder.Services.AddScoped<ICreateHash, SCreateHash>();
builder.Services.AddScoped<ITokenCreate, STokenCreate>();
builder.Services.AddScoped<IValidations, SValidations>();
builder.Services.AddScoped<IValidarCampos, SValidarCampos>();
builder.Services.AddScoped<IDB_Producto, SDB_Producto>();
builder.Services.AddScoped<INR_Producto, SNR_Producto>();
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
