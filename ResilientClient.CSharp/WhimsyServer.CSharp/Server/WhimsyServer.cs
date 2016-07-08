using System;
using System.Threading;
using Nancy;
using WhimsyServer.Api.CSharp;

namespace WhimsyServer.CSharp.Server
{
    public class WhimsyServer: NancyModule
    {

        public WhimsyServer()
        {
            Get[RoutesPaths.TakesForever] = parm =>
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
                return ErrorResponse("takesForever: sorry it took me a long time");
            };
            Get[RoutesPaths.AlwaysWorks] = parm => $"{RoutesPaths.AlwaysWorks}: Here you go buddy!";
            Get[RoutesPaths.AlwaysFails] = _ => ErrorResponse($"{RoutesPaths.AlwaysFails}: Ahah gotcha again!");
            Get[RoutesPaths.RandomlyFails] = parm => (new Random()).Next(1,100) > 50 ? ErrorResponse($"{RoutesPaths.RandomlyFails}: UNLUCKY") : $"{RoutesPaths.RandomlyFails}: LUCKY";
        }

        private static Response ErrorResponse(string msg)
        {
            var response = (Response) msg;
            response.StatusCode = HttpStatusCode.TooManyRequests;
            return response;
        }
    }
}
