using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ADBScripter
{
    [DataContract]
    public class Settings
    {
        [DataMember]
        public string AdbPath { get; set; } = "adb\\adb.exe";
        [DataMember]
        public string DevicesArguments { get; set; } = "devices";
        [DataMember]
        public string ScriptToExecute { get; set; } = "start.cmd";
        [DataMember]
        public string ScriptArguments { get; set; } = "%device%";
    }
}
