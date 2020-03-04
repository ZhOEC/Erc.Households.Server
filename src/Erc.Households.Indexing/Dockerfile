#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG HTTP_PROXY=http://10.67.2.49:50514
ARG HTTPS_PROXY=http://10.67.2.49:50514

WORKDIR /src
COPY ["Erc.Households.Indexing/Erc.Households.Indexing.csproj", "Erc.Households.Indexing/"]
RUN dotnet restore "Erc.Households.Indexing/Erc.Households.Indexing.csproj"
COPY . .
WORKDIR "/src/Erc.Households.Indexing"
RUN dotnet build "Erc.Households.Indexing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erc.Households.Indexing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erc.Households.Indexing.dll"]