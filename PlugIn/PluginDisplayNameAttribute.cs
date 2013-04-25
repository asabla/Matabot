using System;

namespace PlugIn
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginDisplayNameAttribute : System.Attribute
    {
        private string _displayname;

        public PluginDisplayNameAttribute(string DisplayName) : base() { _displayname = DisplayName; return; }
        public override string ToString() { return _displayname; }
    }
}
