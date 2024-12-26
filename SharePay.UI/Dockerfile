FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SharePay.UI/SharePay.UI.csproj", "SharePay.UI/"]
COPY ["SharePay.Models/SharePay.Models.csproj", "SharePay.Models/"]
RUN dotnet restore "SharePay.UI/SharePay.UI.csproj"
COPY . .
WORKDIR "/src/SharePay.UI"
RUN dotnet build "SharePay.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SharePay.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SharePay.UI.dll"]