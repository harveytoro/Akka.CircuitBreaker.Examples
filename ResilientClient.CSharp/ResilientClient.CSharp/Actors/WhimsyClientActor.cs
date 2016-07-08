using System;
using Akka.Actor;
using Akka.Event;
using ResilientClient.CSharp.Messages;
using WhimsyClient.CSharp.Client;
using WhimsyClient.CSharp.Messages;

namespace ResilientClient.CSharp.Actors
{
    internal class WhimsyClientActor: TypedActor, IHandle<GetData>, IHandle<ResponseSuccessful<string>>, IHandle<ResponseFailure<string>>
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private readonly WhimsyHttpClient _whimsyHttpClient;


        public WhimsyClientActor(Uri whimsyServerAddress, TimeSpan requestTimeOut)
        {
            _whimsyHttpClient = new WhimsyHttpClient(whimsyServerAddress, requestTimeOut);
        }

        public void Handle(GetData message)
        {
            _whimsyHttpClient.QueryApiAsync().PipeTo(Self);
        }
        public void Handle(ResponseSuccessful<string> message)
        {
            _log.Info($"Received: {message.Data}");
        }

        public void Handle(ResponseFailure<string> message)
        {
            _log.Debug($"Error: {message.Exception.Message}");
        }
    }
}
