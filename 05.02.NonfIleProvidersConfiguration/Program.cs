//  ���������� ���������� ������������

#region �������� ���������� ��������� ������
//  ��� �������� ���������� ��������� ������ � ������������ ���������� ����������� ��������� CommandLineConfigurationProvider.
//  ��� ���������� ����� ���������� � ������� ConfigurationManager (�������� Configuration ������� WebApplicationBuilder) ����������
//  ����� AddCommandLine(), � ������� ���������� ��������� ��������� ������. �� ��������� ���������� �� ��������� ��������� ���������
//  ��������� ������ � ������ ������������, ������� ���������� � ������ ���������� ����� dependency injection, �� ��� ��� ������
//  ����� ������� ��� ��������.

//  ������ �������� ��������� ������, ������� �� ����� ������������ � �������� ��������� ������������, ������ ������������ ����
//  ����-��������. ���� ������ ������� ����������� ����� ����������. ����� ���������������� - ����� ����� ����� ����� ����� =
//  ����������� ��������, � ����� ������ ����������� ���������:
//      key1 = value1 key2 = value2 key3 = value3

//  ����� ����� ������������ ������ �������:
//      key1 = value1--key2 = value2 / key3 = value3
//      --key1 value1 /key2 value2
//      key1= key2=value2

//  ����� ������ ����� ������� ������� -- ��� /, ����� ����� ������ � ��������� ����� �������� ���� ����� = � �������� ������ ������.

//  ����� ���� �� ����� ������� ����, �� �� ����� ��������� ��������, �� ����� ����� ����� ����� �� ��������� ��������.
#endregion

#region ������������ � Visual Studio
//  ��� ������������ �������� ���������� ��������� ������ ������� � ������� � ����� Properties ���� launchSettings.json.

//  �� ��������� �� �������� �������� ��������� �������:

//{
//    "iisSettings": {
//        "windowsAuthentication": false,
//    "anonymousAuthentication": true,
//    "iisExpress": {
//            "applicationUrl": "http://localhost:51654",
//      "sslPort": 44377
//    }
//    },
//  "profiles": {
//        "_05._02.NonfIleProvidersConfiguration": {
//            "commandName": "Project",
//      "dotnetRunMessages": true,
//      "launchBrowser": true,
//      "applicationUrl": "https://localhost:7235;http://localhost:5235",
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

//  �������� � ���� ����� � �������� "profiles". ���� ������� �� ��������� �������� ��� �������, ������� ����� ����������� ��� �������.
//  ���� ������� ���������� �� ����� ������� (� ���� ������ HelloApp). ������ ������� ���������� "IIS". ������� �������, ������� ��
//  ���������� ��� ������� ������� � ������� � ���� ������� ������
//      "commandLineArgs": "name=Bob age=37",

//  �������� "commandLineArgs" ��������� ���������� ������ ��������� ������, ������� ����� ������������ ����������. � ������ ������
//  ��������������, ��� ����� ��������� ������ ����� ������������ �������� name �� ��������� "Bob" � �������� age �� ��������� "37".
//  ��������, � �������� ������ ��� ���������� ����������, ������� � ������� ������� �� ����� ������� - HelloApp. ������� �����
//  ��������� ����� ��������� ��������� �������:

//"profiles": {
//    "_05._02.NonfIleProvidersConfiguration": {
//        "commandName": "Project",
//      "dotnetRunMessages": true,
//      "launchBrowser": true,
//      "commandLineArgs": "name=Bob age=37",
//      "applicationUrl": "https://localhost:7235;http://localhost:5235",
//      "environmentVariables": {
//            "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//    },

//  ��������� � ���������� ���������� ��� ��� ��������� �������� ������������:

//var builder = WebApplication.CreateBuilder(args);   //������� ���������
//var app = builder.Build();

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

//app.Run();

//  ����� ����� �������� �������� ���������� ��������� ������ - args � ����� WebApplication.CreateBuilder(args). ��������� �����
//  ���������� ������������� ����� ���������� ��������� ��������� ������. � ����� ������� �� ������ ��������� �������� ����������
//  ������������
#endregion

#region ������ ����� �������
//  ��������� CLI, �� ����� ��������� ���������� �� ��������� ������ � �������� ��� ���������.

//  ����, ������ ���� ������������ ��������� � �������� ������������ �������. ������� ��������� ������ � �������� � ������� �
//  ������� ������� cd � ����� �������. � ���� ������ ��� �����
//  C: \Users\Vlad\source\repos\ASPNETCore6\05.02.NonfIleProvidersConfiguration

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

//app.Run();

//  ����� ������ ��������� �������:  // dotnet run name=Tom age=35
//  �� ������ https://localhost:7235 � ��� ������, ����� Tom - 35
#endregion

#region ����������� ��������� ���������� ��������� ������
//  ����� �� ����� �� ������ ���� ������������ �������� ���������� ��������� ������:

//string[] commandLineArgs = { "name=Alice", "age=27" };  // ��������������� ��������� ������
//var builder = WebApplication.CreateBuilder(commandLineArgs);   //������� ���������
//var app = builder.Build();

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

//app.Run();
#endregion

#region ���������� ������ AddCommandLine
//  ����� ����� ���� �� �������� ��������� ��������� ������ ����� ����� AddCommandLine():

//var builder = WebApplication.CreateBuilder();

//string[] commandLineArgs = { "name=Vlad", "age=32" };   // ��������������� ��������� ������
//builder.Configuration.AddCommandLine(commandLineArgs);  // �������� ��������� � �������� ������������

//var app = builder.Build();

//app.Map("/", (IConfiguration config) => $"{config["name"]} - {config["age"]}");

//app.Run();
#endregion

#region ���������� ����� ��������� ��� �������� ������������
//  ��� �������� ���������� ����� ��������� � �������� ���������� ������������ ����������� ���������
//  EnvironmentVariablesConfigurationProvider. ��� ��� ������������� � ������� ConfigurationManager ���������� �����
//  AddEnvironmentVariables(). ������ � ���������� ���� �� ��� �������� ����� ������������, ��� ��� ����� ASP.NET Core ���
//  ��������� ���������� ����� ��������� � ������ ������������ �� ���������.

//  ��������, ������� ���������� ��������� "JAVA_HOME", ������� ��������� �� ����� ��������� java sdk, ���� ��� ���������� ����������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", (IConfiguration config) => $" Java_home = {config["JAVA_HOME"] ?? "not set"}");

//app.Run();

//  ����� ���������� ������ AddEnvironmentVariables():
//          builder.Configuration.AddEnvironmentVariables();
#endregion

#region �������� ������������ � ������
//  ��������� MemoryConfigurationProvider ��������� ������������ � �������� ������������ ��������� IEnumerable<KeyValuePair<string,
//  string>>, ������� ������ ������ � ���� ���� ����-�������� (������ - ������ Dictionary). ��� ���������� ��������� ������������
//  ����������� ����� AddInMemoryCollection(), � ������� ���������� ������� ���������������� ��������:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    { "name", "Nelly"},
    { "age", "27"}
});

app.Map("/", (IConfiguration appConfig) => 
{
    var name = appConfig["name"];
    var age = appConfig["age"];
    return $"{name} - {age}";
});

app.Run();
#endregion