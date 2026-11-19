using HotelSystem.API;
using HotelSystem.API.Configuration;
using HotelSystem.API.Errors;
using HotelSystem.Infrastructure.Persistence.Seeding;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddLocalDevelopmentConfiguration(builder.Environment, args);
builder.Services.AddHotelApi(builder.Configuration, builder.Environment);
await using var app = builder.Build();

if (builder.Configuration.GetValue<bool>("SeedDemo"))
{
    if (!app.Environment.IsDevelopment())
        throw new InvalidOperationException("Демонстраційні дані можна додавати лише в Development.");

    await using var scope = app.Services.CreateAsyncScope();
    var result = await scope.ServiceProvider.GetRequiredService<DemoDataSeeder>().SeedAsync();
    Console.WriteLine($"Демо-дані збережено. Додано власників: {result.Owners}; готелів: {result.Hotels}; номерів: {result.Rooms}.");
    return;
}

app.UseExceptionHandler();
app.UseStatusCodePages(async status =>
    await ApiProblems.WriteAsync(status.HttpContext, ApiProblems.Create(status.HttpContext,
        status.HttpContext.Response.StatusCode, "Не вдалося виконати HTTP-запит."), status.HttpContext.RequestAborted));
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelSystem API v1"));
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseRouting();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Services.GetRequiredService<IOptions<BootstrapAdminOptions>>().Value.Enabled)
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<BootstrapAdminSeeder>().SeedAsync();
}
app.Run();

public partial class Program { }
