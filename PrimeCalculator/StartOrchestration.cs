using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PrimeCalculator
{
    public static class StartOrchestration
    {
        [FunctionName("StartOrchestration")]
        public static async Task Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")]string myQueueItem,
                                     [DurableClient]IDurableOrchestrationClient starter,
                                     ILogger log)
        {
            // Deserialize the message input into a class representing the range to calculate primes between.
            var input = JsonConvert.DeserializeObject<Range>(myQueueItem);

            // Start a new orchestration function, passing the typed input data.
            await starter.StartNewAsync("Orchestration", input);
        }
    }
}
