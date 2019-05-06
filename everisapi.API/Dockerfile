FROM microsoft/dotnet:sdk

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
#RUN dotnet publish -c Release -o out
RUN dotnet publish -o out

WORKDIR /app/out

ENTRYPOINT ["dotnet", "everisapi.API.dll"]