FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ReadySetGo/ReadySetGo.csproj ReadySetGo/
COPY ReadySetGo.Library/ReadySetGo.Library.csproj ReadySetGo.Library/
RUN dotnet restore ReadySetGo/ReadySetGo.csproj
COPY . .
WORKDIR /src/ReadySetGo
RUN dotnet build ReadySetGo.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ReadySetGo.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_URLS=http://*:5001
ENV SPOTIFY_CLIENT_ID=4b0fcb4ba28842529cc2cf6a001d26ab
ENV SPOTIFY_CLIENT_SECRET=be64a5813f0f416380308d095031fd23
EXPOSE 5001
ENTRYPOINT ["dotnet", "ReadySetGo.dll"]
