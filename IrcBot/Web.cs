using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MatafulltIrcBot
{
    public static class Web
    {
        /// <summary>
        /// Fetches the title for a website. Could be used for making sure you're not ending up somewhere bad
        /// </summary>
        /// <param name="website link as string"></param>
        /// <returns>The title of the page</returns>
        public static string GetYoutubeTitle(string link)
        {
            string content = GetSiteContent(link);
            string title = Regex.Match(content, @"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase).Value;

            return title;
        }

        public static bool IsStreamLive(string link)
        {
            string content = GetSiteContent(link);
            string title = Regex.Match(content, @"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase).Value;
            if (string.IsNullOrEmpty(title)) { return false; }
            else { return true; }
        }

        public static string GetNextEpisode(string link)
        {
            string content = GetSiteContent(link);//C# html get value of td by class
            //http://next-episode.net/the-big-bang-theory
            //HtmlNodeCollection collection = doc
            return content;
        }

        private static string GetSiteContent(string link)
        {
            WebClient client = new WebClient();
            try
            {
                string content = client.DownloadString(link);
                return content;
            }
            catch (WebException e) { ErrorLog.PrintOutput("Webclient Exception: " + e.Message); return null; }
        }
    }
}
