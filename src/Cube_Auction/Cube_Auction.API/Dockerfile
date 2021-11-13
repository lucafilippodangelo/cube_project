#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Cube_Auction/Cube_Auction.API/Cube_Auction.API.csproj", "src/Cube_Auction/Cube_Auction.API/"]
RUN dotnet restore "src/Cube_Auction/Cube_Auction.API/Cube_Auction.API.csproj"
COPY . .
WORKDIR "/src/src/Cube_Auction/Cube_Auction.API"
RUN dotnet build "Cube_Auction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cube_Auction.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cube_Auction.API.dll"]