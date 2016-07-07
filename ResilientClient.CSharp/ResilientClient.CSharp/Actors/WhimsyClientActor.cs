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

        public WhimsyClientActor(Uri whimsyServerAddress, TimeSpan requestTimeOut)
        {
            _whimsyServerAddress = whimsyServerAddress;
            _httpClient = new HttpClient {Timeout = requestTimeOut};
        }

        public void Handle(GetData message)
        {
            RequestData(message.GetPath);
        }

        private Uri GetPathToUri(string getPath)
        {

            return new Uri(_whimsyServerAddress, getPath);
        }

        private void RequestData(string getPath)
        {
            QueryApi(getPath).PipeTo(Self);
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
              
                return new ResponseFailure<string>(new Exception($"{getPath} request timed out"));
            }
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new ResponseSuccessful<string>(content);
            }
            else
            {
                return new ResponseFailure<string>(new Exception(response.StatusCode.ToString()));
            }
        }

        public void Handle(ResponseSuccessful<string> message)
        {
            Context.GetLogger().Info($"Received: {message.Data}");
        }

        public void Handle(ResponseFailure<string> message)
        {
            Context.GetLogger().Info($"Error: {message.Exception.Message}");
        }
    }
}
