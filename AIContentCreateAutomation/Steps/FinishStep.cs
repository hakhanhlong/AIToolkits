using Microsoft.SemanticKernel;

namespace AIContentCreateAutomation.Steps
{
#pragma warning disable SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public class FinishStep : KernelProcessStep
    {
        [KernelFunction]
        public async ValueTask ExecuteAsync(KernelProcessStepContext context)
        {
            Console.WriteLine("Finished...\n");
        }
    }
#pragma warning restore SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
