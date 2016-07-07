using System;

namespace ResilientClient.CSharp.Messages
{
    internal class ResponseFailure<T> : IResponseWrapper<T>
    {
        public Exception Exception { get; }

        public ResponseFailure(Exception exception)
        {
            Exception = exception;
        }
    }
}