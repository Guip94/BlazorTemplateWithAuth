
using Domain.Repository;
using Domain.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using UserAPI.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using ToolSecurity;
using System.Text.Json;
using UserAPI.Properties;
using System.Security.Cryptography.X509Certificates;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
IConfiguration _config = builder.Configuration;
IRsaService rsaServiceSK = new RsaService(Resource.sk);
string mesCors = "mesCors";

builder.Services.AddControllers();

//IRsaService rsaServiceDB = new RsaService();

//byte[] keys = rsaServiceDB.Keys;
//byte[] publicKey = rsaServiceDB.PublicKey;

//SecurityInfos info = new SecurityInfos("xxx", "xxx");

//string json = JsonSerializer.Serialize(info);

//byte[] cypher = rsaServiceDB.Encrypt(json);

//File.WriteAllBytes("xxx.bin", cypher);
//File.WriteAllBytes("xxx.bin", keys);





#region : configuration Https

//permettre la lecture du json.env  ==> ajout dans le SDK
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json.env", optional:false, reloadOnChange: true)
    .Build();





string CertificatePath = configuration["certificatePfx:certificatePath"]!;
string Pwd = configuration["certificatePfx:certificatePassword"]!;




X509Certificate2 certificate = new X509Certificate2(CertificatePath, Pwd);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
    options.ListenAnyIP(443, listenOptions =>
        {
            listenOptions.UseHttps(certificate);
        });
});

#endregion






//mettre en place si DVP pour limiter les CORS

var corsOrigin = builder.Configuration.GetSection("CorsOrigins:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(mesCors, policies =>
    {
        policies.WithOrigins(corsOrigin)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowedToAllowWildcardSubdomains().WithExposedHeaders("Content-Disposition");
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policies =>
    {
        policies.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _config["Jwt:Issuer"],


            ValidateAudience = true,
            ValidAudience = _config["Jwt:Audience"],


            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // ValidAudience = _config["Jwt:Audience"], ==> car plusieurs audiences donc impossible
            //AudienceValidator = (audiences, securityToken, validationParameters) =>
            //{
            //    var validAudiences = _config.GetSection("Jwt:Audience").Get<string[]>();
            //    return audiences.Intersect(validAudiences).Any();
            //},


            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(rsaServiceSK.DecryptAsString(Resource.skData)))
        };
        options.Events = new JwtBearerEvents() 
        {
            OnTokenValidated = async context =>
            {
                var blackList = context.HttpContext.RequestServices.GetRequiredService<IBlackList>();
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();


                if (blackList.IsTokenBlacklisted(token))
                {
                    context.Fail("Votre session a �chu. Veuillez vous reconnecter");
                }
            }
        };
       
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor(); // Donne acc�s � l'injection IHttpContextAccessor

builder.Services.AddSwaggerGen( options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        Name ="Authorization",
        In= ParameterLocation.Header,
        Type=SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                }
            },
            Array.Empty<string>()
        }

    });
    
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("useraccess", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
    options.AddPolicy("adminaccess", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("entraccess", policy => policy.RequireClaim(ClaimTypes.Role, "Entr"));
    

});


//builder.Services.AddScoped<IRsaService, RsaService>();
//builder.Services.AddScoped<DbConnection>(sp => new SqlConnection(_config.GetConnectionString("default")));

builder.Services.AddScoped<IRsaService>(sp => new RsaService(Resource.dbKeys));
builder.Services.AddScoped<DbConnection>(sp =>
{
    IRsaService _service = sp.GetService<IRsaService>()!;
    string json = _service.DecryptAsString(Resource.dbData);
    SecurityInfos info = JsonSerializer.Deserialize<SecurityInfos>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;
    return new SqlConnection(string.Format(_config.GetConnectionString("docker")!, info.Mail, info.Pwd));
});





builder.Services.AddScoped<IRoleRepository, RoleService>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<ITokenRepository, TokenService>();
builder.Services.AddScoped<IAdressRepository, AdressService>();
builder.Services.AddSingleton<IBlackList, BlackListService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}


app.UseCors("dev");

app.UseRouting();

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
