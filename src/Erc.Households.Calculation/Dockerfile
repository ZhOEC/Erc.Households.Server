#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG HTTP_PROXY=http://10.67.2.49:50514
ARG HTTPS_PROXY=http://10.67.2.49:50514
ARG NO_PROXY=http://10.67.1.234

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
RUN dotnet restore --configfile NuGet.Config ./"Erc.Households.Calculation/Erc.Households.Calculation.csproj"

WORKDIR "/src/Erc.Households.Calculation"
RUN dotnet build "Erc.Households.Calculation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erc.Households.Calculation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erc.Households.Calculation.dll"]