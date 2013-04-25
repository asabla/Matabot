using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MatafulltIrcBot;

namespace MatafulltIrcBot
{
    public struct IrcConfig
    {
        public string name;
        public string owner;
        public string nick;
        public string description;
        public string server;
        public int port;
        public string channel;
    }

    public class MataBot
    {
        public static TcpClient socket;
        public static StreamReader reader;
        public static StreamWriter writer;
        public static IrcConfig conf;
        public static List<string> UserList = new List<string>();
        private static Encoding Current;

        private string _startmsg;

        public MataBot(IrcConfig Conf) { conf = Conf; }

        #region Connect and disconnect
        public void Connect()
        {
            try
            {
                socket = new TcpClient(conf.server, conf.port);
                socket.ReceiveBufferSize = 1024;
                _startmsg = "PRIVMSG " + conf.channel + " :";
                ErrorLog.PrintOutput("STATUS: CONNECTED!");

                NetworkStream stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream, Encoding.Default);

                Write("USER " + conf.nick + " 3 * :" + conf.description);
                Write("NICK " + conf.nick);

                Web.GetYoutubeTitle("http://www.youtube.com/watch?v=4PTsEjSEnhA");  //To startup Webclient, this will make the bot more responsive (no more 5+sec for first use)

                Read(reader);

                Dispose();
                stream.Close();
            }
            catch { ErrorLog.PrintOutput("Could not connect to: " + conf.server); }
        }

        private void Dispose()
        {
            reader.Close();
            writer.Close();
        }
        #endregion

        #region Read and Write Functions
        private void Read(StreamReader reader)
        {
            try { while (true) { interpret(reader.ReadLine()); } }
            catch { ErrorLog.PrintOutput("Unable to read from server!"); }
        }

        private void Write(string data)
        {
            try
            {
                writer.WriteLine(data);
                ErrorLog.PrintOutput(">>> " + data);
                Console.WriteLine(">>> " + data);
                writer.Flush();
            }
            catch { ErrorLog.PrintOutput("Error!"); }
        }
        #endregion

        private void interpret(string data)
        {
            string[] interpretData = data.Split(' ');
            string msg = String.Join(" ", interpretData.Skip(3).ToArray());
            string user = interpretData[0];
            string[] sender = user.Split('!');
            user = sender[0].Substring(1);

            ErrorLog.PrintOutput("<<< " + user + " - " + msg);
            Console.WriteLine("<<< " + user + " - " + msg);

            if (interpretData[0].Equals("PING")) { onPing(interpretData[1]); }
            else if (interpretData[1].Equals("JOIN")) { onJoin(interpretData[0]); }
            else if (interpretData[1].Equals("PART") || interpretData[1].Equals("QUIT")) { }
            else if (interpretData[1].Equals("PRIVMSG"))
            {
                //Makes sure it's the right channel
                if (interpretData[2].ToLower().Equals(conf.channel))
                {
                    //if a command with two or more paramters are given, then just run control
                    if (interpretData.Length > 4) { onPublicMessage(interpretData[3], interpretData[4]); }

                    if (!String.IsNullOrEmpty(interpretData[3]))//Just makes sure null isn't received, *early control of data*
                    {
                        //ErrorLog.PrintOutput("Received TextEncoding: " + reader.CurrentEncoding.ToString());
                        Current = reader.CurrentEncoding;
                        onPublicMessage(interpretData[3]);
                        if (WordFilter.CheckBadLang(interpretData)) { onBadLanguage(interpretData[0], interpretData[3]); }
                        if (WordFilter.CheckMatabot(interpretData)) { onMatabot(interpretData[0], interpretData[3]); }
                        if (WordFilter.CheckYoutube(interpretData)) { onYoutubeLink(interpretData); }
                        if (WordFilter.CheckSpotify(interpretData)) { onSpotifyLink(interpretData); }
                    }
                }
                //If message is private, then give a proper response
                else if (interpretData[2].Equals(conf.nick))
                {
                    onPrivateMessage(interpretData[0], interpretData[3]);
                }
            }
            //Fetchs all users online in current channel
            else if (interpretData[1].Equals("353")) { if (WordFilter.ifUserslist(interpretData, conf.nick, conf.channel)) { UserList = WordFilter.GetUsersOnline(interpretData); } }
            else if (interpretData[1].Equals("376")) { Write("JOIN " + conf.channel); } //Only runs at startup, if everything went fine join channel
        }

        #region onCommands (like onPublicMessage or onPrivateMessage)
        private void onPing(string pong)
        {
            pong = "PONG " + pong;
            Write(pong);
        }

        private void onJoin(string data)
        {
            //http://msdn.microsoft.com/en-us/library/gg405479(v=pandp.40).aspx
            String[] working = data.Split('!');
            string user = working[0].Substring(1);
            if (!user.Equals(conf.nick))
            {
                //Write(_startmsg + "Welcome to " + conf.channel + ", " + user);
                WordFilter.AddUserToList(user, UserList);
            }
        }

        private void onQuit(string data)
        {
            string[] working = data.Split('!');
            string user = working[0].Substring(1);
            if (!user.Equals(conf.nick))
            {
                //Username - quit   <--format
                UserList.RemoveAt(WordFilter.ListIndexOf(UserList, user));
            }
        }

