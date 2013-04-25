using System;
using System.Text;

namespace PlugIn
{
    public interface IPlugStatic
    {
        string GetPrefix();
        string GetResponse(string irctext);
        PlugDataEditControl GetEditControl(IPlugData Data);
    }
}
