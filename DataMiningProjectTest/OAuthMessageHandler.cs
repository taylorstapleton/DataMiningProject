using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterSample.OAuth
{
    /// <summary> 
    /// Basic DelegatingHandler that creates an OAuth authorization header based on the OAuthBase 
    /// class downloaded from http://oauth.net 
    /// </summary> 
    public class OAuthMessageHandler : DelegatingHandler
    {
        // Obtain these values by creating a Twitter app at http://dev.twitter.com/ 
        private static string _consumerKey = "5TOLpTO4Qljofz1kziiLSNBLI";
        private static string _consumerSecret = "fb2O8oqLzsG3KZ6FnX98FtMWXjYuLwyZKe2lpn4eJdwW2Vi000";
        private static string _token = "16993590-Cvztsa1QmfKnMWzoFb3rI5gnSi9Mfb84lWFbuo3U7";
        private static string _tokenSecret = "flmwsxw0mKqF8wPbwuEIG19JmCiUu6TtuWyP6QH4FAptu";

        private OAuthBase _oAuthBase = new OAuthBase();

        public OAuthMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Compute OAuth header  
            string normalizedUri;
            string normalizedParameters;
            string authHeader;

            string signature = _oAuthBase.GenerateSignature(
                request.RequestUri,
                _consumerKey,
                _consumerSecret,
                _token,
                _tokenSecret,
                request.Method.Method,
                _oAuthBase.GenerateTimeStamp(),
                _oAuthBase.GenerateNonce(),
                out normalizedUri,
                out normalizedParameters,
                out authHeader);

            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
            return base.SendAsync(request, cancellationToken);
        }
    }
}