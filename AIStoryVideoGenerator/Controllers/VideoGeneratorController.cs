using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using System.Text.Json;
using AIStoryVideoGenerator.ViewModels;
using Azure.Core;

namespace AIStoryVideoGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGeneratorController : ControllerBase
    {
        private readonly ILogger<VideoGeneratorController> _logger;
        private readonly Kernel _Kernel;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public VideoGeneratorController(
            ILogger<VideoGeneratorController> logger,
            Kernel kernel)
        {
            _logger = logger;
            _Kernel = kernel;
            //_ITextGenerationService = kernel.GetRequiredService<ITextGenerationService>();
            _IChatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        }


        [HttpPost]
        [Route("Storyboard")]
        public async Task<IActionResult> Storyboard([FromBody] StoryboardRequestViewModel request)
        {
            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "StoryboardVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "StoryboardVietnam", "config.json");

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
                ResponseSchema = typeof(StoryboardResponseViewModel),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4096,
                Temperature = 0.7,
                TopP = 0.9
            })
            {
                ["style"] = request.Style,
                ["num_scenes"] = request.Num_Scenes,
                ["idea"] = request.IdeaText
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var metadata = response.Metadata;
            //var tokenUsage = metadata!["Usage"] as ChatTokenUsage;

            var responseData = response.GetValue<string>();
           


            return new JsonResult(
            new
            {
                Data = JsonConvert.DeserializeObject<StoryboardResponseViewModel>(responseData),
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
        [Route("SceneImages")]
        public async Task<IActionResult> SceneImages([FromBody] SceneImageRequestViewModel request)
        {

            var sceneImageResponse = new SceneImageResponseViewModel();
            sceneImageResponse.Name = request.Storyboard.Name;
            foreach(var scene in request.Storyboard.Scenes)
            {


                SceneImage sceneImage = new()
                {
                    Description = scene.Description,
                    Narration = scene.Narration,
                    Prompt = $"Tạo một hình ảnh {request.ImageStyle} đại diện cho cảnh này: {scene.Description}. " +
                    $"Hình ảnh phải rõ ràng về mặt thị giác và tập trung vào các yếu tố cốt lõi được mô tả."                    
                };


                sceneImage.PromptEnhanced = $"{sceneImage.Prompt}" +
                    $"Lời nhắc nâng cao:" +
                    $"{await EnhancedGenerateImagePrompt(sceneImageResponse.Name, scene.Description, scene.Narration, sceneImage.Prompt, request.ImageStyle)}";

                sceneImageResponse.SceneImages.Add(sceneImage);
            }
            return new JsonResult(sceneImageResponse);
        }



        private async Task<string> GenerateImagePrompt(string userprompt)
        {
            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "GenerateImagePromptVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "GenerateImagePromptVietnam", "config.json");

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
                MaxTokens = 1000,
                Temperature = 0.2
            })
            {
                ["usertext"] = userprompt
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var responseData = response.GetValue<string>();

            return responseData;
        }


        private async Task<string> EnhancedGenerateImagePrompt(string title, string description, string content, string user_prompt, string style)
        {
            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "EnhancedGenerateImagePromptVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "EnhancedGenerateImagePromptVietnam", "config.json");

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
                MaxTokens = 1000,
                Temperature = 0.2                
            })
            {
                ["title"] = title,
                ["description"] = description,
                ["user_prompt"] = user_prompt,
                ["content"] = content,
                ["style"] = style
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var responseData = response.GetValue<string>();

            return responseData;
        }

    }
}
