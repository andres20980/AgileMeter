FROM microsoft/dotnet:sdk AS build-env
#ENV HTTP_PROXY "http://10.0.8.102:8080"
#ENV HTTPS_PROXY "http://10.0.8.102:8080"
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
#RUN dotnet publish -c Release -o out
RUN dotnet publish -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
#ENV HTTP_PROXY "http://10.0.8.102:8080"
#ENV HTTPS_PROXY "http://10.0.8.102:8080"
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "everisapi.API.dll"]