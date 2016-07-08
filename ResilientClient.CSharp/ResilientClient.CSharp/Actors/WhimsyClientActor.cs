using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.Pattern;
using ResilientClient.CSharp.Messages;
using WhimsyClient.CSharp.Client;
using WhimsyClient.CSharp.Messages;

namespace ResilientClient.CSharp.Actors
{
    internal class WhimsyClientActor: TypedActor, IHandle<GetData>, IHandle<ResponseSuccessful<string>>, IHandle<ResponseFailure<string>>, IHandle<GetDataAsync>
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private readonly WhimsyHttpClient _whimsyHttpClient;
        private readonly CircuitBreaker _breaker;


        public WhimsyClientActor(int clientPeriod)
        {
            _log.Debug("Starting WhimsyClientActor");
            _whimsyHttpClient = new WhimsyHttpClient(clientPeriod);
            _breaker = new CircuitBreaker(3, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(9))
                .OnClose(() => _log.Debug("breaker: CLOSE"))
                .OnOpen(() => _log.Debug("breaker: OPEN for 9 seconds"))
                .OnHalfOpen(() => _log.Debug("breaker: HALF OPEN"));
            }
        public void Handle(GetDataAsync message)
        {
           GetTheDataAsync()
                        .PipeTo(Self);
        }

        private Task<IResponseWrapper<string>> GetTheDataAsync()
        {
            return _whimsyHttpClient
                .QueryApiAsync()
                .ContinueWith(x => x.Result);
        }

        public void Handle(GetData message)
        {
            Self.Tell(_whimsyHttpClient.QueryApi());

        }
        public void Handle(ResponseSuccessful<string> message)
        {
            _log.Info($"Received: {message.Data}");
        }

        public void Handle(ResponseFailure<string> message)
        {
            _log.Debug($"Error: {message.Exception.Message}");
        }

        public void Handle(Exception message)
        {
            _log.Error(message, "Hmmm");
        }

    }
}
