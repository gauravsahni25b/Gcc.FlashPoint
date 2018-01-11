using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Gcc.FlashPoint.Core.BusinessLogic
{
    public class HttpCallTimeSpanCalculator
    {
        private readonly HttpClient _client;

        public HttpCallTimeSpanCalculator()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<FlashPointHttpResponse>> MeasureNGetCalls(string path, int n)
        {
            List<FlashPointHttpResponse> resultCollection = new List<FlashPointHttpResponse>();
            for (int i = 0; i < n; i++)
            {
                resultCollection.Add(await MeasureGetCall(path));
            }
            return resultCollection;
        }

        public async Task<IEnumerable<FlashPointHttpResponse>> MeasureNPostCalls(string path, string jsonArray)
        {
            var jsonBodies = JArray.Parse(jsonArray);
            List<FlashPointHttpResponse> resultCollection = new List<FlashPointHttpResponse>();
            for (int i = 0; i < jsonBodies.Count(); i++)
            {
                resultCollection.Add(await MeasurePostCall(path, jsonBodies[i]));
            }
            return resultCollection;
        }

        public async Task<FlashPointHttpResponse> MeasureGetCall(string path)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (HttpResponseMessage response = await _client.GetAsync(path))
            {
                watch.Stop();
                return new FlashPointHttpResponse(response.StatusCode, watch.Elapsed);
            }
        }

        public async Task<FlashPointHttpResponse> MeasurePostCall(string path, string jsonBody)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            return await MeasurePostCall(path, JObject.Parse(jsonBody));
        }

        public async Task<FlashPointHttpResponse> MeasurePostCall(string path, JToken jsonBody)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (HttpResponseMessage response = await _client.PostAsJsonAsync(path, jsonBody))
            {
                watch.Stop();
                return new FlashPointHttpResponse(response.StatusCode, watch.Elapsed);
            }
        }
    }

    public class FlashPointHttpResponse
    {
        public FlashPointHttpResponse(HttpStatusCode statusCode, TimeSpan timeTaken)
        {
            StatusCode = statusCode;
            TimeTaken = timeTaken;
        }
        public HttpStatusCode StatusCode { get; protected set; }
        public TimeSpan TimeTaken { get; protected set; }
    }
}
