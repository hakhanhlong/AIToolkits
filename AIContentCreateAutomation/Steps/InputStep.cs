using AIContentCreateAutomation.Steps.Events;
using Microsoft.SemanticKernel;

namespace AIContentCreateAutomation.Steps
{
#pragma warning disable SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public class InputStep : KernelProcessStep
    {

        [KernelFunction("InputKeyword")]
        public async Task InputKeyword(KernelProcessStepContext context, string keyword, Kernel _kernel)
        {

            await context.EmitEventAsync(new()
            {
                Id = ContentCreateAutomationEvent.SearchProcessStep,
                Data = keyword, //Keyword or Topic
            });

        }
    }
#pragma warning restore SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
