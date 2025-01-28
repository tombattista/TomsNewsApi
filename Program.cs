using Serilog;
using TomsNewsApi.Services;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

// Add logging services
Log.Logger = new LoggerConfiguration()
    .CreateLogger();
builder.Host.UseSerilog(Log.Logger);

// Register HttpClient
builder.Services.AddHttpClient<HackerNewsService>();

// Add services to the container.
if (isDevelopment)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost4200",
            builder => builder.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowLocalhost4200");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
