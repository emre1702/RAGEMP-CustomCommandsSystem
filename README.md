<h1 align="center">
  Moved to <a href="https://github.com/emre1702/altv-CustomCommandsSystem/">altV-CustomCommandsSystem</a>! <br> 
  This library has not been accepted by RAGE:MP team for months and therefore I will no longer work on their mod.<br>
</h1>

<p align="center">
  <h2 align="center">RAGE:MP Custom Commands System</h2>
  <p align="center">A custom commands system for RAGE:MP, written in C#.</p>
</p>
<p align="center">
  <a href="https://www.nuget.org/packages/RAGEMP-CustomCommandsSystem/">
    <img alt="Nuget" src="https://img.shields.io/nuget/v/RAGEMP-CustomCommandsSystem?style=for-the-badge">
  </a>
</p>


## Features

* Register commands in different assemblies
* Unregister commands in specific assemblies
* Add custom parameter converters (with Task support)
* Add custom requirement checkers to command methods
* Execute commands manually
* Configure settings (like error messages)
* Add aliases to command methods
* Remaining text support
* Default values for command parameters support  
* Really fast with IL generated dynamic method
* You can help decide what to implement next 
  
You want more? Add an [issue](https://github.com/emre1702/RAGEMP-CustomCommandSystem/issues) and help make that system better!
  
  
## How do I use it?

1. Install the [NuGet package](https://www.nuget.org/packages/RAGEMP-CustomCommandsSystem/) 
2. [Use this code at clientside](https://github.com/emre1702/RAGEMP-CustomCommandSystem/blob/master/Integration_Client/CommandFetcher.cs).  
The code there cancels the default command and triggers the custom command.
3. Implement it. Use the [Wiki](https://github.com/emre1702/RAGEMP-CustomCommandSystem/wiki) for information.


