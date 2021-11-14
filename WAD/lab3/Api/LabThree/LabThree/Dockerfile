FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LabThree/LabThree.csproj", "LabThree/"]
RUN dotnet restore "LabThree/LabThree.csproj"
COPY . .
WORKDIR "/src/LabThree"
RUN dotnet build "LabThree.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LabThree.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LabThree.dll"]
