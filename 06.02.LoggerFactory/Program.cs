//  ������� ������� � ���������� ������������

//  � �������� � ������� ���� �� �������� ������ �������, ������� ����������� ����� DI. �� �� ����� ����� ������������ �������
//  ������� ��� ��� ��������:

//using Microsoft.Extensions.Logging;

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
//ILogger logger = loggerFactory.CreateLogger<Program>();

//app.Run(async context =>
//{
//    logger.LogInformation($"Reuqest path: {context.Request.Path}");
//    await context.Response.WriteAsync("Hell prog world");
//});

//app.Run();

//  � ������ ������ � ������� ������ LoggerFactory.Create ��������� ������� ������� � ���� ������� ILoggerFactory. � ��������
//  ��������� ����� ��������� �������, ������� ������������� ��������� ��������� ������������. � ���������, ����� AddConsole()
//  ������� ILoggingBuilder ������������� ����� ��������� ���� �� �������. ����� ����� CreateLogger() ������� ���������� ������� ������:
//  ILogger logger = loggerFactory.CreateLogger<Program>();

//  ����� CreateLogger() ������������ �������, ������� ������������ ���������. � ������ ������ ��� ����� Program, � ������� ������
//  ����������� ������ ���. �� � �������� ������������ �������� ��������� ����� �������� � ����� � �������� ��������� � ���� ������:
//  ILogger logger = loggerFactory.CreateLogger("WebApplication");

//  � ����� �� ������� ��� �� ����� ��������� �� �������. �� ������������ ������������� ������� �������� ������� � ���, ��� �� �����
//  ������������� ��������� ��������� ��������� ������������, � ���������, ��������� ������������

#region ��������� ������� ������� ����� dependency injection
//  ��� � ������, ������� ������� �������� � ���������� � ���� �������, �������������� �� ����� ������� ����� �������� ���������
//  ������������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/hello", (ILoggerFactory loggerFactory) => 
//{
//    ILogger logger = loggerFactory.CreateLogger("MapLogger");
//    logger.LogInformation($"Path: /hello, time: {DateTime.Now.ToLongTimeString()}");
//    return "HeLL()";
//});

//app.Run();
#endregion

#region ���������� ������������
//  � ������� ���� ������������ ��� �� �������. ������ ���� ������������ ������������ ����������� ������������. �� ���������
//  ASP.NET Core ������������� ��������� ����������:

//  Console: ����� ���������� �� �������.��������������� ������� AddConsole()

//  Debug: ���������� ��� ������� ������� ���� ����� System.Diagnostics.Debug � � ��������� ��� ����� Debug.WriteLine.��������������
//  ��� ������ ���� �� ����� ������� � ���� Output � Visual Studio.��������������� ������� AddDebug(). ����� ��������, ��� ������
//  ������ �������� ������ ��� ������� ������� � ������ �������

//  EventSource: �� Windows ������ ������������ � ��� ETW (Event Tracing for Windows), ��� ��������� �������� ����� ��������������
//  ���������� PerfView (��� ����������� �����������). ���� ������ ��������� ����������� ��� ������������������, ��� Linux � MacOS
//  ���� ���������� ���� �� ����������. ��������������� ������� AddEventSourceLogger()

//  EventLog: ���������� � Windows Event Log, �������������� �������� ������ ��� ������� �� Windows. ��������������� ������� AddEventLog()

//  ��������, ������ ������� ������� ����� ���� � ���� Output � Visual Studio:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
ILogger logger = loggerFactory.CreateLogger<Program>();
app.Run(async (context) =>
{
    logger.LogInformation($"Requested Path: {context.Request.Path}");
    await context.Response.WriteAsync("Hello World!");
});

app.Run();
#endregion