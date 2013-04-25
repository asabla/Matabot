using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatafulltIrcBot
{
    public static class ErrorLog
    {
        public static void PrintOutput(string message)
        {
            DateTime now = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(now.ToString() + " " + message);
        }
    }
}
