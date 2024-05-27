FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroRabbit.Transfer.Api/MicroRabbit.Transfer.Api.csproj", "MicroRabbit.Transfer.Api/"]
COPY ["MicroRabbit.Infra.IoC/MicroRabbit.Infra.IoC.csproj", "MicroRabbit.Infra.IoC/"]
COPY ["MicroRabbit.Banking.Application/MicroRabbit.Banking.Application.csproj", "MicroRabbit.Banking.Application/"]
COPY ["MicroRabbit.Banking.Domain/MicroRabbit.Banking.Domain.csproj", "MicroRabbit.Banking.Domain/"]
COPY ["MicroRabbit.Domain.Core/MicroRabbit.Domain.Core.csproj", "MicroRabbit.Domain.Core/"]
COPY ["MicroRabbit.Banking.Data/MicroRabbit.Banking.Data.csproj", "MicroRabbit.Banking.Data/"]
COPY ["MicroRabbit.Infra.Bus/MicroRabbit.Infra.Bus.csproj", "MicroRabbit.Infra.Bus/"]
COPY ["MicroRabbit.Transfer.Data/MicroRabbit.Transfer.Data.csproj", "MicroRabbit.Transfer.Data/"]
COPY ["MicroRabbit.Transfer.Domain/MicroRabbit.Transfer.Domain.csproj", "MicroRabbit.Transfer.Domain/"]
RUN dotnet restore "MicroRabbit.Transfer.Api/MicroRabbit.Transfer.Api.csproj"
COPY . .
WORKDIR "/src/MicroRabbit.Transfer.Api"
RUN dotnet build "MicroRabbit.Transfer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MicroRabbit.Transfer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroRabbit.Transfer.Api.dll"]
