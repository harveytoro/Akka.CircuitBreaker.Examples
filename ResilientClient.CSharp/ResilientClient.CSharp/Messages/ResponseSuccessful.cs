namespace ResilientClient.CSharp.Messages
{
    internal class ResponseSuccessful<T> : IResponseWrapper<T>
    {
        public T Data { get; }

        public ResponseSuccessful(T data)
        {
            Data = data;
        }
    }
}