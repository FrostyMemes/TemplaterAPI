using StackExchange.Redis;
using Templater.Services.MarkdownTemplateService;

var builder = WebApplication.CreateBuilder(args);
var redisConnectionString = builder.Configuration["ConnectionStrings:RedisConnectionString"];
var mySQLConnectionString = builder.Configuration["ConnectionStrings:MySQLConnectionString"];

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ITemplateParser, TemplateParser>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(mySQLConnectionString, ServerVersion.AutoDetect(mySQLConnectionString));
});
builder.Services.AddHttpClient();
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

app.UseCors(x => x
    .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();