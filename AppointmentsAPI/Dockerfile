FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AppointmentsAPI/AppointmentsAPI.csproj", "AppointmentsAPI/"]
RUN dotnet restore "AppointmentsAPI/AppointmentsAPI.csproj"
COPY . .
WORKDIR "/src/AppointmentsAPI"
RUN dotnet build "AppointmentsAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppointmentsAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentsAPI.dll"]
