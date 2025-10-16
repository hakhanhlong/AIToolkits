using AIContentCreateAutomation.Steps.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using System.Text.Json;

namespace AIContentCreateAutomation.Steps
{
#pragma warning disable SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public class WritingStep: KernelProcessStep<GoogleSearchResultState>
    {

        private GoogleSearchResultState _state;

        public override ValueTask ActivateAsync(KernelProcessStepState<GoogleSearchResultState> state)
        {
            _state = state.State ?? new GoogleSearchResultState();

            return base.ActivateAsync(state);
        }

        [KernelFunction("Writing")]
        public async Task<WritingResponse> Writing(KernelProcessStepContext context, WritingRequest writingRequest, Kernel _kernel) {


            

            var filterSources = writingRequest.GoogleSearchResults.Where(x => writingRequest.Links.Contains(x.Link)).ToList();


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "WritingVietnam", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "WritingVietnam", "config.json");

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
                ResponseSchema = typeof(WritingResponse),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 0.7

            })
            {
                ["keyword"] = writingRequest.Keyword,
                ["sources"] = JsonConvert.SerializeObject(filterSources),
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_kernel, kernelArguments);
            var responseData = response.GetValue<string>();

            _state.ResponseText = responseData;

            return JsonConvert.DeserializeObject<WritingResponse>(responseData);
        }

    }
#pragma warning restore SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
