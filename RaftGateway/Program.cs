/*using Serilog;
using Serilog.Exceptions;*/

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
// Add services to the container.

builder.Services.AddControllers();
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
