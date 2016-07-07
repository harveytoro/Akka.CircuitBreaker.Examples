using System;
using System.Threading;
using Nancy;
using Nancy.Responses;

namespace WhimsyServer.CSharp.Server
{
    public class WhimsyServer: NancyModule
    {

        public WhimsyServer()
        {
            Get["/takesForever"] = parm =>
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
                return  "sorry it took me a long time";
            };
            Get["/alwaysWorks"] = parm => "Here you go buddy!";
            Get["/alwaysFails"] = _ => ErrorResponse("Ahah gotcha again!");
            Get["/randomlyFails"] = parm => (new Random()).Next(1,100) > 50 ? ErrorResponse("ouch bad luck") : "yay good luck";
        }

        private static Response ErrorResponse(string msg)
        {
            var response = (Response) msg;
            response.StatusCode = HttpStatusCode.TooManyRequests;
            return response;
        }
    }
}
