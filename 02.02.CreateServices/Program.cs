//  �������� ��������

//  ��������� ASP.NET Core ������������� ��� ���������� ��������, ������� �� ����� ������������. �� ����� �� ����� ��������� ����
//  ����������� �������. ����������, ��� ��� �������.

//  ��������� ����� ��������� ITimeService, ������� ������������ ��� ��������� �������:

//interface ITimeService
//{
//    string GetTime();
//}

//  � ����� ��������� ��� ������, ������� ����� ����������� ������ ���������. ������ ����� ����� ���������� ShortTimeService � �����
//  ���������� ������� ����� � ������� hh:mm(�� ���� ���� � ������):

//class ShortTimeService : ITimeService
//{
//    public string GetTime() => DateTime.Now.ToShortTimeString();
//}

//  ������ ����� ����� ���������� LongTimeService - �� ����� ���������� ����� � ������� hh:mm:ss:

//class LongTimeService : ITimeService
//{
//    public string GetTime() => DateTime.Now.ToLongTimeString();
//}

//  ������ ������� � ��������� �������� ������ ITimeService � ���������� ��� � ����������:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();

//var app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//});

//app.Run();

//  ����� ���� �������� ��� �������. ��-������, ���������� ������� � ��������� �������� ����������:
//      builder.Services.AddTransient<ITimeService, ShortTimeService>();

//  ��������� ������ AddTransient<ITimeService, ShortTimeService>() ������� �� ����� �������� ���������� ITimeService �����
//  ���������� ���������� ������ ShortTimeService.

//  ����� ����, ������� ����������� �� �������� ������� WebApplication ������� Build() ������� WebApplicationBuilder:
//      // ���������� ��������
//      builder.Services.AddTransient<ITimeService, ShortTimeService>();
//      // �������� ������� WebApplication
//      var app = builder.Build();

//  ����� ���������� ������� ��� ����� �������� � ������������ � ����� ����� ����������. ��� ��������� ������� ����� �����������
//  ��������� ������� � ����������� �� ��������. � ������ ������ ������������ �������� app.Services., ������� ������������� ���������
//  �������� - ������ IServiceProvider. ��� ��������� ������� � ���������� ������� ���������� ����� GetService(), ������� ������������
//  ����� �������:
//      var timeService = app.Services.GetService<ITimeService>();

//  ����� ��������� ������� �� ����� ������������ ���.
//      await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");

//  ��������� ����� AddTransient ��������� ����������� ����� ITimeService � ShortTimeService, �� � �������� ��������� ������� �����
//  � ������� "hh:mm". �� ����� �������� ���, �������������� � ITimeService:
//      builder.Services.AddTransient<ITimeService, LongTimeService>();
//  � � ���� ������ �� ������ ������ ���������.

#region ������ ��� ���������� �����
//  ��� ���� ������������� ��������� ����������� ������� � ���� ���������� � ��� ����������. ��� ������ "������" � ������ ������ �����
//  ������������ ����� ������, ���������������� �������� ����� �������������� � ����������.

//  ��������, ��������� ����� ����� TimeService:

//class TimeClassService
//{
//    public string GetTime() => DateTime.Now.ToShortTimeString();
//}

//  ������ ����� ���������� ���� ����� GetTime(), ������� ���������� ������� �����.

//  ���������� ���� ����� � �������� �������:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<TimeClassService>();

//var app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<TimeClassService>();
//    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//});

//app.Run();

//  ��� ���������� ������� � ��� ��������� ����������� ����� AddTransient():
//      builder.Services.AddTransient<TimeService>();

//  ����� ���������� ������� �� ��� ����� �������� � ������������ � ����� ����� ����������.
#endregion

#region ���������� ��� ���������� ��������
//  ������� ��� �������� ������� ����������� ������ ���������� � ���� ������� ���������� ��� ���������� IServiceCollection.
//  ��������, �������� �������� ����� ��� ������� TimeService:

//public static class ServiceProviderExtensions
//{
//    public static void AddTimeService(this IServiceCollection services)
//    {
//        services.AddTransient<TimeClassService>();
//    }
//}

//  � ������ ���������� ���� �����:

var builder = WebApplication.CreateBuilder();

builder.Services.AddTimeService();

var app = builder.Build();

app.Run(async context =>
{
    var timeService = app.Services.GetService<TimeClassService>();
    context.Response.ContentType = "text/html; charset = utf-8";
    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
});

app.Run();
#endregion

#region ����� ����
public static class ServiceProviderExtensions
{
    public static void AddTimeService(this IServiceCollection services)
    {
        services.AddTransient<TimeClassService>();
    }
}

class TimeClassService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}
interface ITimeService
{
    string GetTime();
}

// ����� � ������� hh::mm
class ShortTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}

// ����� � ������� hh:mm:ss
class LongTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToLongTimeString();
}
#endregion
