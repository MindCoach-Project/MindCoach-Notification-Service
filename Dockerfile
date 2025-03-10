FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
COPY ./MinhCoach_Notification_Service.Api/MinhCoach_Notification_Service.Api.csproj Api/
COPY ./MinhCoach_Notification_Service.Contracts/MinhCoach_Notification_Service.Contracts.csproj Contracts/
COPY ./MinhCoach_Notification_Service.Infra/MinhCoach_Notification_Service.Infra.csproj Infra/
COPY ./MinhCoach_Notification_Service.App/MinhCoach_Notification_Service.App.csproj App/
COPY ./MinhCoach_Notification_Service.Domain/MinhCoach_Notification_Service.Domain.csproj Domain/

RUN dotnet restore "MinhCoach_Notification_Service.sln"

COPY ./MinhCoach_Notification_Service.Api/ Api/
COPY ./MinhCoach_Notification_Service.Contracts/ Contracts/
COPY ./MinhCoach_Notification_Service.Infra/ Infra/
COPY ./MinhCoach_Notification_Service.App/ App/
COPY ./MinhCoach_Notification_Service.Domain/ Domain/

WORKDIR /src/Api
RUN dotnet build "MinhCoach_Notification_Service.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinhCoach_Notification_Service.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
RUN addgroup --system --gid 1001 dotnet
RUN adduser --system --uid 1001 csharp
COPY --from=publish /app/publish .
RUN chown -R csharp:dotnet /app && chmod -R 750 /app
USER csharp
EXPOSE 8080
ENTRYPOINT ["dotnet", "MinhCoach_Notification_Service.Api.dll"]
