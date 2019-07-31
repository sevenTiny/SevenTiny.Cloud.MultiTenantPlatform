SET copyfile=.\bin\appsettings.json
SET netcoreVersion=2.2

SET dirs=..\..\10-Code\SevenTiny.Cloud.Account,..\..\10-Code\SevenTiny.Cloud.DevelopmentWeb,..\..\10-Code\SevenTiny.Cloud.OfficialWeb,..\..\10-Code\SevenTiny.Cloud.SettingWeb

for %%i in (%dirs%) do (
    COPY %copyfile% %%i\bin\Debug\netcoreapp%netcoreVersion%
    COPY %copyfile% %%i\bin\Debug\netcoreapp%netcoreVersion%\publish\
)

