FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
ARG source
WORKDIR /publish
EXPOSE 80
COPY /. /publish
ENTRYPOINT ["dotnet", "SevenTiny.Cloud.DataApi.dll"]
