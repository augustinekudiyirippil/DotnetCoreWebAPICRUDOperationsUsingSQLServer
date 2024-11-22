using CrudOperationsInNetCore.Controllers;
using CrudOperationsInNetCore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddDbContext<BrandContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BrandCS"))
);


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseSwagger();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    //});

}



app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage(); // Add this in your middleware pipeline (only in Development).


app.Run();
