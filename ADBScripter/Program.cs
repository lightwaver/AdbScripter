using System;
using System.Diagnostics;

namespace ADBScripter
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName ="adb\\adb.exe",
                Arguments = "devices",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var p = Process.Start(pi);

            while(!p.HasExited)
            {
                var line = p.StandardOutput.ReadLine();
                Console.WriteLine("op: " + line);
            }

            while (!p.StandardOutput.EndOfStream)
            {
                var line = p.StandardOutput.ReadLine();
                Console.WriteLine("op: " + line);

                AnalyseLine(line);
            }
            Console.ReadLine();

        }

        private static void AnalyseLine(string line)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line) || line == "List of devices attached") return;
            

        }
    }
}
