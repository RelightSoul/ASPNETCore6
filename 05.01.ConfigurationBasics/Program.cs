//  ������������
//  ������ ������������

//  ������ ���� � ���������� ������ ������������, ������� ���������� ������� ��������� ����������. ���������� ASP.NET Core �����
//  �������� ���������������� ��������� �� ��������� ����������:
//      ��������� ��������� ������
//      ���������� ����� ���������
//      ������� .NET � ������
//      ����� (json, xml, ini)
//      Azure
//      ����� ������������ ���� ��������� ��������� � ��� ��� ��������� ���������� ������������

#region ��������� IConfiguration
//  ������������ ���������� � ASP.NET Core ������������ ������ ���������� IConfiguration:

//using Microsoft.Extensions.Primitives;

//public interface IConfiguration
//{
//    string this[string key] { get; set; }
//    IEnumerable<IConfigurationSection> GetChildren();
//    IChangeToken GetReloadToken();
//    IConfigurationSection GetSection(string key);
//}

//  ������ ��������� �������� ��������� ����������:
//      this[string key]: ����������, ����� ������� ����� �������� �� ����� �������� ��������� ������������. ����� ��������, ��� � ����,
//      � �������� ��������� ������������ ������������ ����� ������ ���� string
//      GetChildren(): ���������� ����� ��������� ������� ������ ������������ � ���� ������� IEnumerable<IConfigurationSection>
//      GetReloadToken(): ���������� ������ IChangeToken, ������� ����������� ��� ������������ ��������� ������������
//      GetSection(string key): ���������� ������ ������������, ������� ������������� ����� key

//  ����� ������������ ����� ���� ������������ ����������� IConfigurationRoot, ������� ����������� �� IConfiguration:

//public interface IConfigurationRoot : IConfiguration
//{
//    IEnumerable<IConfigurationProvider> Providers { get; }
//    void Reload();
//}
//      �������� Providers ���������� ��������� ����������� ����������� ������������. ������ ��������� ������������ ������������ ������
//      IConfigurationProvider
//      ����� Reload() ������������� �������� �� ���� ����������� ���������� ������������

//  ����, ������ IConfiguration �� ���� ������ ��� ���������������� ��������� � ���� ������ ��� "����"-"��������".
#endregion

#region ��������� ������ ������������
//  � ���������� ��������� ������������ �������� � �������� Configuration ������� WebApplication. �������������� ����� ��� ��������
//  �� ����� ���������� ��� �������� ��������� ������������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//// ��������� �������� ������������
//app.Configuration["name"] = "Tom";
//app.Configuration["age"] = "37";

//app.Run(async context =>
//{
//    string name = app.Configuration["name"];
//    string age = app.Configuration["age"];
//    await context.Response.WriteAsync($"Name: {name} \nAge: {age}");
//});

//app.Run();

//  ��� ��������� �������� �������� �� ������������� �����:
//  app.Configuration["name"] = "Tom";
//  � ������ ������ � ������������ ��������������� ������� � ������ "name". �� �������� � �������� �������� ������ "Tom".
//  ��� ���� �������, ��� ���������� � ������������ ��� ��������� � ������ "name". ���� �� ���, ��� �����������. ���� ���
//  ��� ����������, �� �������� �������������������.

//  � ����� �� ����� �������� ������ ��������� �� �����:
//  string name = app.Configuration["name"];

//  ����� �������� ��������, ��� � �������� �������� ���������� ������. ������� � ������ � ����������� � ������������
//  ��������� �������� �� ����� "age" � �������� �������� ����� ���������� ������:
//  app.Configuration["age"] = "37";
#endregion

#region ���������� ��������� ������������
//  � ������� ���� ��������� ������������ ��������������� �� ����������� - ������� ��������� "name", ����� ��������� "age". ������
//  ���� �������� ����� ��� ���� ��� ����� ������� ���������, ������� ����� ���������� �� ����� ������, �������� � ������, �����
//  ��������� �������� � ����� json, xml ��� ������� �� ������-�� ������� ��������� ������������. ��� ���������� ���������
//  ������������ � ���������� ����� ��������� �������� Configuration ������� WebApplicationBuilder. ��� �������� ������������
//  ����� ConfigurationManager, ��� �������� ��������� ��� ������� ��� ���������� ������������.

//var builder = WebApplication.CreateBuilder();

//builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
//{
//    { "name", "Tom" },
//    { "age", "37" }
//});

//var app = builder.Build();  

//app.Run(async context =>
//{
//    // ��������� �������� �����������
//    string name = app.Configuration["name"];
//    string age = app.Configuration["age"];
//    await context.Response.WriteAsync($"{name} - {age}");
//});

//app.Run();

//  ����� ��� ���������� ������������ ����������� ����� AddInMemoryCollection(). ���� ����� ��������� ����� �������� � ����
//  ��������� ��� ����-��������:
//  public static IConfigurationBuilder AddInMemoryCollection(this IConfigurationBuilder configurationBuilder,
//          IEnumerable<KeyValuePair<string, string>> initialData)

//  ��� ��� ����� ������� �������� ����������� ������� Dictionary<string, string>

//  ����� ���������� ��������� ������������ �� ����� ����� �������� ��������� ������������ ����� �������� app.Configuration.
#endregion

#region ��������� ������������ ����� Dependancy Injection
//  ������������ ���������� � ���� ������� IConfiguration ������������ ���� �� ��������, ������� ����������� � ���������� �� ���������.
//  �������������� ��� ������������ ���������� �� ����� �������� ��� � ����� ������ ������ ����� �������� ��������� ������������.
//  ��������:

WebApplicationBuilder builder = WebApplication.CreateBuilder();

WebApplication app = builder.Build();

//  ��������� �������� ������������

app.Configuration["name"] = "Tom";
app.Configuration["age"] = "33";

//  ����� �������� ��������� ������������ ������� ������ IConfiguration
app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

app.Run();

//  � ������ ������ ���������� ������� � ������ app.Map() � �������� ��������� appConfig �������� ������ IConfiguration - �� ����
//  ��� ��� �� ����� ������ IConfiguration, ��� � app.Configuration. �������� �������� �� ����� �������� ������������ � ������
//  ������ ����������, �������� ���, ��� ������ WebApplication ��� ����������.
#endregion