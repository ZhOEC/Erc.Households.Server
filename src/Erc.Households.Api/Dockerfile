#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG HTTP_PROXY=http://10.67.2.49:50514
ARG HTTPS_PROXY=http://10.67.2.49:50514

COPY . .
RUN dotnet restore "Erc.Households.Api/Erc.Households.Api.csproj"

FROM build AS publish
WORKDIR "Erc.Households.Api"
RUN dotnet publish "Erc.Households.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erc.Households.Api.dll"]