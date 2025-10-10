using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using AIContentSummaryGenerator.ViewModels;
using Microsoft.SemanticKernel.Connectors.Google;
using System.Text.Json;
using Newtonsoft.Json;

namespace AIContentSummaryGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;
        private readonly Kernel _Kernel;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public ContentController(
            ILogger<ContentController> logger,
            Kernel kernel)
        {
            _logger = logger;
            _Kernel = kernel;
            //_ITextGenerationService = kernel.GetRequiredService<ITextGenerationService>();
            _IChatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            

        }

        [HttpPost]
        [Route("SummaryText")]
        public async Task<IActionResult> SummaryText([FromBody] SummaryTextRequestViewModel request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "SummaryTextVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "SummaryTextVietnam", "config.json");

            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);

            // Read and parse config.json
            var configJson = System.IO.File.ReadAllText(configFilePath);
            var configOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var config = PromptTemplateConfig.FromJson(configJson);


            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);

            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ResponseSchema = typeof(SummaryTextResult),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 1000,
                Temperature = 0.3

            })
            {
                ["targetwords"] = request.TargetWords,
                ["content"] = request.Content,
                ["keyword_number"] = request.KeywordNumber
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var metadata = response.Metadata;
            //var tokenUsage = metadata!["Usage"] as ChatTokenUsage;

            var responseData = response.GetValue<string>();
            if (!string.IsNullOrEmpty(responseData))
                responseData = responseData.Replace("\"{", "{").Replace("}\"", "}").Replace("\\", "");

            return new JsonResult(new
            {
                Data =  JsonConvert.DeserializeObject<SummaryTextResult>(responseData),
                Info = new
                {
                    TotalTokenCount = metadata!["TotalTokenCount"],
                    PromptTokenCount = metadata!["PromptTokenCount"],
                    CandidatesTokenCount = metadata!["CandidatesTokenCount"],
                    CurrentCandidateTokenCount = metadata!["CurrentCandidateTokenCount"]
                }
            });


        }
    }
}
