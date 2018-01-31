# AdbScripter

Is a small open source command line tool to simple execute script for usb-attached usb-debugging enabled andorid devices.

## Usage

Simply configure it with a settings.json file. There you can configure where to find adb and which script should be executed.

``` js

{
  "AdbPath": "adb\\adb.exe",      // Path to adb.exe
  "DevicesArguments": "devices",  // arguments to pass to it
  "ScriptToExecute": "start.cmd"  // path to the script
  "ScriptArguments": "%device%",  // arguments for the script - 
                                  // %device% is a placeholder for the device name
}

```

