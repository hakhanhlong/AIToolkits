using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using System.Text.Json;
using AIContentTranslator.ViewModels;
using Newtonsoft.Json;

namespace AIContentTranslator.Controllers
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
        [Route("TranslationLongContent")]
        public async Task<IActionResult> TranslationLongContent([FromBody] ContentTranslatorRequestViewModel request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "TranslationLongContentVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "TranslationLongContentVietnam", "config.json");

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
                ResponseSchema = typeof(ContentTranslatorResponseViewModel),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 0.0

            })
            {
                ["content"] = request.Content
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var metadata = response.Metadata;
            //var tokenUsage = metadata!["Usage"] as ChatTokenUsage;

            var responseData = response.GetValue<string>();

            return new JsonResult(new
            {
                Data = JsonConvert.DeserializeObject<ContentTranslatorResponseViewModel>(responseData),
                Info = new
                {
                    TotalTokenCount = metadata!["TotalTokenCount"],
                    PromptTokenCount = metadata!["PromptTokenCount"],
                    CandidatesTokenCount = metadata!["CandidatesTokenCount"],
                    CurrentCandidateTokenCount = metadata!["CurrentCandidateTokenCount"]
                }
            });


        }


        [HttpPost]
        [Route("TranslationShortContent")]
        public async Task<IActionResult> TranslationShortContent([FromBody] ContentTranslatorRequestViewModel request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "TranslationShortContentVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "TranslationShortContentVietnam", "config.json");

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
                ResponseSchema = typeof(ContentTranslatorResponseViewModel),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4096,
                Temperature = 0.0

            })
            {
                ["content"] = request.Content
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var metadata = response.Metadata;
            //var tokenUsage = metadata!["Usage"] as ChatTokenUsage;

            var responseData = response.GetValue<string>();

            return new JsonResult(new
            {
                Data = JsonConvert.DeserializeObject<ContentTranslatorResponseViewModel>(responseData),
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
