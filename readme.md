# POSEIDON

## Description
Poseidon is a C# Backend solution for connected swimmingpool developed with [.NET Core 2](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.0.0-download.md) hosted on a IIS Server.

The solution include 2 projects
* **Poseidon**, the API Solution
* **PoseidonFA**, the Azure Function App that will process data sending by the devices


## Getting Started
Before using Poseidon API Solution, you need to install an SQLServer Database

### Configure Poseidon
Add the *ConnectionStrings:DefaultConnection* and *IssuerSigningKey:SigningKey* into `secrets.json`<sup>1</sup> file with these lines
```json
{
  "IssuerSigningKey": {
    "SigningKey":  "<your issuer signin key here>"
  },
  "ConnectionStrings": {
    "DefaultConnection": "<your sqlserver connection string>"
  }
}
```
*<sup>1</sup> For access to `secrets.json` right click on the project and click on `Manage User Secrets`*

### Configure PoseidonFA
VisualStudio add `local.settings.json` file to the `.gitignore` for the FunctionApp. So, add `local.settings.json` to the PoseidonFA project and configure the connection string and battery level alarm with these lines
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "AzureWebJobsDashboard": "",
    "DefaultConnectionString": "<your connection string here>",
    "DefaultDbName": "poseidon",
    "BatteryLevelAlarm":  20
  }
}
```

### Start the solution
If you want to use PoseidonFA, you have to set the startup project to mutliple startup projects ; else make sur that Poseidon is set as a startup project. Run the solution by clicking on `Start` button. 

Once launching, your browser must be opened with this url : `http://localhost:64705/api/swagger` and the Azure Function App console should be opened. You can use it via [Postman](https://www.getpostman.com/) with this url : `http://localhost:7071/api/PostMeasures` (for body params, see *[IncomingMeasures.cs](PoseidonFA/Payload/IncomingMeasures.cs)*)

Thanks to the SwaggerUI you can see and test all endpoints of Poseidon


## Troubleshooting
### C# Error : Could not load file or assembly 'System.Runtime.InteropServices.RuntimeInformation' (Azure Function App only)
You probably update MongoDB.Driver to 2.4 or higher. Downgrade to 2.3 and the issue must be fixed

*[Connecting to MongoDB from azure function
 on StackOverlow](https://stackoverflow.com/questions/42045678/connecting-to-mongodb-from-azure-function)*