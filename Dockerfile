FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./ ./
RUN dotnet restore Data/Data.csproj
RUN dotnet restore Business/Business.csproj
RUN dotnet restore TaskManagementService/TaskManagementService.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out TaskManagementService/TaskManagementService.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TIKSN.Lionize.TaskManagementService.dll"]