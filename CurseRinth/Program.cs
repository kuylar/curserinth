using CurseForge.APIClient;
using CurseRinth;
using Serilog;
using Serilog.Events;

LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
	.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
	.Enrich.FromLogContext()
	.WriteTo.Console(LogEventLevel.Information);
Log.Logger = loggerConfiguration.CreateLogger();

ApiClient apiClient = new(Environment.GetEnvironmentVariable("CURSEFORGE_APIKEY") ??
                          throw new ArgumentNullException("CFApiKey",
	                          "Please set the CURSEFORGE_APIKEY environment variable"), "noreply@gmail.com");
Log.Information("Loading categories from CurseForge");
CategoryMapping.Set(apiClient).Wait();
Log.Information("Loaded categories");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new SlugMapper());
builder.Services.AddSingleton(apiClient);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(policyBuilder =>
{
	policyBuilder
		.DisallowCredentials()
		.AllowAnyOrigin()
		.AllowAnyMethod();
});
app.UseExceptionHandler("/error");
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();