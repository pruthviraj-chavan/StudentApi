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

app.UseAuthorization();

app.MapControllers();

app.Run();
