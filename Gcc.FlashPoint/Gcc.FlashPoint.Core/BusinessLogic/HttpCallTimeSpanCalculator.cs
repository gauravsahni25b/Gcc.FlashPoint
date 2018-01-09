using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Gcc.FlashPoint.Core.BusinessLogic
{
    public class HttpCallTimeSpanCalculator
    {
        private HttpClient _client;

        public HttpCallTimeSpanCalculator()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<TimeSpan>> MeasureNGetCalls(string path, int n)
        {
            List<TimeSpan> resultCollection = new List<TimeSpan>();
            for (int i = 0; i < n; i++)
            {
                resultCollection.Add(await MeasureGetCall(path));
            }
            return resultCollection;
        }

        public async Task<IEnumerable<TimeSpan>> MeasureNPostCalls(string path, string[] jsonBodies)
        {
            List<TimeSpan> resultCollection = new List<TimeSpan>();
            for (int i = 0; i < jsonBodies.Count(); i++)
            {
                resultCollection.Add(await MeasurePostCall(path, jsonBodies[i]));
            }
            return resultCollection;
        }

        public async Task<TimeSpan> MeasureGetCall(string path)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            HttpResponseMessage response = await _client.GetAsync(path);
            watch.Stop();
            if (response.IsSuccessStatusCode)
            {
                return watch.Elapsed;
            }
            return new TimeSpan(0);
        }

        public async Task<TimeSpan> MeasurePostCall(string path, string jsonBody)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            HttpResponseMessage response = await _client.PostAsJsonAsync(path, JObject.Parse(jsonBody));
            watch.Stop();
            if (response.IsSuccessStatusCode)
            {
                return watch.Elapsed;
            }
            return new TimeSpan(0);
        }
    }
}
