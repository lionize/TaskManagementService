#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TaskManagementService/TaskManagementService.csproj", "TaskManagementService/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "TaskManagementService/TaskManagementService.csproj"
COPY . .
WORKDIR "/src/TaskManagementService"
RUN dotnet build "TaskManagementService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagementService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TIKSN.Lionize.TaskManagementService.dll"]