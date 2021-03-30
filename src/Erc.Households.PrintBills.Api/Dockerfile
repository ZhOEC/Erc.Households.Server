#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ARG HTTP_PROXY=http://10.67.2.49:50514
ARG HTTPS_PROXY=http://10.67.2.49:50514

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["nuget.config", "."]
COPY ["Erc.Households.PrintBills.Api/Erc.Households.PrintBills.Api.csproj", "Erc.Households.PrintBills.Api/"]
COPY ["Erc.Households.Api/Erc.Households.Api.csproj", "Erc.Households.Api/"]
COPY ["Erc.Households.UsageParser.Xlsx.NaturalGas/Erc.Households.ConsumptionParser.Xlsx.NaturalGas.csproj", "Erc.Households.UsageParser.Xlsx.NaturalGas/"]
COPY ["Erc.Households.ConsumptionParser.Core/Erc.Households.ConsumptionParser.Core.csproj", "Erc.Households.ConsumptionParser.Core/"]
COPY ["Erc.Households.CommandHandlers/Erc.Households.CommandHandlers.csproj", "Erc.Households.CommandHandlers/"]
COPY ["Erc.Households.Server.DataAccess.EF/Erc.Households.EF.PostgreSQL.csproj", "Erc.Households.Server.DataAccess.EF/"]
COPY ["Erc.Households.Server.Domain/Erc.Households.Domain.csproj", "Erc.Households.Server.Domain/"]
COPY ["Erc.Households.Domain.Shared/Erc.Households.Domain.Shared.csproj", "Erc.Households.Domain.Shared/"]
COPY ["Erc.Households.Domain.Extensions/Erc.Households.Domain.Extensions.csproj", "Erc.Households.Domain.Extensions/"]
COPY ["Erc.Households.Server.Events/Erc.Households.Events.csproj", "Erc.Households.Server.Events/"]
COPY ["Erc.Households.CalculateStrategies.Core/Erc.Households.CalculateStrategies.Core.csproj", "Erc.Households.CalculateStrategies.Core/"]
COPY ["Erc.Households.Server.ModelLogs/Erc.Households.Server.ModelLogs.csproj", "Erc.Households.Server.ModelLogs/"]
COPY ["Erc.Households.Commands/Erc.Households.Commands.csproj", "Erc.Households.Commands/"]
COPY ["Erc.Households.Notifications/Erc.Households.Notifications.csproj", "Erc.Households.Notifications/"]
COPY ["Erc.Households.Server.Core/Erc.Households.DataAccess.Core.csproj", "Erc.Households.Server.Core/"]
COPY ["Erc.Households.BranchOffices.Managment.Core/Erc.Households.BranchOfficeManagment.Core.csproj", "Erc.Households.BranchOffices.Managment.Core/"]
COPY ["Erc.Households.NotificationHandlers/Erc.Households.NotificationHandlers.csproj", "Erc.Households.NotificationHandlers/"]
COPY ["Erc.Households.BranchOfficeManagment/Erc.Households.BranchOfficeManagment.csproj", "Erc.Households.BranchOfficeManagment/"]
COPY ["Erc.Households.Server.DataAccess/Erc.Households.DataAccess.EF.csproj", "Erc.Households.Server.DataAccess/"]
RUN dotnet restore "Erc.Households.PrintBills.Api/Erc.Households.PrintBills.Api.csproj"
COPY . .
WORKDIR "/src/Erc.Households.PrintBills.Api"
RUN dotnet build "Erc.Households.PrintBills.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erc.Households.PrintBills.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erc.Households.PrintBills.Api.dll"]