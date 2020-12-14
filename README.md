# Coding4Fun-Elsa
Coding4Fun to present Elsa workflow

If you want to known more about Elsa -> [here](https://elsa-workflows.github.io/elsa-core/docs/installing-elsa-core)

### Requirement

- .Net 5
- Visual Studio Code

### Create a blazor server project

```powershell
 dotnet new blazorserver -o Elsa.Blazor.DemoClient --no-https  
 ```
 
 ### Add Elsa nuget package
 
 
```powershell
cd .\src\Elsa.Blazor.DemoClient\
dotnet add package Elsa
dotnet add package Elsa.Dashboard
dotnet build
 ```
Go to StartupFile and configure dependency injection for ELSA

```csharp
 public void ConfigureServices(IServiceCollection services)
 {
    services.AddRazorPages();
     ...
    services.AddElsa();
 }
````


Add SQL Lite persistence

```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Elsa.Persistence.EntityFrameworkCore
```

Configure persistence 

```csharp
 services
         .AddElsa(elsa => elsa
         .AddEntityFrameworkStores<SqliteContext>(options => options
                           .UseSqlite(@"Data Source=C:\data\elsa.db;Cache=Shared")));
```

Configure endpoints for Elsa dashboard

**Add** `endpoints.MapControllers();`


Run your app and goto http://localhost:{port-user-by-kestrel}/elsa/home