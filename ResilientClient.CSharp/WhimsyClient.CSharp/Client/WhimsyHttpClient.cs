using System;
using System.Threading.Tasks;
using WhimsyClient.CSharp.Messages;

namespace WhimsyClient.CSharp.Client
{
    public class WhimsyHttpClient
    {
        public const string MessageError = "SORRY SERVER DOWN";
        public const string MessageSuccess = "USEFUL DATA";
        private readonly PeriodicCounter _apiCallCounter;

        public WhimsyHttpClient(int clientPeriod)
        {
            _apiCallCounter = PeriodicCounter.Create(clientPeriod);
        }


        public IResponseWrapper<string> QueryApi()
        {
            _apiCallCounter.UpdateCounter();
            if (!_apiCallCounter.IsBeforeHalfWay())
                throw new Exception(MessageError);
            return new ResponseSuccessful<string>(MessageSuccess);
        }

        public Task<IResponseWrapper<string>> QueryApiAsync()
        {
            _apiCallCounter.UpdateCounter();
            if (!_apiCallCounter.IsBeforeHalfWay())
                throw new Exception(MessageError);
            return Task.FromResult<IResponseWrapper<string>>(new ResponseSuccessful<string>(MessageSuccess));
        }
    }
}