FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Solution.Api/Solution.Api.csproj", "src/Solution.Api/"]
RUN dotnet restore "src/Solution.Api/Solution.Api.csproj"
COPY . .
WORKDIR "/src/src/Solution.Api"
RUN dotnet build "Solution.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Solution.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Solution.Api.dll"]
