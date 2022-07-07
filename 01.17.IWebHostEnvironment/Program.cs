//  IWebHostEnvironment � ���������

//  ��� �������������� � ����������, � ������� �������� ����������, ��������� ASP.NET Core ������������� ��������� IWebHostEnvironment.
//  ���� ��������� ���������� ��� �������, � ������� ������� �� ����� �������� ���������� �� ���������:

//  ApplicationName: ������ ��� ����������

//  EnvironmentName: ������ �������� �����, � ������� ����������� ����������

//  ContentRootPath: ������ ���� � �������� ����� ����������

//  WebRootPath: ������ ���� � �����, � ������� �������� ����������� ������� ����������. �� ��������� ��� ����� wwwroot

//  ContentRootFileProvider: ���������� ���������� ���������� Microsoft.AspNetCore.FileProviders.IFileProvider, ������� �����
//  �������������� ��� ������ ������ �� ����� ContentRootPath

//  WebRootFileProvider: ���������� ���������� ���������� Microsoft.AspNetCore.FileProviders.IFileProvider, ������� �����
//  �������������� ��� ������ ������ �� ����� WebRootPath

//  ��� ���������� �� ����� ������������ ��� ��������. �� �������� ����� ��� ���������� �������� ������������ �� ���������
//  EnvironmentName. �� ��������� ������� ��� �������� �������� ��� ����� ��������: Development, Staging � Production.
//  � ������� ��� �������� �������� ����� ��������� ���������� ����� ASPNETCORE_ENVIRONMENT. ������� �������� �������
//  ��������� �������� � ����� launchSettings.json, ������� ������������� � ������� � ����� Properties.

//{
//    "iisSettings": {
//        "windowsAuthentication": false,
//    "anonymousAuthentication": true,
//    "iisExpress": {
//            "applicationUrl": "http://localhost:62332",
//      "sslPort": 44394
//    }
//    },
//  "profiles": {
//        "_01._17.IWebHostEnvironment": {
//            "commandName": "Project",
//      "dotnetRunMessages": true,
//      "launchBrowser": true,
//      "applicationUrl": "https://localhost:7285;http://localhost:5285",
//      "environmentVariables": {
//                "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//        },
//    "IIS Express": {
//            "commandName": "IISExpress",
//      "launchBrowser": true,
//      "environmentVariables": {
//                "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//        }
//    }
//}

//  ����� ����� �������, ��� ���������� "ASPNETCORE_ENVIRONMENT" ����������� ��� ���� - ��� ������� ����� IISExpress � ��� �������
//  ����� Kestrel. � ����� ������� ��� ����� �������� Development. �� �� ����� �������� �������� ���� ����������.

//  ��� ����������� �������� ���� ���������� ��� ���������� IWebHostEnvironment ���������� ����������� ������ ����������:
//  IsEnvironment(string envName): ���������� true, ���� ��� ����� ����� �������� ��������� envName
//  IsDevelopment(): ���������� true, ���� ��� ����� - Development
//  IsStaging(): ���������� true, ���� ��� ����� - Staging
//  IsProduction(): ���������� true, ���� ��� ����� - Production

//  ������ ���������������� ��������� ��� ��������� ������������ ��� � ����������� �� ����, �� ����� ������ ��������� ����������.
//  ���� ���������� � �������� ����������, �� �� ����� ��������� ���� ���, � ��� ������������� ��� ������������ ������������� ������ ���:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.Run(async context => await context.Response.WriteAsync("In Development Stage"));
//}
//else
//{
//    app.Run(async context => await context.Response.WriteAsync("In Production Stage"));
//}
//app.Run();

//  ���� �� ����� �������� �������� �����, ������������� �������� ���� launchSettings.json. ��� ����� ������� ����� ����������:

//  app.Environment.EnvironmentName = "Production";

#region ����������� ����� ��������� �����
//  ���� �� ��������� ����� ����� ��������� ��� ���������: Development, Staging, Production, �� �� ����� ��� ������� �������
//  ����� ��������. ��������, ��� ���� ����������� �����-�� �������������� ���������. ��� ����� ������� ����� ��������� �����
//  launchSettings.json ���� ����������.

//  ��������, ������� �������� ����� �� "Test" (�������� ����� ���� ������������):

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Environment.EnvironmentName = "Test";   // �������� �������� ����� �� Test

if (app.Environment.IsEnvironment("Test"))  // ���� ������ � ��������� "Test"
{
    app.Run(async context => await context.Response.WriteAsync("In Test Stage"));
}
else
{
    app.Run(async context => await context.Response.WriteAsync("In Development or Production Stage"));
}
app.Run();

//  ����� ����� �������� �������� "ASPNETCORE_ENVIRONMENT" �� "Test" ��� ����� ������ � ����� launchSettings.json ���
//  ������������� �������:

//{
//    "iisSettings": {
//        "windowsAuthentication": false,
//    "anonymousAuthentication": true,
//    "iisExpress": {
//            "applicationUrl": "http://localhost:62332",
//      "sslPort": 44394
//    }
//    },
//  "profiles": {
//        "_01._17.IWebHostEnvironment": {
//            "commandName": "Project",
//      "dotnetRunMessages": true,
//      "launchBrowser": true,
//      "applicationUrl": "https://localhost:7285;http://localhost:5285",
//      "environmentVariables": {
//                "ASPNETCORE_ENVIRONMENT": "Test"
//      }
//        },
//    "IIS Express": {
//            "commandName": "IISExpress",
//      "launchBrowser": true,
//      "environmentVariables": {
//                "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//        }
//    }
//}
#endregion