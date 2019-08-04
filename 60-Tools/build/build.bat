call .\clean.bat

dotnet build ..\..\10-Code\SevenTiny.Cloud.Account\SevenTiny.Cloud.Account.csproj
dotnet build ..\..\10-Code\Seventiny.Cloud.DevelopmentWeb\Seventiny.Cloud.DevelopmentWeb.csproj
dotnet build ..\..\10-Code\Seventiny.Cloud.OfficialWeb\Seventiny.Cloud.OfficialWeb.csproj
dotnet build ..\..\10-Code\Seventiny.Cloud.SettingWeb\Seventiny.Cloud.SettingWeb.csproj

call .\copy.bat