using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using System.Text.Json;
using AIKeywordAnalyze.ViewModels;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AIKeywordAnalyze.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordController : ControllerBase
    {
        private readonly ILogger<KeywordController> _logger;
        private readonly Kernel _Kernel;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public KeywordController(
            ILogger<KeywordController> logger,
            Kernel kernel)
        {
            _logger = logger;
            _Kernel = kernel;
            //_ITextGenerationService = kernel.GetRequiredService<ITextGenerationService>();
            _IChatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        }

        [HttpPost]
        [Route("KeywordAnalyze")]
        public async Task<IActionResult> AnalyzeContentQuality([FromBody] KeywordAnalyzeRequestViewModel request)
        {


            

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "KeywordAnalyzeVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "KeywordAnalyzeVietnam", "config.json");

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
                ResponseSchema = JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    ["Type"] = "object",
                    ["Properties"] = new Dictionary<string, object>
                    {
                        ["Primary"] = new Dictionary<string, object>
                        {
                            ["Type"] = "array",
                            ["Items"] = new Dictionary<string, object>
                            {
                                ["Type"] = "string"
                            }
                        },
                        ["Secondary"] = new Dictionary<string, object>
                        {
                            ["Type"] = "array",
                            ["Items"] = new Dictionary<string, object>
                            {
                                ["Type"] = "string"
                            }
                        },
                        ["Long_Tail"] = new Dictionary<string, object>
                        {
                            ["Type"] = "array",
                            ["Items"] = new Dictionary<string, object>
                            {
                                ["Type"] = "string"
                            }
                        },
                        ["Search_Intent"] = new Dictionary<string, object>
                        {
                            ["Type"] = "string"
                        },
                        ["Difficulty"] = new Dictionary<string, object>
                        {
                            ["Type"] = "integer"
                        },
                        ["Content_Gaps"] = new Dictionary<string, object>
                        {
                            ["Type"] = "array",
                            ["Items"] = new Dictionary<string, object>
                            {
                                ["Type"] = "string"
                            }
                        },
                        ["Semantic_Keywords"] = new Dictionary<string, object>
                        {
                            ["Type"] = "array",
                            ["Items"] = new Dictionary<string, object>
                            {
                                ["Type"] = "string"
                            }
                        },
                        ["Trending_Terms"] = new Dictionary<string, object>
                        {
                            ["Type"] = "array",
                            ["Items"] = new Dictionary<string, object>
                            {
                                ["Type"] = "string"
                            }
                        },
                        ["Analysis_Insights"] = new Dictionary<string, object>
                        {
                            ["Type"] = "string"
                        },

                    },
                    ["Required"] = new List<string> { "Primary", "Secondary", "Long_Tail", "Search_Intent", "Difficulty", "Content_Gaps", "Semantic_Keywords", "Trending_Terms", "Analysis_Insights" }
                }),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4000,
                Temperature = 0.3

            })
            {
                ["keywords"] = string.Join(",", request.Keywords.ToArray()),
                ["content"] = request.Content
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
                Data = JsonConvert.DeserializeObject<KeywordAnalyzeResponseViewModel>(responseData),
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
