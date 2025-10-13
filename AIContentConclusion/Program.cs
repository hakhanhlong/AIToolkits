using AIContentConclusion;
using Microsoft.SemanticKernel;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var _configuration = provider.GetRequiredService<IConfiguration>();


builder.Host.UseSerilog((hostingContext, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<Kernel>(sk =>
{
    var kernelBuilder = Kernel.CreateBuilder();

    string _geminiApiKey = _configuration["AppSettings:GeminiSettings:APIKey"];
    string _geminiModelCode = _configuration["AppSettings:GeminiSettings:ModelCode"];

    //kernelBuilder.AddOpenAIChatCompletion("llama3.2:3b", new Uri("http://localhost:11434/v1"), apiKey: "", serviceId: "gemma3");
    kernelBuilder.AddGoogleAIGeminiChatCompletion(_geminiModelCode, apiKey: _geminiApiKey);

    return kernelBuilder.Build();

});


builder.Services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
