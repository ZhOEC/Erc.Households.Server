#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG HTTP_PROXY=http://10.67.1.66:3128
ARG HTTPS_PROXY=http://10.67.1.66:3128
WORKDIR /src
COPY ["Erc.Households.Server.Api/Erc.Households.Api.csproj", "Erc.Households.Server.Api/"]
COPY ["Erc.Households.CommandHandlers/Erc.Households.CommandHandlers.csproj", "Erc.Households.CommandHandlers/"]
COPY ["Erc.Households.Server.DataAccess.EF/Erc.Households.EF.PostgreSQL.csproj", "Erc.Households.Server.DataAccess.EF/"]
COPY ["Erc.Households.Server.Domain/Erc.Households.Domain.csproj", "Erc.Households.Server.Domain/"]
COPY ["Erc.Households.Server.Events/Erc.Households.Events.csproj", "Erc.Households.Server.Events/"]
COPY ["Erc.Households.Server.ModelLogs/Erc.Households.Server.ModelLogs.csproj", "Erc.Households.Server.ModelLogs/"]
COPY ["Erc.Households.Commands/Erc.Households.Commands.csproj", "Erc.Households.Commands/"]
COPY ["Erc.Households.Notifications/Erc.Households.Notifications.csproj", "Erc.Households.Notifications/"]
COPY ["Erc.Households.Server.Core/Erc.Households.DataAccess.Core.csproj", "Erc.Households.Server.Core/"]
COPY ["Erc.Households.BranchOffices.Managment.Core/Erc.Households.BranchOfficeManagment.Core.csproj", "Erc.Households.BranchOffices.Managment.Core/"]
COPY ["Erc.Households.NotificationHandlers/Erc.Households.NotificationHandlers.csproj", "Erc.Households.NotificationHandlers/"]
COPY ["Erc.Households.BranchOfficeManagment.EF/Erc.Households.BranchOfficeManagment.EF.csproj", "Erc.Households.BranchOfficeManagment.EF/"]
COPY ["Erc.Households.Server.DataAccess/Erc.Households.DataAccess.EF.csproj", "Erc.Households.Server.DataAccess/"]
RUN dotnet restore "Erc.Households.Server.Api/Erc.Households.Api.csproj"
COPY . .
WORKDIR "/src/Erc.Households.Server.Api"
RUN dotnet build "Erc.Households.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erc.Households.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erc.Households.Api.dll"]