FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG HTTP_PROXY=http://10.67.2.49:50514
ARG HTTPS_PROXY=http://10.67.2.49:50514

COPY . .
RUN dotnet restore "Erc.Households.Indexing/Erc.Households.Indexing.csproj"
WORKDIR "Erc.Households.Indexing"
RUN dotnet publish "Erc.Households.Indexing.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Erc.Households.Indexing.dll"]