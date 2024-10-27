using api.Data;
using api.Interfaces;
using api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//2. add contoller
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//4. add AutoMappe
builder.Services.AddAutoMapper(typeof(Program));

//1. Add DBContext
builder.Services.AddDbContext<ApplicationDBContext>(Options => {
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//5.add Repository
builder.Services.AddScoped<IStockRepository,StockReposiory>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//3. add Mapcontrollers
app.MapControllers();

app.Run();
