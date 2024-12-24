using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using manage_auth.src.clients;
using manage_auth.src.repository;
using manage_auth.src.dataservice;
using manage_auth.src.util;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
// Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {environmentName}");

var builder = WebApplication.CreateBuilder(args);

// app config setup
var configuration = builder.Configuration;

// var env = builder.Environment;
// Console.WriteLine("env: ", env);
// Console.WriteLine("env.EnvironmentName: ", env.EnvironmentName);

configuration.AddJsonFile("appsettings.json", optional: false);
configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

configuration.AddEnvironmentVariables();


// Add services to the container.
builder.Services.AddControllers();;
builder.Services.AddSingleton<IRequestValidator, RequestValidator>();
builder.Services.AddSingleton<IAuthRepository, AuthRepository>();
builder.Services.AddSingleton<IAuthDataservice, AuthDataservice>();
builder.Services.AddSingleton<ICognitoClient, CognitoClient>();
builder.Services.AddSingleton<IUsersClient, UsersClient>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo 
        { 
            Title = "manage-auth", 
            Version = "v1", 
            Description = "An ASP.NET Core Web API for managing Authentication and Authorization",
            Contact = new OpenApiContact
                    {
                        Name = "Tyler Simeone",
                        Url = new Uri("https://github.com/tyler-simeone")
                    },
        });
    });

// Configure Kestrel to listen on port 80
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            // Allow all origins, methods, and headers
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


var userPoolId = configuration["UserPoolId"];
if (userPoolId.IsNullOrEmpty())
    userPoolId = configuration["AWS:Cognito:UserPoolId"];

var awsRegion = configuration["Region"];
if (awsRegion.IsNullOrEmpty())
    awsRegion = configuration["AWS:Cognito:Region"];

// Add JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://cognito-idp.{awsRegion}.amazonaws.com/{userPoolId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = $"https://cognito-idp.{awsRegion}.amazonaws.com/{userPoolId}"
        };
    });
    
// Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "manage-auth v1");
    });
    // app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();