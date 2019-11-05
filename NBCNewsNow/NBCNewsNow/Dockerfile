#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-nanoserver-1803 AS build
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