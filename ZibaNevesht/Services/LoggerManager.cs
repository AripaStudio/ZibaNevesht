using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZibaNevesht.Services
{
    public static class LoggerManager
    {
        public static void Log(
            string message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            Debug.WriteLine(
                $"[{Path.GetFileName(file)}:{line}] {member}: {message}");
        }
    }
}
