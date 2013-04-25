using System;

namespace PlugIn
{
    public class PluginException : System.Exception
    {
        public PluginException(System.Type type, string message) : base("The plug-in " + type.Name + " is not valid or not working!\n" + message) { return; }
    }
}
