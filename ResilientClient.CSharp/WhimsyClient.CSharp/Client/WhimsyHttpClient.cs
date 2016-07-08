using System;
using System.Net.Http;
using System.Threading.Tasks;
using WhimsyClient.CSharp.Messages;
using WhimsyServer.Api.CSharp;

namespace WhimsyClient.CSharp.Client
{
    public class WhimsyHttpClient
    {
        private readonly Uri _whimsyServerAddress;
        private readonly HttpClient _httpClient;
        private readonly PeriodicCounter _apiCallCounter;

        public WhimsyHttpClient(Uri whimsyServerAddress, TimeSpan requestTimeOut)
        {
            _whimsyServerAddress = whimsyServerAddress;
            _httpClient = new HttpClient {Timeout = requestTimeOut};
            _apiCallCounter = PeriodicCounter.Create(10);
        }


        private string GetPath()
        {
            return _apiCallCounter.IsBeforeHalfWay() ? RoutesPaths.AlwaysWorks : RoutesPaths.TakesForever;
        }

        public async Task<IResponseWrapper<string>> QueryApiAsync()
        {
            var getPath = GetPath();
            _apiCallCounter.UpdateCounter();
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(GetPathToUri(getPath));
            }
            catch (TaskCanceledException e)
            {
                var errMsg = $"{getPath} request timed out {e.Message}";
                throw new Exception(errMsg);
//                return new ResponseFailure<string>(new Exception(errMsg));
            }
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new ResponseSuccessful<string>(content);
            }
            else
            {
                var errMsg = $"Request failed with code {response.StatusCode}";
                return new ResponseFailure<string>(new Exception(errMsg));
            }
        }

        private Uri GetPathToUri(string getPath) => new Uri(_whimsyServerAddress, getPath);
    }
}