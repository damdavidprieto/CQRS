FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/CQRS.API/TodoCQRS.API.csproj", "src/CQRS.API/"]
COPY ["src/CQRS.Application/TodoCQRS.Application.csproj", "src/CQRS.Application/"]
COPY ["src/CQRS.Domain/TodoCQRS.Domain.csproj", "src/CQRS.Domain/"]
COPY ["src/CQRS.Infrastructure/TodoCQRS.Infrastructure.csproj", "src/CQRS.Infrastructure/"]

RUN dotnet restore "src/CQRS.API/CQRS.API.csproj"
COPY . .
WORKDIR "/src/src/CQRS.API"
RUN dotnet build "CQRS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CQRS.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 443
EXPOSE 80
ENTRYPOINT ["dotnet", "CQRS.API.dll"]
