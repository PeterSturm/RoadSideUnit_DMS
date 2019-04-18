FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Manager/SNMPManager.WebAPI/SNMPManager.WebAPI.csproj", "Manager/SNMPManager.WebAPI/"]
COPY ["Manager/SNMPManager.Core/SNMPManager.Core.csproj", "Manager/SNMPManager.Core/"]
COPY ["Common/DTO/DTO.csproj", "Common/DTO/"]
COPY ["Manager/SNMPManager.Persistence/SNMPManager.Persistence.csproj", "Manager/SNMPManager.Persistence/"]
COPY ["Manager/SNMPManager.Infrastructure/SNMPManager.Infrastructure.csproj", "Manager/SNMPManager.Infrastructure/"]
RUN dotnet restore "Manager/SNMPManager.WebAPI/SNMPManager.WebAPI.csproj"
COPY . .
WORKDIR "/src/Manager/SNMPManager.WebAPI"
RUN dotnet build "SNMPManager.WebAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SNMPManager.WebAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SNMPManager.WebAPI.dll"]