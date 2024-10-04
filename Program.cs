

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStore.Models;

var builder = WebApplication.CreateBuilder(args);

//EF Core
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>{
  c.SwaggerDoc("v1",new OpenApiInfo
        {Title="PizzaStore API",Description="Making the Pizzas you love",Version="v1"}
        );
});

var app = builder.Build();

//Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>{
  c.SwaggerEndpoint("/swagger/v1/swagger.json","Pizza Store API V100");
});

app.MapGet("/", () => "Hello World!");

//Pizza store apis
app.MapGet("/pizzas",async (PizzaDb db)=> await db.Pizzas.ToListAsync());

app.Run();
