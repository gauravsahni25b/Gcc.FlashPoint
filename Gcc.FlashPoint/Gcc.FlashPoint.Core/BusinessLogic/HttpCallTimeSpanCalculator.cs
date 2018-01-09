using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gcc.FlashPoint.Core.BusinessLogic
{
    public class HttpCallTimeSpanCalculator
    {
        private HttpClient _client;

        public HttpCallTimeSpanCalculator()
        {
            _client = new HttpClient();
        }

        //public Task<TimeSpan> MeasureMultipleGetCalls()
        //{
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //HttpResponseMessage response = await _client.GetAsync(path);
            //watch.Stop();
            //if (response.IsSuccessStatusCode)
            //{
            //    return watch.Elapsed;
            //}
            //throw new ApplicationException();
       // }

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
            throw new ApplicationException();
        }

        public async Task<TimeSpan> MeasurePostCall(string path, string jsonBody)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            HttpResponseMessage response = await _client.PostAsync(path, new StringContent(jsonBody));
            watch.Stop();
            if (response.IsSuccessStatusCode)
            {
                return watch.Elapsed;
            }
            throw new ApplicationException();
        }
    }
}
