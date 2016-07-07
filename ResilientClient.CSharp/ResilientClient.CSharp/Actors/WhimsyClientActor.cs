using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using ResilientClient.CSharp.Messages;

namespace ResilientClient.CSharp.Actors
{
    internal class WhimsyClientActor: TypedActor, IHandle<GetData>, IHandle<string>
    {
        public void Handle(GetData message)
        {
            RequestData();
        }

        private void RequestData()
        {
            QueryAPI().PipeTo(Self);
        }

        private Task<string> QueryAPI()
        {
            return Task.Factory.StartNew(() => "hello");
        }

        public void Handle(string message)
        {
            Context.GetLogger().Info($"Received: {message}");
        }
    }
}
