# POSEIDON

## Description
Poseidon is a C# Backend solution for connected swimmingpool developed with [.NET Core 2](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.0.0-download.md) hosted on a IIS Server or Docker Container.

The solution include 2 projects
* **Poseidon**, the API Solution
* **PoseidonFA**, the Azure Function App that will process data sending by the devices

## Getting Started
Before using Poseidon API Solution locally, you need to install an SQLServer Database

If you want to run it on Azure, you can use the Azure Button to deploy all necessary resources

[![Deploy to Azure](https://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

### Configure Poseidon
Add the *ConnectionStrings:DefaultConnection* and *IssuerSigningKey:SigningKey* into `secrets.json`<sup>1</sup> file with these lines
```json
{
  "IssuerSigningKey": {
    "SigningKey":  "<your_issuer_signin_key_here>"
  },
  "ConnectionStrings": {
    "DefaultConnection": "<your_sqlserver_connection_string>"
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
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "BatteryLevelAlarm": 20
  },
  "ConnectionString": {
    "DefaultConnection": "<your_sqlserver_connection_string>"
  }
}
```

### Start the solution
If you want to use PoseidonFA, you have to set the startup project to mutliple startup projects ; else make sur that Poseidon is set as a startup project. Run the solution by clicking on `Start` button. 

Once launching, your browser must be opened with this url : `http://localhost:64705/api/swagger` and the Azure Function App console should be opened. You can use it via [Postman](https://www.getpostman.com/) with this url : `http://localhost:7071/api/Telemetries?poolid=<your_pool_id>` (for body params, see *[TelemetriesSetDto.cs](PoseidonFA/Dtos/TelemetriesSetDto.cs)*)

Thanks to the SwaggerUI you can see and test all endpoints of Poseidon.