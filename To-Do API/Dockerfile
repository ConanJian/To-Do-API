#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["To-Do API/To-Do API.csproj", "To-Do API/"]
RUN dotnet restore "To-Do API/To-Do API.csproj"
COPY . .
WORKDIR "/src/To-Do API"
RUN dotnet build "To-Do API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "To-Do API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "To-Do API.dll"]