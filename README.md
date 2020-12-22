# Coding4Fun-Elsa
Coding4Fun to present Elsa workflow

If you want to known more about Elsa -> [here](https://elsa-workflows.github.io/elsa-core/docs/installing-elsa-core)

## Presentation

[Click here](/src/Presentation/index.html)

### Requirement

--------------------------

- .Net 5
- Visual Studio Code

### Create a blazor server project

--------------------------

```powershell
 dotnet new blazorserver -o Elsa.Blazor.DemoClient --no-https  
 ```
 
 ### Add Elsa nuget package

 --------------------------
 
 
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
```

### Add SQL Lite persistence

--------------------------


**Create db directory**

`mkdir C:\data`

Add connection string in your app settings

```
"ConnectionStrings": {
    "SqlLite": "Data Source=c:\\data\\elsa.db;Cache=Shared;"
  }
```

**Configure persistence dependencies**

```csharp
 services
         .AddElsa(elsa => elsa
         .AddEntityFrameworkStores<SqliteContext>(options => options
                                     .UseSqlite(Configuration.GetConnectionString("SqlLite"))))      
```

Add **ElsaContext** as parameter in the **Configure** method

```
 public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ElsaContext elsaDbcontext)
```
And add db creation statement like this to allow EF Core to create tables

```
if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                elsaDbcontext.Database.EnsureCreated();
            }
```

### Add dashboard for ELSA

--------------------------

**Execute** `dotnet add package Elsa.Dashboard`

**Add dependecies configuration**

```csharp
services.AddElsaDashboard();
```
**Configure endpoints for Elsa dashboard**

**Add** `endpoints.MapControllers();`

### Run your app

-----------------------

 goto http://localhost:{port-user-by-kestrel}/elsa/home


 You can create worklflow but don't do that now. Because you not add any activities.
 
### Add activities

To use built in activites from designer add this packages:

```
dotnet add package Elsa.Activities.Console
dotnet add package Elsa.Activities.Timers
dotnet add package Elsa.Activities.Http
dotnet add package Elsa.Activities.Email
dotnet add package Elsa.Activities.ControlFlow
dotnet add package Elsa.Activities.Workflows 
dotnet add package Elsa.Activities.UserTask
```

Declare **Activites for dependency injection**. Go to the Startup.cs in the ConfigureServices method and add

```
public void ConfigureServices(IServiceCollection services)
{
          .....
//Configure dependencies injection for activities
            services
                    .AddTimerActivities()
                    .AddConsoleActivities()
                    .AddWorkflowActivities()
                    .AddEmailActivities()
                    .AddControlFlowActivities()
                    .AddUserTaskActivities()
                    .AddHttpActivities();
                    ....
```

App middleware for http activities in the configure method


```
  public void Configure(IApplicationBuilder app)
  {
    ...
       app.UseHttpActivities();
       app.UseTimersActivities();
       ...
  }
```

### It's time to use workflow :D

Go to the dashboard of elsa /elsa/home
Click on workflows and on the button Create workflow
The designer will open

In this exercize we will a very basic workflow

Call "it hello world from the web".

Add an http request activity that handle message from the route /console
Check the case read content

After that add a Variable. Name it Message and set the content with a javascript expression queryString("text").

Add Console write activity and set text of the context with the content of variable Body.

Connect all your activities that you added.

Publish your workflow. Go to the bottom buggy scroll page ;).

### No we will the power of workflow ;)

We will try to trigger two activities in parallel when we receive the http request.

Reopen the previous workflow and add http redirect activity. set the url to http://www.staggeringbeauty.com/

Add a fork activity. The fork activity allow you to make branch. You need to specify branch name separate by a ","

You can see we don't need to recompile anything. All the modification is made at runtime. So you can add or modify logic directly here.

### Play with timer

Create a new workflow call it intervall, don't forget to made it as singleton. 
If you don't do that the workflow instance will ebe destroy after the last ativity ran to completion.

Add a Timer intervall

Add a Console write line in the text set the text to `It's now ${new Date()}. Let's do this thing!`

Link the two activity and publish the workflow. See your debug console. You will see a message with the date update each 5seconds.

In the next episode we will try to invoke by code.







