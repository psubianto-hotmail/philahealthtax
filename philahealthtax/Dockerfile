FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["philahealthtax/philahealthtax.csproj", "philahealthtax/"]
RUN dotnet restore "philahealthtax/philahealthtax.csproj"
COPY . .
WORKDIR "/src/philahealthtax"
RUN dotnet build "philahealthtax.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "philahealthtax.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "philahealthtax.dll"]