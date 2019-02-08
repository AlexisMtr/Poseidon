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
To run the WebAPI on Docker, go in the Poseidon folder and execute
```bash
$ docker build -t poseidon-api .
$ docker run -it --rm
  -e "ConnectionStrings:DefaultConnection=<your_connection_string>"
  -e "IssuerSigningKey:SigningKey=<your_signinkey>"
  -p 8080:80
  poseidon-api
```

### Configure PoseidonFA
#### VisualStudio / VSCode
VisualStudio add `local.settings.json` file to the `.gitignore` for the FunctionApp. So, add `local.settings.json` to the PoseidonFA project and configure the connection string and battery level alarm with these lines
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "BatteryLevelAlarm": 20
  },
  "ConnectionStrings": {
    "DefaultConnection": "<your_sqlserver_connection_string>"
  }
}
```
#### Docker
To run the FunctionApp on Docker, go in the PoseidonFA folder and execute
```bash
$ docker build -t poseidon-fa .
$ docker run -it --rm
  -e "ConnectionStrings:DefaultConnection=<your_connection_string>"
  -e "BatteryLevelAlarm=20"
  -e "AzureFunctionsJobHost__Logging__Console__IsEnabled=true"
  -e "AzureWebJobsSecretStorageType='blob'"
  -e "AzureWebJobsStorage=<your_storage_account>"
  -e "AzureWebJobsDashboard=<your_storage_account>"
  -p 8081:80
   poseidon-fa
```
The 3 last parameters are required to enable `AccesLevel` other than `Anonymous`  
To find the key, explore the storage account and find the key inside blob `azure-webjobs-secrets`>`<folderId>`>`telemetries.json`

### Docker-Compose
#### Using Windows Storage Emulator (Windows OS only)
If you are on Windows OS, you must start the Windows Storage Emulator. Don't touch any things on `docker-compose.yml` and execute
```
$ docker-compose up [--build]
```
#### Using Storage Emulator (All OS)
If your a not on Windows OS (or you don't want to install Storage Emulator), you can use the project [Azurite](https://github.com/Azure/Azurite) which simulate the Storage Emulator (/!\ some features are not implemented). To use it, update the `docker-compose.yml` file like follow
```yml
services:
  #...
  function:
    #...
    environment:
      # ...
      - "AzureWebJobsStorage=..." # replace 'host.docker.internal' by 'storage-emulator'
      - "AzureWebJobsDashboard=..." # replace 'host.docker.internal' by 'storage-emulator'
    depends_on:
      - database
      - storage-emulator

  # Add new service based on Azurite
  storage-emulator:
     image: arafato/azurite
     ports:
       - "10000:10000"
       - "10001:10001"
       - "10002:10002"
     networks: 
       - function-net
     volumes:
       - "function-data:/var/opt/mssql"
```