FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY TaskV3.Core/*.csproj ./TaskV3.Core/
COPY TaskV3.Repositories/*.csproj ./TaskV3.Repositories/
COPY TaskV3.Business/*.csproj ./TaskV3.Business/
COPY TaskV3.Service/*.csproj ./TaskV3.Service/
COPY TaskV3.Test/*.csproj ./TaskV3.Test/
RUN dotnet restore

# copy everything else and build app
COPY TaskV3.Core/. ./TaskV3.Core/
COPY TaskV3.Repositories/. ./TaskV3.Repositories/
COPY TaskV3.Business/. ./TaskV3.Business/
COPY TaskV3.Service/. ./TaskV3.Service/
COPY TaskV3.Test/. ./TaskV3.Test/

# run tests on docker build
RUN dotnet test

WORKDIR /app/TaskV3
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/TaskV3/out ./
ENTRYPOINT ["dotnet", "TaskV3.Service.dll"]

