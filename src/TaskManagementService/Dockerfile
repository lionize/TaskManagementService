FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TaskManagementService/TaskManagementService.csproj", "TaskManagementService/"]
RUN dotnet restore "TaskManagementService/TaskManagementService.csproj"
COPY . .
WORKDIR "/src/TaskManagementService"
RUN dotnet build "TaskManagementService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TaskManagementService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TIKSN.Lionize.TaskManagementService.dll"]
