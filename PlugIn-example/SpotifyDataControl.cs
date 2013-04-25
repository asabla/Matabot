using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlugIn;

namespace PlugIn_example
{
    public class SpotifyDataControl : PlugDataEditControl
    {
        private Label _lblSpotifyTitle;

        internal SpotifyDataControl(SpotifyData Data) : base(Data)
        {
            _lblSpotifyTitle = new Label();
            _lblSpotifyTitle.Text = ((SpotifyData)_data).SpotifyTitle;
            _lblSpotifyTitle.TextChanged += new EventHandler(this.TextChanged);

            this.Controls.Add(_lblSpotifyTitle);
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (sender == _lblSpotifyTitle) { ((SpotifyData)_data).SpotifyTitle = _lblSpotifyTitle.Text; }
        }
    }
}
