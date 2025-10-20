using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.Plugins.Web.Google;
using Microsoft.Extensions.Options;
using AIContentLongForm.ViewModels;
using System.IO.Pipes;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using System.Text.Json;

#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace AIContentLongForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;
        private readonly Kernel _Kernel;
        private AppSettings appSettings;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public ContentController(
            ILogger<ContentController> logger,
            Kernel kernel,
            IOptions<AppSettings> options)
        {
            _logger = logger;
            _Kernel = kernel;
            //_ITextGenerationService = kernel.GetRequiredService<ITextGenerationService>();
            _IChatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            appSettings = options.Value;

        }


        [HttpPost]
        [Route("LongformCreate")]
        public async Task<IActionResult> LongformCreate([FromBody] LongformRequestViewModel request)
        {


            var listSources = await DiscoveryContent(request.Keyword);


            //var outline = await CreateOutline(request);






            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "LongFormCreationVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "LongFormCreationVietnam", "config.json");

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
                ResponseSchema = typeof(LongformResponse),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 0.7

            })
            {
                ["keyword"] = request.Keyword,
                ["length"] = request.Length,
                ["tone"] = request.Tone,
                ["type"] = request.Type,
                ["demographic"] = request.Demographic,
                ["language"] = request.Language,
                //["outlinecontent"] = outline,
                ["sources"] = JsonConvert.SerializeObject(listSources)
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseData = response.GetValue<string>();


            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = JsonConvert.DeserializeObject<LongformResponse>(responseData),
                Info = new
                {
                    TotalTokenCount = metadata!["TotalTokenCount"],
                    PromptTokenCount = metadata!["PromptTokenCount"],
                    CandidatesTokenCount = metadata!["CandidatesTokenCount"],
                    CurrentCandidateTokenCount = metadata!["CurrentCandidateTokenCount"]
                }
            });

        }


        private async Task<List<GoogleSearchResultViewModel>> DiscoveryContent(string keyword = "")
        {


            var textSearch = new GoogleTextSearch(
           initializer: new() { ApiKey = appSettings.GoogleSearchSettings.APIKey }, 
           searchEngineId: appSettings.GoogleSearchSettings.SearchEngineId);

            var query = keyword;

            KernelSearchResults<object> fullResults = await textSearch.GetSearchResultsAsync(query, new() { Top = 5, Skip = 10 });
            var listResult = new List<GoogleSearchResultViewModel>();
            await foreach (Google.Apis.CustomSearchAPI.v1.Data.Result result in fullResults.Results)
            {
                listResult.Add(new GoogleSearchResultViewModel
                {
                    Title = result.Title,
                    Snippet = result.Snippet,
                    Link = result.Link,
                    DisplayLink = result.DisplayLink,
                    Kind = result.Kind,
                });
            }

            return listResult;
        }

        private async Task<string> CreateOutline(LongformRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "LongFormOutlineVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "LongFormOutlineVietnam", "config.json");

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
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4096,
                Temperature = 0.2

            })
            {
                ["keyword"] = request.Keyword,
                ["length"] = request.Length,
                ["tone"] = request.Tone,
                ["type"] = request.Type,
                ["demographic"] = request.Demographic,
                ["language"] = request.Language,
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseData = response.GetValue<string>();

            return responseData!;

        }
    }
}

#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
