using System;
using System.Configuration;
using System.Threading;
using Akka.Actor;
using ResilientClient.CSharp.Actors;
using ResilientClient.CSharp.Messages;

namespace ResilientClient.CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("AResilientRESTClient");
            var numberOfConsecutiveResults = 5;
            var client = actorSystem.ActorOf(Props.Create(() => new WhimsyClientActor(2*numberOfConsecutiveResults)), "WhimsyClientActor");
            Console.WriteLine("Starting in 3 seconds.");
          
            Console.WriteLine("Press Return to exit:");
            Communicate(actorSystem, client);
            Console.ReadLine();
        }

        static void Communicate(ActorSystem actorSystem, IActorRef client)
        {
            
            actorSystem
                .Scheduler
                .ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(3), 
                TimeSpan.FromSeconds(1),
                client,
                GetDataAsync.Create(),
                null 
                );
        }
    }
}
