#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TaskV3.Service/TaskV3.Service.csproj", "TaskV3.Service/"]
COPY ["TaskV3.Business/TaskV3.Business.csproj", "TaskV3.Business/"]
COPY ["TaskV3.Core/TaskV3.Core.csproj", "TaskV3.Core/"]
COPY ["TaskV3.Repositories/TaskV3.Repositories.csproj", "TaskV3.Repositories/"]
RUN dotnet restore "TaskV3.Service/TaskV3.Service.csproj"
COPY . .
WORKDIR "/src/TaskV3.Service"
RUN dotnet build "TaskV3.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskV3.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskV3.Service.dll"]