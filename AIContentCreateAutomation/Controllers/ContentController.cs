using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using System.Text.Json;
using AIContentCreateAutomation.ViewModels;
using AIContentCreateAutomation.Steps;
using AIContentCreateAutomation.Steps.Events;

namespace AIContentCreateAutomation.Controllers
{
#pragma warning disable SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

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
        [Route("ContentCreate")]
        public async Task<IActionResult> ContentCreate([FromBody] ContentCreateRequestViewModel request)
        {
            ProcessBuilder process = new("ContentCreateAutomation");
            var inputStep = process.AddStepFromType<InputStep>();
            var searchStep = process.AddStepFromType<SearchStep>();
            var suggestStep = process.AddStepFromType<SuggestStep>();
            var writingStep = process.AddStepFromType<WritingStep>();



            var finishStep = process.AddStepFromType<FinishStep>();




            process.OnInputEvent(ContentCreateAutomationEvent.StartProcessStep)
                .SendEventTo(new ProcessFunctionTargetBuilder(inputStep, functionName: "InputKeyword", parameterName: "keyword"));

            inputStep.OnEvent(ContentCreateAutomationEvent.SearchProcessStep)
                .SendEventTo(new ProcessFunctionTargetBuilder(searchStep, functionName: "Search", parameterName:"keyword"));

            searchStep.OnEvent(ContentCreateAutomationEvent.SuggestProcessStep)
                .SendEventTo(new ProcessFunctionTargetBuilder(suggestStep, functionName: "Suggest", parameterName: "suggestRequest"));


            suggestStep.OnEvent(ContentCreateAutomationEvent.WritingProcessStep)
                .SendEventTo(new ProcessFunctionTargetBuilder(writingStep, functionName: "Writing", parameterName: "writingRequest"));

            var test =  writingStep.OnFunctionResult().ToString();

            finishStep.OnEvent(ContentCreateAutomationEvent.FinishProcessStep)
                .StopProcess();


            KernelProcess kernelProcess = process.Build();

            await using var runningProcess = await kernelProcess.StartAsync(_Kernel, new KernelProcessEvent() { Id = ContentCreateAutomationEvent.StartProcessStep, Data = request.Keyword });

            var steps = kernelProcess.Steps;






            return new JsonResult("OK");

        }

    }

#pragma warning restore SKEXP0080 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
