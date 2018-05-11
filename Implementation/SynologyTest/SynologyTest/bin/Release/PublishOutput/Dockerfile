FROM microsoft/aspnetcore:2.0.0

COPY . /app
WORKDIR /app

ENV ASPNETCORE_URLS http://*:7500
EXPOSE 7500

ENTRYPOINT ["dotnet","SynologyTest.dll"]