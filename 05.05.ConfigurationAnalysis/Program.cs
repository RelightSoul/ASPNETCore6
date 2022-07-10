//  ������ ������������

//  ��� ������ � ������������� ��������� IConfiguration ���������� ��������� ������:
//      GetSection(name): ���������� ������ IConfiguration, ������� ������������ ������ ������������ ������ name
//      GetChildren(): ���������� ��� ��������� �������� ������� ������������ � ���� ������ �������� IConfiguration
//      GetReloadToken(): ���������� ����� - ������ IChangeToken, ������� ������������ ��� ����������� ��� ��������� ������������
//      GetConnectionString(name): ������������ ������ GetSection("ConnectionStrings")[name] � ��������������� ��������������� ��� ������
//      �� �������� ����������� � ��������� ����� �����
//      [key]: ����������, ������� ��������� �������� �� ������������� ����� key ���������� ��������

//  ��������, ���� � ��� ���� ��������� ������������ � ����� config.json:

//{
//    "ConnectionStrings": {
//        "DefaultConnection": "Main database",
//    "UsersContext": "Users database"
//    }
//}

//  �� �� ����� � ������� ������ GetSection() �������� ��������� ������ � �� ��������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) =>
//{
//    IConfigurationSection connStrings = appConfig.GetSection("ConnectionStrings");
//    string defaultConnection = connStrings.GetSection("DefaultConnection").Value;
//    return defaultConnection;
//});

//app.Run();

//  ������ ��������� ������ ������������ ������ IConfigurationSection. ���� ������ �������� ������ ������, �� ����� ����� �������
//  � ��� ����� GetSection(). ���� �� ������ �������� ������ ��������, �� ��� �������� ����� �������� Value.

//  ����� �� ����� �� ������������ ���� ����� GetSection(), ������� ��� ������ ���� � ������ ������:
//      string defaultConnection = appConfig.GetSection("ConnectionStrings:DefaultConnection").Value;
//  ��������� ������ �� ������������ ���������� ����������.

//  ����� �� ����� �� �������� ������ ��������, ��������� �����������:
//      string defaultConnection = appConfig["ConnectionStrings:DefaultConnection"];

//  �� � ����� ����, ��� ������ ��������������� � ������� "ConnectionStrings" ��� �������� ����� GetConnectionString():
//      string con = appConfig.GetConnectionString("DefaultConnection");

//  ��������� ���� ������������� ������, �� ����� �������� ������ ����� ����� ������������. ��������, ����� � ������� ���������
//  ��������� ���������������� ���� project.json:

//{
//    "projectConfig": {
//        "dependencies": {
//            "Microsoft.Extensions.Configuration": "1.0.0",
//      "Microsoft.Extensions.Configuration.Json": "1.0.0",
//      "Microsoft.NETCore.App": {
//                "version": "1.0.1",
//        "type": "platform"
//      },
//      "Microsoft.AspNetCore.Diagnostics": "1.0.0",

//      "Microsoft.AspNetCore.Server.IISIntegration": "1.0.0",
//      "Microsoft.AspNetCore.Server.Kestrel": "1.0.1",
//      "Microsoft.Extensions.Logging.Console": "1.0.0"
//        },

//    "tools": {
//            "Microsoft.AspNetCore.Server.IISIntegration.Tools": "1.0.0-preview2-final"
//    },

//    "frameworks": {
//            "netcoreapp1.0": {
//                "imports": [
//                  "dotnet5.6",
//          "portable-net45+win8"
//              ]
//      }
//        },

//    "buildOptions": {
//            "emitEntryPoint": true,
//      "preserveCompilationContext": true
//    },

//    "runtimeOptions": {
//            "configProperties": {
//                "System.GC.Server": true
//            }
//        },

//    "publishOptions": {
//            "include": [
//              "wwwroot",
//        "web.config"
//      ]
//    },

//    "scripts": {
//            "postpublish": [ "dotnet publish-iis --publish-folder %publish:OutputPath% --framework %publish:FullTargetFramework%" ]
//    }
//    }
//}

//  �������������� � ������� ��� ��� ���������� � ��������:

using System.Text;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddJsonFile("project.json");

app.Map("/", (IConfiguration appConfig) => GetSectionContent(appConfig.GetSection("projectConfig")));

app.Run();

string GetSectionContent(IConfiguration configSection)
{
    System.Text.StringBuilder contentBuilder = new StringBuilder();
    foreach (var section in configSection.GetChildren())
    {
        contentBuilder.Append($"\"{section.Key}\":");
        if (section.Value == null)
        {
            string subSectionContent = GetSectionContent(section);
            contentBuilder.Append($"{{\n{subSectionContent}}},\n");
        }
        else
        {
            contentBuilder.Append($"\"{section.Value}\",\n");
        }
    }

    return contentBuilder.ToString();
}