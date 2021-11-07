[Home page of sample projects](..)

# Useful links

* Worker:
  - https://wakeupandcode.com/worker-service-in-net-core-3-1/
  - https://stackoverflow.com/questions/56941898/how-to-setup-event-log-for-net-core-3-0-worker-service
* Options and configuration: 
  - https://stackoverflow.com/questions/58183920/how-to-setup-app-settings-in-a-net-core-3-worker-service#58184278
* Environment selection:
  - https://www.thecodebuzz.com/set-appsettings-json-dynamically-dev-and-release-environments-asp-net-core/
  - https://www.thecodebuzz.com/createdefaultbuilder-configuration-management-net-core-and-asp-net-core/
  - https://stackoverflow.com/questions/46364293/automatically-set-appsettings-json-for-dev-and-release-environments-in-asp-net-c
  - https://stackoverflow.com/questions/37322565/dotnet-run-or-dotnet-watch-with-development-environment-from-command-line/43992754#43992754
  - https://www.tektutorialshub.com/asp-net-core/aspnetcore_environment-variable-in-asp-net-core/
* User secret in console app:
  - https://medium.com/@granthair5/how-to-add-and-use-user-secrets-to-a-net-core-console-app-a0f169a8713f

# Useful commands

  ## Create the project

```ps
mkdir worker
dotnet new sln
dotnet new worker -o Worker
dotnet sln add reference .\Worker\

dotnet add package Microsoft.Extensions.Hosting.WindowsServices
dotnet add package Microsoft.Extensions.Logging
dotnet add package Microsoft.Extensions.Logging.EventLog
dotnet add package Microsoft.Extensions.Configuration.UserSecrets #needed if using user secrets on console
```

  ## Test the worker

Open Terminal and launch the worker (see profiles in launchSettings.json):
```ps
clear; dotnet run --project Worker --launch-profile Worker-Dev
clear; dotnet run --project Worker --launch-profile Worker-Prod
```

  ## Manage the window service (using PS ISE)

*Note:* need to run Powershell ISE as administrator to use ```sc``` commands.

```ps
sc create ".Net Test Worker" binpath=C:\<path to project>\samples\worker\Worker\bin\Debug\net5.0\publish\Worker.exe
sc start
sc delete
sc stop
```

Do not forget to publish the project before:
```ps
dotnet publish ...
```

# Add user secret on console application
By default the path of user secrets is ```C:\\Users\\<user>\\AppData\\Roaming\\Microsoft\\UserSecrets\\<secretID>\\secrets.json```.
By launching the command ```dotnet user-secrets list```, we can have the list of current secrets

Sample of secrets.json file:
```json
{
  "FirstBackgroundService": {
    "IsEnable": false,
    "DelayInSeconds": 3
  },
  "SecondBackgroundService": {
    "IsEnable": false,
    "DelayInSeconds": 2
  }
}
```

# Has one AppSttings by environment
There are several files to modify to get appsettings.Developpement.json working
* launch.json (used by Visual Code Debug)
* launchSettings.json (used by Visual Studio Debug and commmand line like ```dotnet run --project Worker --launch-profile Worker-Dev```)
* appsettings(.Developpement).json
* Program.cs (to add the logic getting the right appsettings file)

# Todo
* specific folder in Event Viewer 
* unit tests