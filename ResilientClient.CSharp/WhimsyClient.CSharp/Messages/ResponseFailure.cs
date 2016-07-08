using System;

namespace WhimsyClient.CSharp.Messages
{
    public class ResponseFailure<T> : IResponseWrapper<T>
    {
        public Exception Exception { get; }

        public ResponseFailure(Exception exception)
        {
            Exception = exception;
        }
    }
}