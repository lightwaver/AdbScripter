using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ADBScripter
{
    [DataContract]
    public class Settings
    {
        public string AdbPath { get; set; } = "adb\\adb.exe";
        public string DevicesArguments { get; set; } = "devices";

        public string ScriptToExecute { get; set; } = "start.cmd";
        public string ScriptArguments { get; set; } = "%device%";
    }
}
