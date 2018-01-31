using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace ADBScripter
{
    class Program
    {
        private const string settingsFile = "settings.json";
        private static bool running = true;
        private static Settings settings = new Settings();

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ADB Scripter - 2018 - Stefan M. Marek");
                Console.WriteLine();

                settings = LoadSettings();

                Doit();

                Console.WriteLine("press enter to exit...");
                Console.ReadLine();
                running = false;

                //SaveSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private static void SaveSettings()
        {
            var serializer = new DataContractJsonSerializer(typeof(Settings));
            using (var sr = new FileStream(settingsFile, FileMode.Create))
            {
                serializer.WriteObject(sr, settings);
            }
        }

        private static async void Doit()
        {
            var devices = new List<string>();
            while (running)
            {
                try
                {
                    var newDevices = GetDeviceList();

                    foreach (var newDevice in newDevices)
                    {
                        if (devices.Contains(newDevice)) continue;

                        devices.Add(newDevice);
                        Console.WriteLine("device added: " + newDevice);
                        StartScript4Device(newDevice);
                    }

                    foreach (var device in devices.ToArray())
                    {
                        if (newDevices.Contains(device)) continue;

                        devices.Remove(device);
                        Console.WriteLine("device removed: " + device);
                    }

                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ops: " + ex);
                }
            }

        }

        private static Settings LoadSettings()
        {
            Settings settings = null;
            if (File.Exists(settingsFile))
            {
                var serializer = new DataContractJsonSerializer(typeof(Settings));
                using (var sr = new FileStream(settingsFile, FileMode.Open))
                {
                    settings = serializer.ReadObject(sr) as Settings;
                }
            }
            return settings;
        }

        private static void StartScript4Device(string device)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = settings.ScriptToExecute,
                Arguments = settings.ScriptArguments.Replace("%device%", device),
                UseShellExecute = true
            };
            Process.Start(pi);
        }

        static List<string> GetDeviceList()
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = "adb\\adb.exe",
                Arguments = "devices",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var p = Process.Start(pi);

            string device;
            var newDevices = new List<string>();
            while (!p.HasExited)
            {
                var line = p.StandardOutput.ReadLine();

                if (AnalyseLine(line, out device))
                {
                    newDevices.Add(device);
                }
            }

            while (!p.StandardOutput.EndOfStream)
            {
                var line = p.StandardOutput.ReadLine();
                //Console.WriteLine("op: " + line);

                if (AnalyseLine(line, out device))
                {
                    newDevices.Add(device);
                }
            }

            return newDevices;
        }

        private static bool AnalyseLine(string line, out string device)
        {
            device = null;
            line = line.Trim();
            //if (string.IsNullOrEmpty(line) || line == "List of devices attached") return false;
            if (line.EndsWith("device"))
            {
                device = line.Substring(0, line.IndexOf('\t'));
                return true;
            }
            return false;
        }
    }
}
