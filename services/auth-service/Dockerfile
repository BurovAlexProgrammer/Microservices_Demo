FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:3001
EXPOSE 3001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["auth-service.csproj", "./"]
RUN dotnet restore "auth-service.csproj"
COPY . .
RUN dotnet build "auth-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "auth-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "auth-service.dll"]
