﻿# POSEIDON

## Description
Poseidon is a C# Backend solution for connected swimmingpool developed with .NET Core Framework hosted on a IIS Server. It will be used by the futurs apps [Percy]() and [Sally]()


## Getting Started
Before using Poseidon API Solution, you need to install and run [MongoDB](https://docs.mongodb.com/v3.0/tutorial/install-mongodb-on-windows/) NoSQL Database. Once started, run MongoDb Shell and copy/paste content from `Poseidon\Resources\InitialInsert.txt` file (or use it to create your own data sets) into it.

### Connect Poseidon to the database
Add the MongoDbSettings into `appsettings.json` file with these lines
```json
"MongoDbSettings": {
  "DefaultConnectionString": "<your connection string here>",
  "DefaultDbName":  "poseidon"
}
```

### Sign the JWT Token
Add the IssuerSigninKey into `appsettings.json` file with these lines
```json
"IssuerSigningKey": {
    "SigningKey":  "<your issuer signin key here>"
}
```

### Launch Poseidon
From VisualStudio, make sur that Poseidon is set as a Startup project and run it by clicking on `Start` button. Once launching, your browser must be opened with this url : `http://localhost:64705/api/swagger`

Thanks to the SwaggerUI you can see and test all endpoints of Poseidon


## Troubleshooting
### Poseidon is running but it return nothings
It's possible that you missed a step in the configuration. Make sur that you have correctly configure the `appsettings.json` file, in particular the `DefaultDbName` that must be the same name used in the `Poseidon\Resources\InitialInsert.txt` file
```
use poseidon
 
db.users.insert(
...
```
```json
"MongoDbSettings": {
  "DefaultConnectionString": "<your connection string here>",
  "DefaultDbName":  "poseidon"
}
```