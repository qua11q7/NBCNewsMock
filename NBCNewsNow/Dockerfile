# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
EXPOSE 80
EXPOSE 443
COPY NBCNewsNow.sln .
COPY NBCNewsNow/*.csproj NBCNewsNow/
COPY NNNDataContext/*.csproj NNNDataContext/
COPY NNNDataModel/*.csproj NNNDataModel/
COPY NNNLogger/*.csproj NNNLogger/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/NBCNewsNow
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "NBCNewsNow.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet NBCNewsNow.dll