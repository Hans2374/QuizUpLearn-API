using Microsoft.EntityFrameworkCore;
using Repository.DBContext;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();

//Add db context
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<QuizUpLearn.API.Middlewares.ExceptionHandlingMiddleware>();
app.UseMiddleware<QuizUpLearn.API.Middlewares.ApiResponseWrappingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
