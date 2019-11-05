FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["NBCNewsNow/NBCNewsNow.csproj", "NBCNewsNow/"]
COPY ["NNNDataContext/NNNDataContext.csproj", "NNNDataContext/"]
COPY ["NNNDataModel/NNNDataModel.csproj", "NNNDataModel/"]
COPY ["NNNLogger/NNNLogger.csproj", "NNNLogger/"]
RUN dotnet restore "NBCNewsNow/NBCNewsNow.csproj"
COPY . .
WORKDIR "/src/NBCNewsNow"
RUN dotnet build "NBCNewsNow.csproj" -c Release -o /app/build
RUN dotnet update-database

FROM build AS publish
RUN dotnet publish "NBCNewsNow.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NBCNewsNow.dll"]