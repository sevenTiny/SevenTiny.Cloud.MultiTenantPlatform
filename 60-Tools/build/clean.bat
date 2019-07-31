dotnet clean ..\..\10-Code\SevenTiny.Cloud.Account\SevenTiny.Cloud.Account.csproj
dotnet clean ..\..\10-Code\Seventiny.Cloud.DevelopmentWeb\Seventiny.Cloud.DevelopmentWeb.csproj
dotnet clean ..\..\10-Code\Seventiny.Cloud.OfficialWeb\Seventiny.Cloud.OfficialWeb.csproj
dotnet clean ..\..\10-Code\Seventiny.Cloud.SettingWeb\Seventiny.Cloud.SettingWeb.csproj

RD /s/q  ..\..\10-Code\SevenTiny.Cloud.Account\bin\Debug\netcoreapp2.2\publish
RD /s/q  ..\..\10-Code\SevenTiny.Cloud.DevelopmentWeb\bin\Debug\netcoreapp2.2\publish
RD /s/q  ..\..\10-Code\SevenTiny.Cloud.OfficialWeb\bin\Debug\netcoreapp2.2\publish
RD /s/q  ..\..\10-Code\SevenTiny.Cloud.SettingWeb\bin\Debug\netcoreapp2.2\publish
