FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY *.sln .
COPY ./src/BackendChallenge/*.fsproj ./BackendChallenge/
RUN dotnet restore ./BackendChallenge/

COPY ./src/BackendChallenge/. ./BackendChallenge/
WORKDIR /source/BackendChallenge
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "BackendChallenge.Main.dll"]
