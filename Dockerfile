#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#WORKDIR /src
#COPY ["./UserInfo.csproj", "UserInfo/"]
#RUN dotnet restore "UserInfo/UserInfo.csproj"
#COPY . .
#WORKDIR "/source/"
#RUN dotnet build "UserInfo.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "UserInfo.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "UserInfo.dll"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
CMD ["dotnet", "UserInfo.dll"]
#ENTRYPOINT ["dotnet", "UserInfo.dll"]