        private void onBadLanguage(string user, string data)
        {
            string[] sender = user.Split('!');
            user = sender[0].Substring(1);
            data = data.Substring(1);

            Write(_startmsg + "Watch your language, " + user + "!");
        }

        private void onMatabot(string user, string data)
        {
            string[] sender = user.Split('!');
            user = sender[0].Substring(1);
            data = data.Substring(1);

            Write(_startmsg + "Hey " + user + "! What do you want with me?");
        }

        private void onYoutubeLink(string[] data)
        {
            List<string> urls = WordFilter.GetUrlsFromStrings(data, @"http://www.youtube.com/watch");
            foreach (string s in urls) { Write(_startmsg + "Youtube title: " + Web.GetYoutubeTitle(s.TrimStart(':'))); }
        }

        private void onSpotifyLink(string[] data)
        {
            List<string> urls = WordFilter.GetUrlsFromStrings(data, @"http://open.spotify.com/track");
            foreach (string s in urls) 
            {
                string response = Web.GetYoutubeTitle(s.TrimStart(':'));
                if (!string.IsNullOrEmpty(response)) { Write(_startmsg + "Spotify Title: " + response); }
                else { Write(_startmsg + "Your link is broken! Check it!"); }
            }
        }

        private void onPublicMessage(string data)
        {
            string prefix = "!";
            data = data.Substring(1);

            if (data.Equals(prefix + "time")) { DateTime time = DateTime.Now; String now = String.Format("{0:F}", time); Write(_startmsg + now); }
            else if (data.Equals(prefix + "cycle")) { Write("PART " + conf.channel + " :cycling."); Write("JOIN " + conf.channel); Write(_startmsg + "I'm back MF's!"); }
            else if (data.Equals(prefix + "derp")) { Write(_startmsg + "Don't you mean DERP!?"); }
            else if (data.Equals(prefix + "next")) { Write(_startmsg + "Next episode for this shit is!"); }
            else if (data.Equals(prefix + "whoisadam"))
            {
                string temp = "Is a balless man with great greasy muscles, or you can just read more about him here: http://en.wikipedia.org/wiki/Eunuch";
                Write(_startmsg + temp);
            }
            else if (data.Equals(prefix + "whoismart"))
            {
                string temp = "Mart is a cheeseloving animal from the northern countries of Europe, you can read more about him here: http://en.wikipedia.org/wiki/Mouse";
                Write(_startmsg + temp);
            }
            else if (data.Equals(prefix + "whoissparrow"))
            {
                string temp = "Sparrow is a drunken bird which never sleeps, you can read more about him here: http://www.newscientist.com/article/dn21850-drunk-birds-had-onetoomany-berries-to-blame.html";
                Write(_startmsg + temp);
            }
            else if (data.Equals(prefix + "users"))
            {
                Write(_startmsg + "Users online: " + string.Join(" ", UserList));
            }
            else if (data.Equals(prefix + "refreshusers"))
            {
                Write("NAMES " + conf.channel + " :");
            }
            else { ErrorLog.PrintOutput("Function not found!"); }
            //else { Write("PRIVMSG " + conf.channel + " :HEY! Watch your language", writer); }
        }

        private void onPublicMessage(string data, string arg1)
        {
            string prefix;
            prefix = "!";

            data = data.Substring(1);

            if (data.Equals(prefix + "stream"))
            {
                string url = "http://api.justin.tv/api/stream/list.xml?channel=" + arg1;
                //Checks if arg1 is a link or just the username for the channel
                //if (string.IsNullOrEmpty(WordFilter.GetUrlFromString(arg1, "http://www.twitch.com/"))) { url = "http://api.justin.tv/api/stream/list.xml?channel="; }
                
                if (Web.IsStreamLive(url)) { Write(_startmsg + "Stream is live!"); }
                else { Write(_startmsg + "Stream is offline!"); }
            }
            /*
            if (data.Equals(prefix + "voice")) { Write("MODE " + conf.channel + " +v " + arg1, writer); }
            else if (data.Equals(prefix + "halfop")) { Write("MODE " + conf.channel + " +h " + arg1, writer); }
            else if (data.Equals(prefix + "op")) { Write("MODE " + conf.channel + " +o" + arg1, writer); }
            else if (data.Equals(prefix + "protect")) { Write("MODE " + conf.channel + " +a " + arg1, writer); }
            else if (data.Equals(prefix + "owner")) { Write("MODE " + conf.channel + " +q " + arg1, writer); }
            else if (data.Equals(prefix + "devoice")) { Write("MODE " + conf.channel + " -v " + arg1, writer); }
            else if (data.Equals(prefix + "dehalfop")) { Write("MODE " + conf.channel + " -h " + arg1, writer); }
            else if (data.Equals(prefix + "deop")) { Write("MODE " + conf.channel + " -o " + arg1, writer); }
            else if (data.Equals(prefix + "deprotect")) { Write("MODE " + conf.channel + " -a " + arg1, writer); }
            else if (data.Equals(prefix + "deowner")) { Write("MODE " + conf.channel + " -q " + arg1, writer); }
             */
        }

        private void onPrivateMessage(string user, string data)
        {
            string[] sender = user.Split('!');
            user = sender[0].Substring(1);
            Write("PRIVMSG " + user + " :" + conf.nick + " Matabot: please try commands in " + conf.channel);
        }
        #endregion
    }
}
