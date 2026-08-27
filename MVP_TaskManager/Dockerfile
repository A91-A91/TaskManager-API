FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["MVP_TaskManager.csproj", "."]
RUN dotnet restore "MVP_TaskManager.csproj"

COPY . .
RUN dotnet publish "MVP_TaskManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "MVP_TaskManager.dll"]