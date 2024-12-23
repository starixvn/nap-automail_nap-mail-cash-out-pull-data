FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src

COPY ./Fanex.COPullData.Service/Fanex.COPullData.Business ./Fanex.COPullData.Business
COPY ./Fanex.COPullData.Service/Fanex.COPullData.Service/Fanex.COPullData.Service.csproj ./Fanex.COPullData.Service/Fanex.COPullData.Service.csproj

RUN ls /src -l
WORKDIR /src/Fanex.COPullData.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Fanex.COPullData.Service/Fanex.COPullData.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-nap-mail-core



# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.COPullData.Service.dll"]
