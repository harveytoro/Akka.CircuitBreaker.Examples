using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using ResilientClient.CSharp.Messages;

namespace ResilientClient.CSharp.Actors
{
    internal class WhimsyClientActor: TypedActor, IHandle<GetData>, IHandle<ResponseSuccessful<string>>, IHandle<ResponseFailure<string>>
    {
        private readonly string _whimsyServerAddress;
        private readonly HttpClient _httpClient;

        public WhimsyClientActor(string whimsyServerAddress)
        {
            _whimsyServerAddress = whimsyServerAddress;
            _httpClient = new HttpClient();
        }

        public void Handle(GetData message)
        {
            RequestData(message.GetPath);
        }

        private string GetPathToUri(string getPath)
        {
            
            return getPath.StartsWith("/") ? $"{_whimsyServerAddress}{getPath}": $"{_whimsyServerAddress}/{getPath}";
        }

        private void RequestData(string getPath)
        {
            QueryAPI(getPath).PipeTo(Self);
        }

        async private Task<IResponseWrapper<string>> QueryAPI(string getPath)
        {
            var response = await _httpClient.GetAsync(GetPathToUri(getPath));
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

    internal interface IResponseWrapper<T>
    {
    }

    internal class ResponseSuccessful<T> : IResponseWrapper<T>
    {
        public T Data { get; }

        public ResponseSuccessful(T data)
        {
            Data = data;
        }
    }

    internal class ResponseFailure<T> : IResponseWrapper<T>
    {
        public Exception Exception { get; }

        public ResponseFailure(Exception exception)
        {
            Exception = exception;
        }
    }

}
