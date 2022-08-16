FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CurseRinth/CurseRinth.csproj", "CurseRinth/"]
RUN dotnet restore "CurseRinth/CurseRinth.csproj"
COPY . .
WORKDIR "/src/CurseRinth"
RUN dotnet build "CurseRinth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurseRinth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurseRinth.dll"]
