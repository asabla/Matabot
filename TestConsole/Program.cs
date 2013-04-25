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
            IrcConfig conf = new IrcConfig();
            conf.description = "Description for bot";   //DEscription for the bot
            conf.name = "botName";  //Botname
            conf.nick = "botNick";  //Nick for the bot
            conf.owner = "owner";   //Irc Owner
            conf.port = 6667;   //Default quakenet port
            conf.server = "irc.quakenet.org";
            conf.channel = "#channel";  //IRC-Channel HERE!

            MataBot bot = new MataBot(conf);
            bot.Connect();
            Console.ReadLine();
        }
    }
}
