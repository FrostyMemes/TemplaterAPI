using Minio;
using StackExchange.Redis;
using Templater.Services;
using Templater.Services.Interfaces;
using Templater.Services.MarkdownTemplateService;

var builder = WebApplication.CreateBuilder(args);
var redisConnectionString = builder.Configuration["ConnectionStrings:RedisConnectionString"];
var mySQLConnectionString = builder.Configuration["ConnectionStrings:MySQLConnectionString"];
var endpoint = builder.Configuration["MinioObjectStorageParams:endpoint"];;
var accessKey = builder.Configuration["MinioObjectStorageParams:AccessKey"];;
var secretKey = builder.Configuration["MinioObjectStorageParams:SecretKey"];;


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IMarkdownParser, MarkdownParser>();
//builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(mySQLConnectionString, ServerVersion.AutoDetect(mySQLConnectionString));
});

builder.Services.AddScoped(m => new MinioClient()
    .WithEndpoint(endpoint)
    .WithCredentials(accessKey, secretKey)
    .Build());

builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IObjectStorageService, MinioObjectStorageService>();


builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.5
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();