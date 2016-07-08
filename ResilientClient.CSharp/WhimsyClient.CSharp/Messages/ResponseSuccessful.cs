namespace WhimsyClient.CSharp.Messages
{
    public class ResponseSuccessful<T> : IResponseWrapper<T>
    {
        public T Data { get; }

        public ResponseSuccessful(T data)
        {
            Data = data;
        }
    }
}