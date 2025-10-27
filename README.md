# About this Project

[Virtual Tabletop software](https://en.wikipedia.org/wiki/Digital_tabletop_game#Virtual_tabletops) uses icons for things like items, spells, skills, etc. Many VTTs come pre-packaged with icons, but it can be nice to have more.

This project contains a number of scripts that can be used to download icons from sources on the internet. This project ***does not*** contain the icons themselves.

# Using these Scripts

These are PowerShell scripts that use the [Invoke-WebRequest](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/invoke-webrequest) [cmdlet](https://learn.microsoft.com/en-us/powershell/scripting/developer/cmdlet/cmdlet-overview) (alias `wget`) to download icons from locations on the internet.

How to run a PowerShell script is beyond the scope of this document, but you should be able to find guides online with an internet search.

Find the scripts in the project repository [here](https://github.com/d16-nichevo/foundry-icon-download/tree/main). Scripts have a `ps1` extension.

Important things to note when using these scripts:

1. Observe the last edit date of each script. If a script is particularly old, it may not work any longer, as the internet a forever-changing place. It's worth a try, right?
1. The scripts download the files to the current working directory. Keep that in mind so you don't make a mess.
