using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    ////web api
    //options.Listen(IPAddress.Any, 5059, listenoptions =>
    //{
    //    listenoptions.Protocols = HttpProtocols.Http1;
    //});

    //grpc
    options.Listen(IPAddress.Any, 5000, listenoptions =>
    {
        listenoptions.Protocols = HttpProtocols.Http2;
    });
});

var routes = new[]
{
    new RouteConfig()
    {
        RouteId = "route1",
        ClusterId = "cluster1",

        Match = new RouteMatch
        {
            Path = "{**catch-all}"
        }
    }
};
var clusters = new[]
{
    new ClusterConfig()
    {
        ClusterId = "cluster1",
        HttpRequest = new ForwarderRequestConfig()
        {
            Version=HttpVersion.Version20,
            VersionPolicy= HttpVersionPolicy.RequestVersionExact,
        },
        Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "cluster1/destination1",
                new DestinationConfig()
                {
                    Address = "http://localhost:6000/"
                }
            }
        }
    }
};


//builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")); ;
builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);
//// Add services to the container.
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapReverseProxy();
//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//       new WeatherForecast
//       (
//           DateTime.Now.AddDays(index),
//           Random.Shared.Next(-20, 55),
//           summaries[Random.Shared.Next(summaries.Length)]
//       ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

app.Run();

//internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}





