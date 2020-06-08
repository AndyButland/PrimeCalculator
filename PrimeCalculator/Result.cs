using System.Collections.Generic;
using Newtonsoft.Json;

namespace PrimeCalculator
{
    public class Result
    {
        [JsonProperty("totalPrimes")]
        public int TotalPrimes => Primes.Count;

        [JsonProperty("primes")]
        public List<int> Primes { get; set; }
    }
}
