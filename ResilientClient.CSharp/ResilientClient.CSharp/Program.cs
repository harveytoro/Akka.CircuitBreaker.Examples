using System;
using Akka.Actor;
using ResilientClient.CSharp.Actors;
using ResilientClient.CSharp.Messages;

namespace ResilientClient.CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Return to exit:");
            var actorSystem = ActorSystem.Create("AResilientRESTClient");
            var client = actorSystem.ActorOf(Props.Create(() => new WhimsyClientActor()), "WhimsyClientActor");
            client.Tell(new GetData());
            Console.ReadLine();
        }
    }
}
