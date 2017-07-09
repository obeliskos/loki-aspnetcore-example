# loki-aspnetcore

### Sample application stack using asp.net core, nodeservices, and lokijs for persistence.


This project uses a basic mvc scaffold to implement an ASP.NET MVC application which uses lokijs (running in nodejs) as a database backend. Server-side, you can define your C# classes and directly pass those to/from the lokiservice layer or stay within the dynamic realm to deal with objects more similarly to javascript.

Example retrieving all Users stored in the 'users' collection :
```cs
List<User> result = await db.Find<User>("users");
```

With server-side chaining (implemented using loki transforms) your queries may look something like : 
```cs
List<User> users = await db.Chain("users")
    .Find("{ age: { $gte: 50 }}")
    .SimpleSort("age", true)
    .Limit(3)
    .Data<User>();
```

At the moment, query expressions are still passed in as json strings due to both the dynamic structure of the object and certain symbols not being valid as the first character of a c# property name.

The node.js service layer supports several simulatenously loaded database, each initialized according to a 'service initializer class' which you would create.  The service initializer class is a javascript module which acts a factory for lokijs databases.  Your module constructor should accept a filename, instantiate a lokijs database with settings of your choice (for that initializer) and set up any transforms, dynamic views, or seed data.  All interactions with your created databases have performance info captured and overall global configuration and statistics can be reported on.

### ASP.Net Core NodeServices
For more information view [Microsoft's description](https://github.com/aspnet/JavaScriptServices/tree/dev/src/Microsoft.AspNetCore.NodeServices#microsoftaspnetcorenodeservices) or this [Adjen Towfeek's youtube tutorial](https://www.youtube.com/watch?v=Vg9gDsfV7Oo). NodeServices is a Microsoft-created nuget package library allow asp.net to call into nodejs.  Initial call into node takes a second to spin up but, once running, it will be retained in memory.  This allows our javascript module (and your initializers) to initialize and establish it's state, implement autosave timers, provide module export functions for interfacing with lokijs,  and accumulating statistics for database uptime and request counts.

### .Net Core SDK 1.1
To run this, you will need to install the [.NET Core SDK 1.1](https://www.microsoft.com/net/download/core) for your platform (Windows/OSX/Linux)

After cloning, you should be able to open a termin within project directory and type :
```
dotnet restore
npm install
dotnet run
```
Once the web server spins up you can view the home page from your browser at http://localhost:5000

This project was created with the help of visual studio code and has 'development' launch options set to show detailed error stack traces by default.

I attempted to scaffold this project with minimal dependecies.  If you wish to re-engineer this is on your own this was my general process :
```
mkdir loki-aspnetcore
cd loki-aspnetcore
dotnet new mvc
dotnet restore
npm init
npm install --save lokijs
```
I then :
- ran [Visual Studio Code](https://code.visualstudio.com/) to load the directory
- added nodeservices reference to the csproj and then ran dotnet restore to pull that library down for use
- modified Startup.cs to register nodeservices (in ConfigureServices) for later injection into controller actions.
- i added my node module script and put it under an arbitrarily named 'nodesvcs' folder
- i added a new LokiServiceController (mvc controller), implementing a single (model-less for now) view and several json service actions
- created my LokiService folder (under 'Views') for storing razor markup view pages

This will remain a work in progress and an evolving experiment experimenting with Nodeservices and lokijs.