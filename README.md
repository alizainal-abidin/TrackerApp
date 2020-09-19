# Issue Tracker 
a RESTful API for an issue tracking API.

## Run Project
You can either run this application using these 2 options:

- Launching the solution from Visual Studio 2019. <br>Set "Multiple Startup Projects" by right-click the Solution and choose:
  - IdentityServer, and
  - Tracker.App.Api

  then just press F5.

- using .NET Core CLI

  From the root directory, navigate to:
  #### `> cd src/Tracker.App.Api && dotnet build` 
  <br>and

  #### `> cd src/IdentityServer && dotnet build`

  This will install all the dependencies needed to run this application.<br>

  #### `> dotnet run --project src/Tracker.App.Api/Tracker.App.Api.csproj`
  #### `> dotnet run --project src/IdentityServer/IdentityServer.csproj`
  <br>

  It will run the app in the development mode.<br>
Open [https://localhost:5001](https://localhost:5001) to view the IdentityServer project in the browser, and <br>
[https://localhost:5003](https://localhost:5003) to view the Tracker API project.

## Run Unit Test
#### `> ls test/**/*.csproj | xargs -L1 dotnet test`