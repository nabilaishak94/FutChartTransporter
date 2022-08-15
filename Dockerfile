FROM mcr.microsoft.com/dotnet/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 80
COPY /bin/Release/netcoreapp2.1/publish . 

FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build
WORKDIR /app
COPY /bin/Release/netcoreapp2.1/publish . 

FROM ubuntu
RUN ["apt-get", "update"]
RUN ["apt-get", "install", "-y", "vim"]

FROM base AS final
WORKDIR /app
ENTRYPOINT ["dotnet", "FutChartTransporter_DotCore.dll"]
