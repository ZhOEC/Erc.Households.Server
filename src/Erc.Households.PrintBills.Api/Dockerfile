#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ARG http_proxy
ARG https_proxy
RUN apt-get update && apt-get install -y libgdiplus

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Erc.Households.PrintBills.Api/Erc.Households.PrintBills.Api.csproj", "Erc.Households.PrintBills.Api/"]
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