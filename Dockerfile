# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY M8ScoreTool.sln ./
COPY M8ScoreLibrary/M8ScoreLibrary.csproj M8ScoreLibrary/
COPY M8ScoreTool/M8ScoreTool.csproj M8ScoreTool/
COPY M8PoolScoreTests/M8PoolScoreTests.csproj M8PoolScoreTests/

RUN dotnet restore M8ScoreTool.sln

COPY . .
RUN dotnet publish M8ScoreTool/M8ScoreTool.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "M8ScoreTool.dll"]
