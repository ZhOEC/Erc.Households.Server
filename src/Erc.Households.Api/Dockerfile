#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build

COPY . .
WORKDIR "Erc.Households.Api"
RUN dotnet restore 

FROM build AS publish

RUN dotnet publish -c Release -o /app/publish

FROM base AS final
ENV TZ=Europe/Kiev
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erc.Households.Api.dll"]