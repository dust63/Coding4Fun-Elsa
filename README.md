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




### Add SQL Lite persistence


Configure persistence dependencies

Add connection string in your app settings

```
"ConnectionStrings": {
    "SqlLite": "Data Source=c:\\data\\elsa.db;Cache=Shared;"
  }
```

Add dependencies injection

```csharp
 services
         .AddElsa(elsa => elsa
         .AddEntityFrameworkStores<SqliteContext>(options => options
                                     .UseSqlite(Configuration.GetConnectionString("SqlLite"))))      
```

Add elsacontext in the configure method

```
 public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ElsaContext elsaDbcontext)
```
And add db creation statement like this

```
if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                elsaDbcontext.Database.EnsureCreated();
            }
```
### Add dashboard for ELSA

**Run** `dotnet add package Elsa.Dashboard`

**Add dependecies configuration**

```csharp
services.AddElsaDashboard();
```
**Configure endpoints for Elsa dashboard**

**Add** `endpoints.MapControllers();`

Run your app and goto http://localhost:{port-user-by-kestrel}/elsa/home