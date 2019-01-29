FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY PoseidonFA.csproj .
RUN dotnet restore PoseidonFA.csproj
COPY . .
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM microsoft/azure-functions-dotnet-core2.0:2.0 as final
ENV AzureWebJobsScriptRoot=/home/site/wwwroot
COPY --from=publish /app /home/site/wwwroot