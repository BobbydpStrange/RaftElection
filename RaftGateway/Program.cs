/*using Serilog;
using Serilog.Exceptions;*/

using RaftElection;
using RaftGateway.Controllers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<GatewayController>();
string nodesLocation = Environment.GetEnvironmentVariable("Nodes_Location");
List<string> nodeLocations = nodesLocation?.Split(',')?.ToList();
List<Node> allNodes = new List<Node>();
foreach(string location in nodeLocations)
{
    var httpClient = new HttpClient();
    var response = await httpClient.GetAsync($"{location}/apiNode/Node/GetNode");
    if(response.IsSuccessStatusCode)
    {
        var node = await response.Content.ReadFromJsonAsync<Node>();
        allNodes.Add(node);
    }
    else { }
}
Gateway gateway = new Gateway(allNodes);
builder.Services.AddSingleton<Gateway>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.WriteTo.Console()
    .Enrich.WithProperty("job", "your-api-job")
    .Enrich.WithExceptionDetails();
    //.WriteTo.LokiHttp("http://loki:3100");
});*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
/*app.Use((context, next) =>
{
    context.Request.Scheme = "http";
    return next();
});*/
app.UseCors(policy =>
    policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
