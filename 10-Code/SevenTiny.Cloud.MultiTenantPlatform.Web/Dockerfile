FROM docker.io/microsoft/aspnetcore:latest
ARG source
WORKDIR /publish
EXPOSE 80
COPY /. /publish
ENTRYPOINT ["dotnet", "SevenTiny.Cloud.MultiTenantPlatform.Web.dll"]
