using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace PrimeCalculator
{
    public static class Orchestration
    {
        [FunctionName("Orchestration")]
        public static async Task Run([OrchestrationTrigger]IDurableOrchestrationContext context, ILogger log)
        {
            // Obtain the orchestration input range.
            var input = context.GetInput<Range>();

            // Create a collection of asynchronous tasks that will call an activity function to determine
            // the number of primes in a part of the full range.
            var parallelTasks = new List<Task<List<int>>>();
            const int ChunkSize = 10000;
            var count = input.From;
            while (count <= input.To)
            {
                var task = context.CallActivityAsync<List<int>>("FindPrimesActivity",
                    new Range
                    {
                        From = count,
                        To = count + ChunkSize,
                    });
                parallelTasks.Add(task);
                count += ChunkSize;
            }

            // Execute the tasks (fan-out) and collate the responses (fan-in).
            // Given each task returns a list of integers (the primes found) the collated results will be an array of lists.
            var taskResults = await Task.WhenAll(parallelTasks);

            // Collapse the results into an output structure and write to blob storage.
            await context.CallActivityAsync("WriteResultsActivity",
                new Result
                {
                    Primes = taskResults.SelectMany(x => x).ToList()
                });
        }
    }
}
