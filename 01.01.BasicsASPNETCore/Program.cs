//  � ������ ���������� ASP.NET ��������� ����� WebApplication. ��������, ���� �� ������� ������ ASP.NET �� ����
//  ASP.NET Core Empty, �� � ����� Program.cs �� �������� ��������� ���:
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
//  ���������� app � ������ ���� ��� ��� ������������ ������ WebApplication. ������ ��� �������� ����� �������
//  ��������� ������ ������ - WebApplicationBuilder, ������� � ������ ���� ����������� ���������� builder.

#region ����� WebApplicationBuilder
//  �������� ���������� �� ��������� ���������� ���������� � ������ WebApplicationBuilder. �������� ��� �������� ��
//  ������ WebApplicationBuilder.cs

//  ��� ��� �������� ������� ����� ������ ���������� ����������� ����� WebApplication.CreateBuilder():
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();

//  ��� ������������� ������� WebApplicationBuilder � ���� ����� ����� ������������ ��������� ��������� ������,
//  ��������� ��� ������� ���������� (�������� ����� ������ ������������ �������� args):
//  WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//  ���� ����� ���������� ������ WebApplicationOption:
//  WebApplicationOptions options = new() { Args = args };
//  WebApplicationBuilder builder = WebApplication.CreateBuilder(options);

//  ����� �������� ������� WebApplication ����� WebApplicationBuilder ��������� ��� ��� �����, ����� �������
//  ����� �������� ���������:
//      ��������� ������������ ����������
//      ���������� ��������
//      ��������� ������������ � ����������
//      ��������� ��������� ����������
//      ������������ �������� IHostBuilder � IWebHostBuilder, ������� ����������� ��� �������� ����� ����������

//  ��� ���������� ���� ����� � ������ WebApplicationBuilder ���������� ��������� ��������:
//      Configuration: ������������ ������ ConfigurationManager, ������� ����������� ��� ���������� ������������ � ����������.
//      Environment: ������������� ���������� �� ���������, � ������� �������� ����������.
//      Host: ������ IHostBuilder, ������� ����������� ��� ��������� �����.
//      Logging: ��������� ���������� ��������� ������������ � ����������.
//      Services: ������������ ��������� �������� � ��������� ��������� ������� � ����������.
//      WebHost: ������ IWebHostBuilder, ������� ��������� ��������� ��������� ��������� �������.
#endregion

#region ����� WebApplication
//  ����� build() ������ WebApplicationBuilder ������� ������ WebApplication:
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();
//  WebApplication app = builder.Build();

//  ����� WebApplication ����������� ��� ���������� ���������� �������, ��������� ���������, ��������� �������� � �.�.
//  �������� ��� ������ ����� ����� �� Github �� ������ WebApplication.cs.

//  ����� WebApplication ��������� ��� ����������:
//      IHost: ����������� ��� ������� � ��������� �����, ������� ������������ �������� �������
//      IApplicationBuilder: ����������� ��� ��������� �����������, ������� ��������� � ��������� �������
//      IEndpointRouteBuilder: ����������� ��� ��������� ���������, ������� �������������� � ���������

//  ��� ��������� ������� � ���������������� ���������� ����� ������������ �������� ������ WebApplication:
//      Configuration: ������������ ������������ ���������� � ���� ������� IConfiguration
//      Environment: ������������ ��������� ���������� � ���� IWebHostEnvironment
//      Lifetime: ��������� �������� ����������� � �������� ���������� ����� ����������
//      Logger: ������������ ������ ���������� �� ���������
//      Services: ������������ ������� ����������
//      Urls: ������������ ����� �������, ������� ���������� ������

//  ��� ���������� ������ ����� WebApplication ���������� ��������� ������:
//      Run(): ��������� ����������//
//      RunAsync(): ���������� ��������� ����������
//      Start(): ��������� ����������
//      StartAsync(): ��������� ����������
//      StopAsync(): ������������� ����������

//  ����� �������, ����� ������ ����� Run/Start/RunAsync/StartAsync ���������� ����� �������, � �� ������ � ���� ����������:
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();
//  WebApplication app = builder.Build();
//  app.Run();

//  ��� ������������� � ������� ������ StopAsync() ����� ����������� �������� ��������� ���������� ����������:
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();

//  WebApplication app = builder.Build();

//  app.MapGet("/", () => "Hello World!");

//  await app.StartAsync();
//  await Task.Delay(10000);
//  await app.StopAsync();  // ����� 10 ������ ��������� ���������� ����������
#endregion
