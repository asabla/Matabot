using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlugIn;

namespace PlugIn_example
{
    [PluginDisplayName("Spotify")]
    [PlugDescription("This Plugin-in is able to interact with spotify and give information back to IRC-bot")]
    public class SpotifyPlugin : System.Object, IPlugStatic
    {
        public SpotifyPlugin() : base() { return; }

        public string GetPrefix() { return null; }

        public string GetResponse(string irctext)
        {
            return null;
        }

        public PlugDataEditControl GetEditControl(IPlugData Data) { return null; }
    }
}
