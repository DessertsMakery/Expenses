using DessertsMakery.Expenses.Common.Extensions;
using DessertsMakery.Expenses.Common.Helpers;
using DessertsMakery.Expenses.Persistence.DependencyInjection;

var allAssemblies = AssemblyHelper.LoadAssemblies<Program>(nameof(DessertsMakery));
var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCustomBsonSerializing(allAssemblies)
    .AddPersistence(builder.Configuration);

var app = builder.Build();
app.UseCustomBsonSerializing();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.Run();
