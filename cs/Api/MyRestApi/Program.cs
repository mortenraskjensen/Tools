using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

string strict = "StrictPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(strict, policy =>
    {
        policy.WithOrigins("https://mortenr.azurewebsites.net/") // Replace with your allowed origin(s)
              .AllowAnyMethod()                  // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader()                  // Allow all headers
              //.AllowCredentials()               // Allow cookies/authentication headers
              .AllowAnyOrigin()
              .WithExposedHeaders("Custom-Header");
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors(strict);

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/hello", () => new { Message = "Hello World" })
.WithName("Hello")
.WithOpenApi();

app.MapGet("/hello2", () => TypedResults.Ok(new Message() { Text = "Hello World!" }));
app.MapGet("/text", () => Results.Text("This is some text"));
app.MapGet("/mul", (int a, int b) =>
{
    int result = a * b;
    Result[] res = [new Result() { Value = result }];

    return new { results = res };
});

app.MapGet("/mul2", (int a, int b) =>
{
    int result = a * b;
    return new { Result = result };
});

app.Run();

record Message
{
    public string? Text;
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Result
{
    public int Value { get; set; }
}
