# Coding4Fun-Elsa
Coding4Fun to present Elsa workflow

###Requirement

.Net 5
Visual Studio Code

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

Configure endpoints for Elsa dashboard

**Add** `endpoints.MapControllers();`
