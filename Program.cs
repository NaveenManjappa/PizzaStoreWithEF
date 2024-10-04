

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();
