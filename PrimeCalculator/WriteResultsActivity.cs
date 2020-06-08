using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace PrimeCalculator
{
    public class WriteResultsActivity
    {
        [FunctionName("WriteResultsActivity")]
        public async Task Run([ActivityTrigger]IDurableActivityContext context,
                              [Blob("mycontainer-results", FileAccess.Write)] CloudBlobContainer outputContainer)
        {
            // Obtain the results.
            var input = context.GetInput<Result>();

            // Create the container if first use, and write the serialized results to a blob.
            await outputContainer.CreateIfNotExistsAsync();
            var blob = outputContainer.GetBlockBlobReference("results.json");
            await blob.UploadTextAsync(JsonConvert.SerializeObject(input, Formatting.Indented));
        }
    }
}
