using System;
using System.Net.Http;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using ResilientClient.CSharp.Messages;

namespace ResilientClient.CSharp.Actors
{
    internal class WhimsyClientActor: TypedActor, IHandle<GetData>, IHandle<ResponseSuccessful<string>>, IHandle<ResponseFailure<string>>
    {
        private readonly Uri _whimsyServerAddress;
        private readonly HttpClient _httpClient;
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public WhimsyClientActor(Uri whimsyServerAddress, TimeSpan requestTimeOut)
        {
            _whimsyServerAddress = whimsyServerAddress;
            _httpClient = new HttpClient {Timeout = requestTimeOut};
        }

        public void Handle(GetData message)
        {
            QueryApi(message.GetPath).PipeTo(Self);
        }
        public void Handle(ResponseSuccessful<string> message)
        {
            _log.Info($"Received: {message.Data}");
        }

        public void Handle(ResponseFailure<string> message)
        {
            _log.Debug($"Error: {message.Exception.Message}");
        }
        
        private async Task<IResponseWrapper<string>> QueryApi(string getPath)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(GetPathToUri(getPath));
            }
            catch (TaskCanceledException e)
            {
                var errMsg = $"{getPath} request timed out";
//                _log.Error(errMsg);
                return new ResponseFailure<string>(new Exception(errMsg));
            }
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new ResponseSuccessful<string>(content);
            }
            else
            {
                var errMsg = $"Request failed with code {response.StatusCode}";
//                _log.Error(errMsg);
                return new ResponseFailure<string>(new Exception(errMsg));
            }
        }
        private Uri GetPathToUri(string getPath) => new Uri(_whimsyServerAddress, getPath);
    }
}
