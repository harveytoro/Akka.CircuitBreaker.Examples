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
            var whimsyServerAddress = $"{ConfigurationManager.AppSettings["address"]}:{ConfigurationManager.AppSettings["port"]}";
            
            var actorSystem = ActorSystem.Create("AResilientRESTClient");
            var client = actorSystem.ActorOf(Props.Create(() => new WhimsyClientActor(new Uri(whimsyServerAddress), TimeSpan.FromSeconds(45))), "WhimsyClientActor");
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
                GetData.Create(),
                null 
                );
        }
    }
}
