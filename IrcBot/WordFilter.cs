using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MatafulltIrcBot
{
    public static class WordFilter
    {
        #region User related Methods
        public static bool ifUserslist(string[] textarray, string nick, string channel)
        {
            List<string> checkText = new List<string>();
            for (int i = 2; i < textarray.Length; i++) { checkText.Add(textarray[i]); }
            checkText[0].Remove(0, 1);

            if (String.Join(" ", checkText).TrimStart(':').StartsWith(nick + " = " + channel)) { return true; }

            return false;
        }

        public static List<String> GetUsersOnline(string[] textarray)
        {
            List<string> users = new List<string>();
            List<string> checkText = new List<string>();
            for (int i = 5; i < textarray.Length; i++) { checkText.Add(textarray[i]); }
            checkText[0].Remove(0, 1);

            foreach (string u in checkText) { users.Add(u.TrimStart(':')); }

            return users;
        }

        public static void AddUserToList(string username, List<string> userlist)
        {
            if (UserIndexOf(username, userlist) == -1) { userlist.Add(username); ErrorLog.PrintOutput("Added user: " + username); }
            else 
            {
                //User already excist
            }
        }

        public static int UserIndexOf(string username, List<string> userlist)
        {
            int index = 0;
            foreach (string s in userlist) { if (username.Equals(s)) { return index; } index += 1; }
            return -1;
        }
        #endregion

        #region General checks for Texts
        //Needs a method for loading bad words list
        public static bool CheckBadLang(string[] textarray)
        {
            bool notOK = false;
            List<string> checkText = new List<string>();

            for (int i = 3; i < textarray.Length; i++) { checkText.Add(textarray[i]); }
            checkText[0].Remove(0, 1);

            List<string> Badwords = new List<string>();
            Badwords.Add("hora");
            Badwords.Add("ost");
            Badwords.Add("fuck");
            Badwords.Add("neger");
            Badwords.Add("fitta");
            Badwords.Add("slyna");
            Badwords.Add("lekamedlego");

            foreach (string c in checkText)
            {
                if (new Regex(String.Format(@"\b({0})\b", String.Join("|", Badwords)), RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(c.TrimStart(':'))) { notOK = true; }
            }

            return notOK;
        }

        public static bool CheckMatabot(string[] textarray)
        {
            bool found = false;
            List<string> checkText = new List<string>();

            for (int i = 3; i < textarray.Length; i++) { checkText.Add(textarray[i]); }

            foreach (string c in checkText)
            {
                if (new Regex(String.Format(@"\b({0})\b", String.Join("|", "matabot")), RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(c.TrimStart(':'))) { found = true; }
            }

            return found;
        }

        public static bool CheckYoutube(string[] textarray)
        {
            bool isLink = false;
            string youtubelink = @"http://www.youtube.com/watch";
            List<string> checkText = new List<string>();

            for (int i = 3; i < textarray.Length; i++) { checkText.Add(textarray[i]); }
            checkText[0].Remove(0, 1);

            foreach (string c in checkText)
            {
                if (new Regex(youtubelink, RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(c.TrimStart(':'))) { isLink = true; }
            }

            return isLink;
        }

        public static bool CheckSpotify(string[] textarray)
        {
            bool isLink = false;
            string spotifyLink = @"http://open.spotify.com/track";
            List<string> checkText = new List<string>();

            for (int i = 3; i < textarray.Length; i++) { checkText.Add(textarray[i]); }
            checkText[0].Remove(0, 1);

            foreach (string c in checkText)
            {
                if (new Regex(spotifyLink, RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(c.TrimStart(':'))) { isLink = true; }
            }

            return isLink;
        }
        #endregion

        #region Other Methods
        public static string GetUrlFromString(string text, string url)
        {
            if (new Regex(url, RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(text)) { return text; }
            else { return null; }
        }

        public static List<string> GetUrlsFromStrings(string[] textarray, string url)
        {
            string text = String.Join(" ", textarray);
            List<string> checkText = new List<string>();
            List<string> result = new List<string>();

            for (int i = 3; i < textarray.Length; i++) { checkText.Add(textarray[i]); }
            checkText[0].Remove(0, 1);

            foreach (string c in checkText)
            {
                if (new Regex(url, RegexOptions.IgnoreCase | RegexOptions.Multiline).IsMatch(c.TrimStart(':'))) { result.Add(c); ErrorLog.PrintOutput("Found link!"); }
            }

            return result;
        }

        public static int ListIndexOf(List<string> list, object pattern)
        {
            int index = 0;
            foreach (string o in list) { if (o.Equals(pattern)) { return index; } index++; }

            return -1;
        }
        #endregion
    }
}