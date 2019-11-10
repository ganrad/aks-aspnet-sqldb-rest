FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "ClaimsApi.dll"]
