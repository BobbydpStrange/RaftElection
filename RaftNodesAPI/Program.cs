
using RaftElection;
using RaftNodesAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<NodeController>();
builder.Services.AddSingleton( provider =>
{
    var nodeid = Guid.NewGuid();
    return new Node(nodeid, "follower", false, 0, "n1", true);// name the name be the index of which node it is.
});//this will make it throw a https error.
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
