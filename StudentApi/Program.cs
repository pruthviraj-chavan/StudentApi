using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentApi.Configuration;
using StudentApi.Data;
using StudentApi.Data.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme. Enter Bearer [space] add your token in the text input. Example: Bearer swersdf877sdf",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "oauth2",

                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});




//builder.Services.AddTransient<ILogger, LogToServerMemory>();

builder.Services.AddScoped<IStudentRepository , StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

//builder.Services.AddCors(options => options.AddPolicy("MyTestCors", policy =>
//{
//    //allow only few origins
//    //policy.WithOrigins("http://localhost:4200");

//    //allowing all origins
//    //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); //methods are get post etc 



//}));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    }); //default policy

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }); //allowing all origins 


    options.AddPolicy("AllowOnlyLocalHost", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
        
    }); //allow only local host origin

    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        policy.WithOrigins("http://google.com", "http://gmail.com","http://googledrive.com").AllowAnyMethod().AllowAnyHeader();

    }); //only google

    options.AddPolicy("AllowOnlyMicrosoft", policy =>
    {
        policy.WithOrigins("http://microsoft.com", "http://onedrive.com","http://azure.com").AllowAnyMethod().AllowAnyHeader();

    }); //only microsoft 

});

builder.Services.AddAutoMapper(typeof(AutomapperConfig));

builder.Services.AddDbContext<StudentDbContext>(options => {

       
options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDbConnection"));
});

var key3 = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTLocal")); 
var key1 = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTGoogle"));
var key2 = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTMicrosoft"));

//injecting JWT authentication dependancy configuration

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginForGoogle",options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key1),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
}).AddJwtBearer("LoginForMicrosoft", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key2),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
}).AddJwtBearer("LoginForLocal", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key3),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(); //if u want to use default policy then make empty 

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testwebapi",
        context => context.Response.WriteAsync("echo"))
        .RequireCors("AllowOnlyLocalHost");

    endpoints.MapControllers()
             .RequireCors("AllowAll");

    endpoints.MapGet("api/testwebapi1",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecretKey")));

    
});


//app.MapControllers();

app.Run();
