FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroBlog.WebApi/MicroBlog.WebApi.csproj", "MicroBlog.WebApi/"]
COPY ["MicroBlog.WebApi.Services/MicroBlog.WebApi.Services.csproj", "MicroBlog.WebApi.Services/"]
COPY ["MicroBlog.Common/MicroBlog.Common.csproj", "MicroBlog.Common/"]
COPY ["MicroBlog.WebApi.Core/MicroBlog.WebApi.Core.csproj", "MicroBlog.WebApi.Core/"]
COPY ["MicroBlog.WebApi.Models/MicroBlog.WebApi.Models.csproj", "MicroBlog.WebApi.Models/"]
RUN dotnet restore "MicroBlog.WebApi/MicroBlog.WebApi.csproj"
COPY . .
WORKDIR "/src/MicroBlog.WebApi"
RUN dotnet build "MicroBlog.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroBlog.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroBlog.WebApi.dll"]