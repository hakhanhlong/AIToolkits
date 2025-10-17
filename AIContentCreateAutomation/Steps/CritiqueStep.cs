using AIContentCreateAutomation.Steps.Events;
using AIContentCreateAutomation.Steps.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using System.Text.Json;

namespace AIContentCreateAutomation.Steps
{
#pragma warning disable SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public class CritiqueStep: KernelProcessStep
    {
        [KernelFunction("Critique")]
        public async Task Critique(KernelProcessStepContext context, CritiqueRequest critiqueRequest, Kernel _kernel)
        {
            


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "CritiqueVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "CritiqueVietnam", "config.json");

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
                ResponseSchema = typeof(CritiqueResponse),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 0.2

            })
            {                
                ["content"] = JsonConvert.SerializeObject(critiqueRequest.WritingResponse.Paragraphs),
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_kernel, kernelArguments);
            var responseData = JsonConvert.DeserializeObject<CritiqueResponse>(response.GetValue<string>());


            if (responseData != null && responseData.Critiques.Any())
            {
                await context.EmitEventAsync(new()
                {
                    Id = ContentCreateAutomationEvent.WritingProcessStepRevise,
                    Data = new WritingReviseRequest()
                    {
                        Title = critiqueRequest.WritingResponse.Title,
                        Summary = critiqueRequest.WritingResponse.Summary,
                        Date = critiqueRequest.WritingResponse.Date,
                        Critiques = responseData.Critiques,
                        Paragraphs = critiqueRequest.WritingResponse.Paragraphs
                    }
                });
            }


            return;

        }
    }
#pragma warning restore SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
