using System.Net.Http;
using System;
using Polly.Extensions.Http;
using Polly;
using Polly.Retry;

namespace NSE.WebAPI.Core.Extensions
{
    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> WaitTry()
        {
            var retry = HttpPolicyExtensions
               .HandleTransientHttpError()
               .WaitAndRetryAsync(new[]
               {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
               });

            return retry;
        }
    }
}
