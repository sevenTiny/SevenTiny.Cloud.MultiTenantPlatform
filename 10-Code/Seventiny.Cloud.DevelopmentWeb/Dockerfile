FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
ARG source
WORKDIR /publish
EXPOSE 80
COPY /. /publish
ENTRYPOINT ["dotnet", "Seventiny.Cloud.DevelopmentWeb.dll"]
