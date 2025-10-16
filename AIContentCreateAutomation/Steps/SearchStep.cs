using AIContentCreateAutomation.Steps.Events;
using AIContentCreateAutomation.Steps.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.Plugins.Web.Google;
using StarodubOleg.GPPG.Runtime;

namespace AIContentCreateAutomation.Steps
{
#pragma warning disable SKEXP0080,SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public class SearchStep: KernelProcessStep
    {
        private AppSettings appSettings;
        

        public SearchStep(IOptions<AppSettings> options)
        {
            appSettings = options.Value;
        }
     

        [KernelFunction("Search")]
        public async Task<List<GoogleSearchResult>> Search(KernelProcessStepContext context, string keyword, Kernel _kernel)
        {

            var textSearch = new GoogleTextSearch(
            initializer: new() { ApiKey = appSettings.GoogleSearchSettings.APIKey }, searchEngineId: appSettings.GoogleSearchSettings.SearchEngineId);
            var query = keyword;

            KernelSearchResults<object> fullResults = await textSearch.GetSearchResultsAsync(query, new() { Top = 10, Skip = 10 });
            var listResult = new List<GoogleSearchResult>();
            await foreach (Google.Apis.CustomSearchAPI.v1.Data.Result result in fullResults.Results)
            {              
                listResult.Add(new GoogleSearchResult
                {
                    Title = result.Title,
                    Snippet = result.Snippet,
                    Link = result.Link,
                    DisplayLink = result.DisplayLink,
                    Kind = result.Kind,
                });
            }

            
            await context.EmitEventAsync(new()
            {
                Id = ContentCreateAutomationEvent.SuggestProcessStep,
                Data = new SuggestRequest { Keyword = keyword , GoogleSearchResults = listResult},
                Visibility = KernelProcessEventVisibility.Public
            });

            return listResult;

        }
    }
#pragma warning restore SKEXP0080, SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
