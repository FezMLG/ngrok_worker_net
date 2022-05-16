# ngrok_worker_net

App is checking ngrok api for new tunnels and sending email with new url.
App is working as worker every minute.

## Starting an app

- Install [.NET Runtime >=6.0.5](https://dotnet.microsoft.com/en-us/download/dotnet)
- Go to `ngrok_worker_net` folder.
- Duplicate `appsettings.json` file and rename the duplicate to `appsettings.Local.json`
- Fill in `appsettings.Local.json` file
- Run app
