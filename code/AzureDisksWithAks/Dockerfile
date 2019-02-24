FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o output

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
EXPOSE 80

COPY --from=build-env /app/output .
ENTRYPOINT ["dotnet", "AzureDisksWithAks.dll"]