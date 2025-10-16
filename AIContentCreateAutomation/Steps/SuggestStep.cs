using AIContentCreateAutomation.Steps.Events;
using AIContentCreateAutomation.Steps.Models;
using Azure.Core;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using Serilog.Extensions.Hosting;
using System.Text.Json;

namespace AIContentCreateAutomation.Steps
{
#pragma warning disable SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public class SuggestStep : KernelProcessStep
    {


        [KernelFunction("Suggest")]
        public async Task Suggest(KernelProcessStepContext context, SuggestRequest suggestRequest, Kernel _kernel)
        {
            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "SuggestVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "SuggestVietnam", "config.json");

            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);

            // Read and parse config.json
            var configJson = System.IO.File.ReadAllText(configFilePath);
            var configOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var config = PromptTemplateConfig.FromJson(configJson);


            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _kernel.CreateFunctionFromPrompt(promptContent);

            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ResponseSchema = typeof(SuggestResponse),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4096,
                Temperature = 0.2

            })
            {
                ["keyword"] = suggestRequest.Keyword,
                ["sources"] = JsonConvert.SerializeObject(suggestRequest.GoogleSearchResults),
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_kernel, kernelArguments);
            var responseData = response.GetValue<string>();

            await context.EmitEventAsync(new()
            {
                Id = ContentCreateAutomationEvent.WritingProcessStep,
                Data = new WritingRequest()
                {
                    Keyword = suggestRequest.Keyword,
                    Links = JsonConvert.DeserializeObject<SuggestResponse>(responseData)!.Links,
                    GoogleSearchResults = suggestRequest.GoogleSearchResults
                },
                Visibility = KernelProcessEventVisibility.Public
            });

            return;
        }

    }
#pragma warning restore SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
