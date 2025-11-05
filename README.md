# About this Project

[Virtual Tabletop software](https://en.wikipedia.org/wiki/Digital_tabletop_game#Virtual_tabletops) uses icons for things like items, spells, skills, etc. Many VTTs come pre-packaged with icons, but it can be nice to have more.

This project contains a number of scripts that can be used to download icons from sources on the internet. This project ***does not*** contain the icons themselves. These scripts don't do anything you couldn't do manually with a browser and some patience.

# Using these Scripts

Some of these scripts are PowerShell scripts. Some are LINQPad scripts. Find the scripts in the project repository [here](https://github.com/d16-nichevo/vtt-icons-download-scripts/tree/main).

* PowerShell scripts end with a `.ps1` extension.
* LINQPad script end with a `.linq` extension.

General notes about these scripts:

* The scripts contain comments at the top. These may contain important usage information, and may tell you more about what they download.
* Observe the last edit date of each script. If a script is particularly old, it may not work any longer, as the internet a forever-changing place. It's worth a try, right?

## Using PowerShell Scripts

These are PowerShell scripts that use the [Invoke-WebRequest](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/invoke-webrequest) [cmdlet](https://learn.microsoft.com/en-us/powershell/scripting/developer/cmdlet/cmdlet-overview) (alias `wget`) to download icons from locations on the internet.

How to run a PowerShell script is beyond the scope of this document, but you should be able to find guides online with an internet search. Here are some guides I found with a quick search:

* https://sentry.io/answers/run-a-powershell-script/
* https://netwrix.com/en/resources/blog/how-to-run-powershell-script/

The PowerShell scripts in this project generally download the files to the current working directory. If that's not the case, the comments at the top will say otherwise.

## Using LINQPad Scripts

These are files designed to be opened in [LINQPad](https://www.linqpad.net/Download.aspx). I used version 8, but other recent versions may work.

LINQPad scripts contain C# code inside them. Open the `.linq` file in a text editor. If you're more comfortable running C# in another tool (such as Visual Studio or VSCode), you are certainly welcome to.
