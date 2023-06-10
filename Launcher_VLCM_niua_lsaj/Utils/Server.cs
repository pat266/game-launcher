using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Globalization;

namespace Launcher_VLCM_niua_lsaj.Utils
{
    public class Server
    {
        static readonly HttpClient httpClient = new HttpClient();
        /**
         * Helper method:
         * Get the maximum server number
         */
        static async Task<List<UpcomingServer>> GetServer_Niua_Async()
        {
            List<UpcomingServer> upcomingServers = new List<UpcomingServer>();
            try
            {
                var htmlDoc = new HtmlDocument();
                var html = await httpClient.GetStringAsync("http://www.niua.com/website/type/server/gamecode/lsaj/");

                htmlDoc.LoadHtml(html);

                var unreleasedServersNodes = htmlDoc.DocumentNode.SelectNodes("//dd[@class='d1 clearfix']/p");

                if (unreleasedServersNodes != null)
                {
                    foreach (var node in unreleasedServersNodes)
                    {
                        var serverNumber = node.InnerText.Trim().Split(']')[0].Replace("[", "").Replace("\u670d", "");
                        var releaseDateNode = node.SelectNodes("span");

                        if (releaseDateNode != null)
                        {
                            var releaseDateText = releaseDateNode[0].InnerText.Replace("月", "-").Replace("日", " ").Replace("开启", "").Trim();
                            DateTime releaseDate = DateTime.ParseExact(releaseDateText, "MM-dd HH:mm", CultureInfo.InvariantCulture);

                            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                            TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                            DateTime releaseDateUtc = TimeZoneInfo.ConvertTimeToUtc(releaseDate, cstZone);
                            DateTime releaseDateEst = TimeZoneInfo.ConvertTimeFromUtc(releaseDateUtc, estZone);

                            // Get current time in New York
                            DateTime currentTimeEst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone);

                            // Calculate the difference in hours
                            TimeSpan timeDifference = releaseDateEst - currentTimeEst;
                            double hoursTillRelease = timeDifference.TotalHours;

                            upcomingServers.Add(new UpcomingServer
                            {
                                serverNumber = int.Parse(serverNumber), // Remove the Chinese character
                                releaseDateEst = releaseDateEst,
                                hoursTillRelease = hoursTillRelease
                            });
                            Console.WriteLine(upcomingServers[upcomingServers.Count - 1]);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No unreleased servers found");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {e.Message}");
            }
            return upcomingServers;
        }
    }


    public class UpcomingServer
    {
        public int serverNumber { get; set; }
        public DateTime releaseDateEst { get; set; }
        public double hoursTillRelease { get; set; }
        public override string ToString()
        {
            return $"Server Number: {serverNumber}, Release Date: {releaseDateEst} EST, Hours until release: {hoursTillRelease}";
        }
    }
    public class Game2cnServer
    {
        public string state { get; set; }
        public string code { get; set; }
        public string open_date { get; set; }
    }
}
