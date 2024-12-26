FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SharePay.APIFunctions/SharePay.APIFunctions.csproj", "SharePay.APIFunctions/"]
COPY ["SharePay.Business/SharePay.Business.csproj", "SharePay.Business/"]
COPY ["SharePay.Models/SharePay.Models.csproj", "SharePay.Models/"]
COPY ["SharePay.Repository/SharePay.Repository.csproj", "SharePay.Repository/"]
RUN dotnet restore "SharePay.APIFunctions/SharePay.APIFunctions.csproj"
COPY . .
WORKDIR "/src/SharePay.APIFunctions"
RUN dotnet build "SharePay.APIFunctions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SharePay.APIFunctions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SharePay.APIFunctions.dll"]