using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatafulltIrcBot;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            IRCConfig conf = new IRCConfig();
            conf.name = "MataBot";
            conf.nick = "Matabot";
            conf.port = 6667;
            conf.server = "irc.quakenet.org";
            conf.channel = "#MatafulltGN";

            IrcBot bot = new IrcBot(conf);
            bot.Connect();
            Console.WriteLine("Bot Quit/Crashed");
            Console.ReadLine();
             * */

            IrcConfig conf = new IrcConfig();
            conf.description = "A IRC-bot from MatafulltGamingNetwork";
            conf.name = "kebablabot";
            conf.nick = "kebablabot";
            conf.owner = "kebabla";
            conf.port = 6667;
            conf.server = "irc.quakenet.org";
            conf.channel = "#webhallen";

            MataBot bot = new MataBot(conf);
            bot.Connect();
            Console.ReadLine();
        }
    }
}
