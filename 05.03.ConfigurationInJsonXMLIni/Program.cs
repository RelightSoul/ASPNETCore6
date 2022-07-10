//  ������������ � ������ JSON, XML � Ini

#region ������������ � JSON
//  ��� �������, ��� �������� ������������ � ���������� ASP.NET Core ������������ ����� json. ��� ������ � ������� json �����������
//  ��������� JsonConfigurationProvider, � ��� �������� ������������ �� json ����������� ����� ���������� AddJsonFile().

//  �� ��������� � ������� ��� ���� ���� ������������ json - appsettings.json, � ����� appsettings.Development.json, �������
//  ����������� �� ��������� � ���������� � ������� �� ����� ������������ ��� �������� ���������������� ��������.

//  ��������, ��� ����� appsettings.json:

//{
//    "Logging": {
//        "LogLevel": {
//            "Default": "Information",
//      "Microsoft.AspNetCore": "Warning"
//        }
//    },
//  "AllowedHosts": "*"
//}

//  ����� ������������ ��������� ������������ (������� "Logging") � ����������� ����� (������� "AllowedHosts"). ���� �������� �����
//  ����� ��������� ��������. ����������� ������� ����� ������ ������ ����������� ��������� ��� ������� ����� ������������.

//  ������ � ������ ������ ��� ������� ������� ����� ���� json. ����, ������� � ������ ����� ���� config.json
//  � ��������� � ��� ��������� ����������:

//{
//    "person": "Tom",
//    "company": "Microsoft"
//}
//  ����� �������� ��� �������� � ������� "person" � "company". ���������� ��� ��������� � ����������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

//app.Run();

//  ��� ��������� ������������ �� json-����� �������� ����� ���������� � ����� AddJsonFile().

//  ��� ����������� �������� � ����� json ��� ���� ���������, ��� ��� ������ ����� ���������� �����. �� ��� ���� �� �����
//  ������������ ��� ������������ ����� ��� ������ �����:

//builder.Configuration
//    .AddJsonFile("config.json")
//    .AddJsonFile("otherconfig.json");

//  � ���� �� ������ ����� ���� ���������, ������� ����� ��� �� ����, ��� � ��������� ������� �����, �� ���������� ��������������� ��������: ��������� �� ������� ����� �������� ��������� �������.

//  �� json ����� ������� ����� ����� ������� �� ������� �������, ��������:

//{
//    "person": { "profile": { "name" : "Tomas", "email" : "tom@gmail.com"} },
//    "company": { "name": "Mocrosoft"}
//}

//  � ����� ���������� � ���� ���������, ��� ���� ������������ ���� ��������� ��� ��������� � �������� ��������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) => 
//{
//    var personName = appConfig["person:profile:name"];
//    var companyName = appConfig["company:name"];
//    return $"{personName} - {companyName}";
//});

//app.Run();
#endregion

#region ������������ � XML
//  �� ������������� ������������ � XML-����� �������� ��������� XmlConfigurationProvider. ��� �������� xml-����� �����������
//  ����� ���������� AddXmlFile().

//  ����, ������� � ������ ����� xml-����, ������� ������� config.xml. ����� ������� ��� ��� �� ���������:

//<? xml version = "1.0" encoding = "utf-8" ?>
//< configuration >
//  < person > Tom </ person >
//  < company > Microsoft </ company >
//</ configuration >

//  ����� ���������� ��� �������� person � company, ������� ������������ ���������������� ���������.
//  �������� ��������, ��� � ����� xml � ��������� ������ ���� ���������� ����������� ��� ���������� � �������� ����� ����������

//  ������ ���������� ���� ����:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddXmlFile("config.xml");

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

//app.Run();

//���� � ��� ���� ������������ ����� ������ ������ �����������, ��������:

//<? xml version = "1.0" encoding = "utf-8" ?>
//< configuration >
//  < person >
//      < profile >
//          < name > Tomas </ name >
//          < email > toma@gmail.com </ email >
//      </ profile >
//  </ person >
//  < company >
//      < name > Microsoft </ name >
//  </ company >
//</ configuration >

//  �� �� ����� ���������� � ���� ������� �����, ��� � � ����� json:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddXmlFile("config.xml");

//app.Map("/", (IConfiguration appConfig) => 
//{
//    var name = appConfig["person:profile:name"];
//    var company = appConfig["company:name"];
//    return $"{name} - {company}";
//});

//app.Run();
#endregion

#region ������������ � ini-������
//  ��� ������ � ������������� INI ����������� ��������� IniConfigurationProvider. � ��� �������� ������������ �� INI-����� ���
//  ���� ������������ ����� ���������� AddIniFile().

//  ��������, ������� � ������ ��������� ���� � ����������� ��� � config.ini.
//  ��������� � ���� ����� ��������� ����������:
//      person = "Bob"
//      company = "Microsoft"

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddIniFile("config.ini");

app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

app.Run();
#endregion