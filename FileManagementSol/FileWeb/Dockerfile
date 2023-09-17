FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FileWeb/FileWeb.csproj", "FileWeb/"]
RUN dotnet restore "FileWeb/FileWeb.csproj"
COPY . .
WORKDIR "/src/FileWeb"
RUN dotnet build "FileWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileWeb.dll"]