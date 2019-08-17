# Modernise the look and feel of the application to the most recent Windows GUI styling

## Context and Problem Statement

To provide the application a modern look and feel, 

## Considered Options
* [MahApps](https://github.com/MahApps/MahApps.Metro), and their dedicated [website](https://mahapps.com/)
* [Syncfusion](https://www.nuget.org/packages/Syncfusion.Themes.Metro.WPF/) 
* [Modern UI](https://github.com/firstfloorsoftware/mui)
* [Elysium](http://elysium.asvishnyakov.com/en/)

[Source](https://stackoverflow.com/questions/13592326/making-wpf-applications-look-metro-styled-even-in-windows-7-window-chrome-t)
## Decision Outcome
MahApps was chosen as an option. The setup is relatively simple to incorporate all the Metro styling in the application and the solution is still maintained. It also seems that MahApps is the least intrusive, as it makes use of templates to perform the styling of the GUI components. 

Other considerations are:
* Obsolescence
The package ModernUI doesn't seem to have updated for a while. The website for Elysium was modernised, but the latest release seems to be somewhere around 2013. Additionally, it is unknown whether both packages are compatible with the most recent .NET frameworks

* Intrusiveness and dependencies
Elysium requires a separate installation, meaning that the styling will be dependent on the installed version without indication to auto-update. 

* Freeware
For a project of such a small scale, the paid option Syncfusion is not preferred. 
