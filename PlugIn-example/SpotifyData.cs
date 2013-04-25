using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlugIn;

namespace PlugIn_example
{
    //http://www.c-sharpcorner.com/UploadFile/rmcochran/plug_in_architecture09092007111353AM/plug_in_architecture.aspx
    //http://www.drdobbs.com/cpp/implementing-a-plug-in-architecture-in-c/184403942?pgno=1

    public class SpotifyData : System.Object, IPlugData
    {
        public event EventHandler DataChanged;

        public string SpotifyTitle
        {
            get;
            set
            {
                this.SpotifyTitle = value;
                if (DataChanged != null) { DataChanged(this, new EventArgs()); return; }
            }
        }

        public override string ToString()
        {
            return SpotifyTitle;
        }

        internal SpotifyData(string spotifytitle) { SpotifyTitle = spotifytitle; return; }
    }
}
