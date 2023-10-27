using DemoApi.Services;
using Marten;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// What is this doing? Inversion of Control and Dependency Injection - Loose Coupling.
builder.Services.AddScoped<TemperatureConverterService>();
builder.Services.AddScoped<ICalculateFees, StandardFeeCalculator>();
builder.Services.AddScoped<ISystemTime, SystemTime>();
builder.Services.AddScoped<TodoListService>();

var connectionString = builder.Configuration.GetConnectionString("database") ?? throw new Exception("No Database");

builder.Services.AddMarten(options =>
{
	options.Connection(connectionString);
	if (builder.Environment.IsDevelopment())
	{
		options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
	}
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}



app.MapGet("/todo-list/{id:Guid}", async (Guid id, TodoListService service) =>
{
	TodoItemCreated? item = await service.GetTodoItemAsync(id);
	if (item is null)
	{
		return Results.NotFound();
	}
	else
	{
		return Results.Ok(item);
	}
});

app.MapGet("/todo-list", async (TodoListService service, [FromServices] ILogger<Program> logger) =>
{
	logger.LogInformation("Getting this todo list");

	var response = await service.GetAllAsync();
	return Results.Ok(response);
});

app.MapPost("/todo-list", async (CreateToDoItem item, TodoListService service) =>
{
	var response = await service.CreateTodoItemAsync(item);
	return Results.Ok(response);
});

app.MapGet("/temperatures/farenheit/{temp:float}/celcius", (float temp, TemperatureConverterService service) =>
{

	return service.ConvertFtoC(temp);
});

app.MapGet("/temperatures/celcius/{temp:float}/farenheit", (float temp, TemperatureConverterService service) =>
{
	return service.ConvertCtoF(temp);
});

app.MapGet("/states", () =>
{
	var states = new Dictionary<string, string>()
	{
		{ "OH", "Ohio" },
		{ "KY", "Kentucky" }
	};

	return Results.Ok(states);
});

app.MapGet("/employees/{id}", (string id) =>
{
	return Results.Ok(new { name = "Bob Smith", status = "fulltime" });
});

app.Run(); // "Blocking Call"

public record ConversionResponse(float F, float C);

public partial class Program { }