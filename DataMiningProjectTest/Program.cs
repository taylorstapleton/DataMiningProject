using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using tweetStream = Tweetinvi.Stream;
using TwitterSample.OAuth;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Models;
using Newtonsoft.Json;
using System.Web;
using Tweetinvi.Logic.Model;

namespace DataMiningProjectTest
{
    class Program
    {
        public static int count = 0;
        public static long total = 618232;
        public static int fileCount = 61;
        public static int linesPerFile = 10000;
        public static List<string> lines = new List<string>();
        public static string filePath = "C:\\Users\\taylor\\Desktop\\TweetData\\Sunday22";


        static void Main(string[] args)
        {

            TwitterCredentials.SetCredentials("16993590-Cvztsa1QmfKnMWzoFb3rI5gnSi9Mfb84lWFbuo3U7", "flmwsxw0mKqF8wPbwuEIG19JmCiUu6TtuWyP6QH4FAptu", "5TOLpTO4Qljofz1kziiLSNBLI", "fb2O8oqLzsG3KZ6FnX98FtMWXjYuLwyZKe2lpn4eJdwW2Vi000");

            var southwest = new Coordinates(-125.711777, 30.618056);
            var northeast = new Coordinates(-64.495959, 49.281421);

            while (true)
            {
                var filteredStream = tweetStream.CreateFilteredStream();

                filteredStream.AddLocation(southwest, northeast);

                filteredStream.MatchingTweetReceived += printTweet;

                filteredStream.StartStreamMatchingAllConditions();
            }




            //sampleStream.TweetReceived += printTweet;//(sender, arguments) => { Console.WriteLine(arguments.Tweet.Text); };
            //sampleStream.StartStream();//"https://stream.twitter.com/1.1/statuses/filter.json?language=en&locations=30.618056,-125.711777,49.281421,-64.495959");

        }

        public static void printTweet(object sender, MatchedTweetReceivedEventArgs arguments)
        {
            if (arguments.Tweet.Language != Tweetinvi.Core.Enum.Language.English)
            {
                Console.Write(arguments.Tweet.Language.ToString().Substring(0,3) + "  ");
                return;
            }

            if (arguments.Tweet.Coordinates == null)
            {
                Console.Write("CN  ");
                return;
            }

            // create an object of the data we want to keep
            FileLine current = new FileLine() { Id = total, TweetId = arguments.Tweet.Id, Latitude = arguments.Tweet.Coordinates.Latitude, Longitude = arguments.Tweet.Coordinates.Longitude, Text = arguments.Tweet.Text };

            // url encode the text of the tweet to avoid messy characters in files
            current.Text = HttpUtility.UrlEncode(current.Text);

            // convert to json
            var json = JsonConvert.SerializeObject(current);

            // add to our collection of lines
            lines.Add(json);

            // time to write a file
            if (count > linesPerFile)
            {
                string path = filePath + "\\TweetFile" + fileCount++ + ".tweet";
                File.WriteAllLines(path, lines);
                lines.Clear();
                count = 0;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("current file count: " + count++ + " total tweet count: " + total++);
            Console.WriteLine();
            Console.WriteLine();

        }

#region to nuke

            //var result = service.GetTweet(new GetTweetOptions() { Id = 1234 });

            /*
            var result = service.Search(new SearchOptions() { Lang = "en", Count = 10, Q = " "}); //Geocode = new TwitterGeoLocationSearch(40.7441704, -111.8628205, 1, TwitterGeoLocationSearch.RadiusType.Mi), Count = 200});

            var asdf = service.

            var geoCode = new TwitterGeoLocationSearch(40.7441704, -111.8628205, 1, TwitterGeoLocationSearch.RadiusType.Mi);

            string auth = "oauth_consumer_key=\"5TOLpTO4Qljofz1kziiLSNBLI\",oauth_nonce=\"euvxprnriyj0dose\",oauth_signature=\"hKLeVcpzw2V5fFVD9I6448nvsDw%3D\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1424560637\",oauth_token=\"16993590-Cvztsa1QmfKnMWzoFb3rI5gnSi9Mfb84lWFbuo3U7\",oauth_version=\"1.0\"";

            HttpClient client = new HttpClient();

            string url = "https://api.twitter.com/1.1/search/tweets.json?geocode={0}%2C{1}%2C1mi&count={3}&include_entities={2}&include_rts=1";

            string langurl = "https://api.twitter.com/1.1/search/tweets.json?lang=en";

            url = string.Format(url, geoCode.Coordinates.Latitude, geoCode.Coordinates.Longitude, geoCode.Radius, 200);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", auth);

            //var result = client.GetAsync(url).Result;

            //var result = client.GetAsync(langurl).Result;

            //var asdf = JsonConvert.DeserializeObject<TwitterResponse>(result.Content.ReadAsStringAsync().Result);
             * */

            //HttpClient client = new HttpClient(new OAuthMessageHandler(new HttpClientHandler()));

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("5TOLpTO4Qljofz1kziiLSNBLI", "fb2O8oqLzsG3KZ6FnX98FtMWXjYuLwyZKe2lpn4eJdwW2Vi000");

            //string url = "https://stream.twitter.com/1/statuses/sample.json";

            //var result = client.GetAsync(url).Result;

            //using (StreamReader reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            //{
             //   while (true)
              //  {
               //     var currentLine = reader.ReadLine();
                //    Console.Read();
                //}
        //}
#endregion

    }

    public class FileLine
    {
        public long Id;
        public long TweetId;
        public string Text;
        public double Latitude;
        public double Longitude;
    }
}
