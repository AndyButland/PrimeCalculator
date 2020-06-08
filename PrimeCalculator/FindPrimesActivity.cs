using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace PrimeCalculator
{
    public class FindPrimesActivity
    {
        [FunctionName("FindPrimesActivity")]
        public async Task<List<int>> Run([ActivityTrigger]IDurableActivityContext context)
        {
            // Obtain the number range.
            var input = context.GetInput<Range>();

            // Find all the prime numbers within the provided range.
            return Enumerable.Range(input.From, input.To)
                .Where(IsPrime)
                .ToList();
        }

        private bool IsPrime(int number)
        {
            // Calculate whether the provided integer is a prime number.
            // Credit for the algorithm: https://stackoverflow.com/a/15743238/489433
            if (number <= 1)
            {
                return false;
            }

            if (number == 2)
            {
                return true;
            }

            if (number % 2 == 0)
            {
                return false;
            }

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
