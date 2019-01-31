# POSEIDON

## Description
Poseidon is a C# Backend solution for connected swimmingpool developed with [.NET Core 2](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.0.0-download.md) hosted on a IIS Server or Docker Container.

The solution include 3 projects
* **Poseidon**, the API Solution
* **PoseidonFA**, the Azure Function App that will process data sending by the devices
* **PoseidonRG**, the ARM project to deploy on Azure

## Getting Started
If you want to run it on Azure, you can use the Azure Button to deploy all necessary resources

[![Deploy to Azure](https://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

You can also use Docker with the `docker-compose.yml` file in the root folder of the solution  
Go inside the root folder and run
```bash
$ docker-compose up [--build]
```
### Configure Poseidon
#### VisualStudio
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

#### Docker
For run the WebAPI on Docker, go in the Poseidon folder and execute
```bash
$ docker build -t poseidon-api .
$ docker run -it --rm -e "ConnectionStrings:DefaultConnection=<your_connection_string>" -e "IssuerSigningKey:SigningKey=<your_signinkey>" poseidon-api
```

### Configure PoseidonFA
#### VisualStudio / VSCode
VisualStudio add `local.settings.json` file to the `.gitignore` for the FunctionApp. So, add `local.settings.json` to the PoseidonFA project and configure the connection string and battery level alarm with these lines
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "BatteryLevelAlarm": 20
  },
  "ConnectionStrings": {
    "DefaultConnection": "<your_sqlserver_connection_string>"
  }
}
```
#### Docker
For run the FunctionApp on Docker, go in the PoseidonFA folder and execute
```bash
$ docker build -t poseidon-fa .
$ docker run -it --rm -e "ConnectionStrings:DefaultConnection=<your_connection_string>" -e "BatteryLevelAlarm=20" poseidon-fa
```