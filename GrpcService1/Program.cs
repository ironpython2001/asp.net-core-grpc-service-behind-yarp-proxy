using GrpcService1.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    //web api
    options.Listen(IPAddress.Any, 5059, listenoptions => 
    { 
        listenoptions.Protocols = HttpProtocols.Http1; 
    });
    
    //grpc
    options.Listen(IPAddress.Any, 6000, listenoptions => 
    { 
        listenoptions.Protocols = HttpProtocols.Http2; 
    });
});
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add grpc services to the container.
builder.Services.AddGrpc();


//add web api 
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//webapi developer exception page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


//webapi show swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
//webapi use routing 
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    //webapi use controllers
    endpoints.MapControllers();

    //grpc
    // Configure the HTTP request pipeline.
    app.MapGrpcService<GreeterService>();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
});

app.Run();
