using Microsoft.EntityFrameworkCore;
using StudentApi.Configuration;
using StudentApi.Data;
using StudentApi.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

    endpoints.MapGet("/echo2",
        context => context.Response.WriteAsync("echo2"));

    
});


//app.MapControllers();

app.Run();
