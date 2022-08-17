FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

WORKDIR /src
COPY . .
RUN dotnet restore ". Backend/Backend.csproj"
WORKDIR "/src/BuySellApi"
RUN dotnet build "Backend.csproj" -c Release -o /app

FROM build AS publish
WORKDIR "/src/Backend"
RUN dotnet publish "Backend.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build /webapp/out .
ENTRYPOINT ["dotnet","Backend.dll"]