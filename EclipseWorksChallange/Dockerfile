#base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#copy
COPY ["EclipseWorksChallange/EclipseWorksChallange.csproj", "EclipseWorksChallange/"]
RUN dotnet restore "EclipseWorksChallange/EclipseWorksChallange.csproj"

COPY . .

#build
WORKDIR "/src/EclipseWorksChallange"
RUN dotnet build "EclipseWorksChallange.csproj" -c $BUILD_CONFIGURATION -o /app/build

#publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EclipseWorksChallange.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

#finsh
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EclipseWorksChallange.dll"]