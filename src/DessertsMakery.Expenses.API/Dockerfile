FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/DessertsMakery.Expenses.API/DessertsMakery.Expenses.API.csproj", "src/DessertsMakery.Expenses.API/"]
RUN dotnet restore "src/DessertsMakery.Expenses.API/DessertsMakery.Expenses.API.csproj"
COPY . .
WORKDIR "/src/src/DessertsMakery.Expenses.API"
RUN dotnet build "DessertsMakery.Expenses.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DessertsMakery.Expenses.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DessertsMakery.Expenses.API.dll"]
