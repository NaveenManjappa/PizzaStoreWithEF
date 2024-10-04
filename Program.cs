
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStore.Models;

var builder = WebApplication.CreateBuilder(args);

//Connection string
var connectionString=builder.Configuration.GetConnectionString("pizzas") ?? "Data Source=Pizzas.db";

//EF Core
// builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSqlite<PizzaDb>(connectionString);

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

app.MapPost("/pizzas",async (PizzaDb db,Pizza pizza)=>{
  await db.Pizzas.AddAsync(pizza);
  await db.SaveChangesAsync();
  return Results.Created($"/pizzas/{pizza.Id}",pizza);
});

app.MapGet("/pizzas/{id}",async(PizzaDb db,int id) => await db.Pizzas.FindAsync(id));

app.MapPut("/pizzas/{id}",async (PizzaDb db,Pizza updatePizza,int id)=>{
  var pizza=await db.Pizzas.FindAsync(id);
  if(pizza is null) return Results.NotFound();

  pizza.Name = updatePizza.Name;
  pizza.Description = updatePizza.Description;

  await db.SaveChangesAsync();
  return Results.NoContent();
});

app.MapDelete("/pizzas/{id}",async (PizzaDb db, int id) => {
  var pizza=await db.Pizzas.FindAsync(id);

  if(pizza is null) return Results.NotFound();

  db.Pizzas.Remove(pizza);
  await db.SaveChangesAsync();
  return Results.Ok();
});

app.Run();
