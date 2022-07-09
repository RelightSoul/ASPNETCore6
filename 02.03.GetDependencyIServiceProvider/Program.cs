//  ��������� ������������

//  � ASP.NET Core �� ����� �������� ����������� � ���������� ������� ���������� ���������;

//  ����� �������� Services ������� WebApplication (service locator)
//  ����� �������� RequestServices ��������� ������� HttpContext � ����������� middleware (service locator)
//  ����� ����������� ������    
//  ����� �������� ������ Invoke ���������� middleware
//  ����� �������� Services ������� WebApplicationBuilder

//  ��� ������ ��������� ��������� ITimeService � ����� ShortTimeService, ������� ��������� ������ ���������:

//interface ITimeService
//{
//    string GetTime();
//}
//class ShortTimeService : ITimeService
//{
//    public string GetTime() => DateTime.Now.ToShortTimeString();
//}

#region �������� Services ������� WebApplication
//  ���, ��� ��� �������� ������ WebApplication, ������� ������������ ������� ����������, (��������, � ����� Program.cs), ��� ���������
//  �������� �� ����� ������������ ��� �������� Services. ��� �������� ������������� ������ IServiceProvider, ������� ������������� ���
//  ������� ��� ��������� ��������:
//      GetService<service>(): ���������� ��������� �������� ��� �������� �������, ������� ������������ ��� service. � ������ ���� �
//      ���������� �������� ��� ������� ������� �� ����������� �����������, �� ���������� �������� null
//      GetRequiredService<service>(): ���������� ��������� �������� ��� �������� �������, ������� ������������ ��� service. � ������
//      ���� � ���������� �������� ��� ������� ������� �� ����������� �����������, �� ���������� ����������

//  ������ ������� ��������� ������� ��� ���������� service locator, �, ��� �������, �� ������������� � �������������, �� ��� �� �����
//  � ������ ASP.NET Core � �������� �� ����� ������������ �������� ���������������� �������� ���, ��� ������ ������� ���������
//  ������������ �� ��������.

//  ��������, ��������� ��������� ��� ����������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();

//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time {timeService?.GetTime()}");
//});

//app.Run();

//  � ������ ������ � ������� ������ ����
//      var timeService = app.Services.GetService<ITimeService>();
//  �������� �� ��������� �������� ������ ������� ITimeService - � ������ ������ �� ����� ������������ ������ ShortTimeService

//  �������� ��������, ����� ������ �� ����� �������� � ��������� ��������, ������ � �����-�� ����� ���������� �� ����� ����������
//  ��� ��������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();

//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time {timeService?.GetTime()}");
//});

//app.Run();
//  � ���� ������ ���������� timeService ����� ����� �������� null.

//  ����������� ������� ����� ������������ ����� GetRequiredService() �� ��� �����������, ��� ���� ������ �� ��������, �� �����
//  ���������� ����������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();

//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetRequiredService<ITimeService>();
//    await context.Response.WriteAsync($"Time {timeService.GetTime()}");
//});

//app.Run();
#endregion

#region HttpContext.RequestServices
//  ���, ��� ��� �������� ������ HttpContext, �� ����� ������������ ��� ��������� �������� ��� �������� RequestServices. ��� ��������
//  ������������� ������ IServiceProvider. �� ���� �� ���� �� ����� ���� � ���� ��������� �������� ��������� �������� � �������
//  ������� GetService() � GetRequiredService():

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();

//var app = builder.Build();

//app.Run(async(HttpContext context)=>
//{
//    var timeService = context.RequestServices.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//});

//app.Run();
#endregion

#region ������������
//  ���������� � ASP.NET Core ������� ��������� ������������ ���������� ������������ ������� ��� �������� ���� ������������. ��������
//  �������� ����� ������������ �������� ���������������� �������� ��������� ������������.

//  ��������, ����� � ������� ��������� ��������� ����� TimeMessage:

//class TimeMessage
//{
//    ITimeService timeService;
//    public TimeMessage(ITimeService timeService)
//    {
//        this.timeService = timeService;
//    }
//    public string GetTime() => $"Time: {timeService.GetTime()}";
//}
//  ����� ����� ����������� ������ ���������� ����������� �� ITimeService. ������ ����� ����������, ��� ��� ����� �� ����������
//  ���������� ITimeService. � ������ GetTime() ��������� ���������, � ������� �� ������� �������� ������� �����.

//  ��� ������������� ������ TimeMessage ��������� ��������� ����������:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();
//builder.Services.AddTransient<TimeMessage>();

//var app = builder.Build();

//app.Run(async context =>
//{
//    var timeServise = context.RequestServices.GetService<TimeMessage>();
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.WriteAsync($"<h2>{timeServise?.GetTime()}</h2>");
//});

//app.Run();

//  ��� ������������� � ���������� � �������� ������� ����� MessageService ����� ����������� � ��������� ��������. ��������� ���
//  ��������������� �����������, ������� ������������ ���������� �����, �� ����� builder.Services.AddTransient ������������ �����
//  ���� ����� TimeMessage. �� ���� ������, ������� ���������� �������, ���� ����� ��������� � �������� ��������.

//  �� ��� ��� ����� TimeMessage ���������� ����������� ITimeService, ������� ���������� ����� �����������, �� ��� ���� �����
//  ���������� � ��� �����������:
//      builder.Services.AddTransient<ITimeService, ShortTimeService>();
//  � ����� ��� ��������� ������� ����� �������������� ����� TimeMessage, ��� �������� ������� ����� ������ ����� ���������� ���������
//  ��������. ��������� �������� �������� ����������� ������ TimeMessage �� ������� ������������. ����� ������� ������� ��� ����
//  ������������ ������������ � �������� �� � �����������.
#endregion

#region ����� Invoke/InvokeAsync ����������� middleware
//  ������� ����, ��� ����������� ���������� � ����������� �������, ����� ����� �� ����� ���������� � ����� Invoke/InvokeAsync()
//  ���������� middleware. ��������,��������� ��������� ���������:

//class TimeMessageMiddleware
//{
//    private readonly RequestDelegate next;
//    public TimeMessageMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext context, ITimeService timeService)
//    {
//        context.Response.ContentType = "text/html; charset=utf-8";
//        await context.Response.WriteAsync($"<h1>Time: {timeService?.GetTime()}</h1>");
//    }
//}

//  �������� ��������� ��� ��������� �������:
var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<ITimeService, ShortTimeService>();

var app = builder.Build();

app.UseMiddleware<TimeMessageMiddleware>();

app.Run();

//  ����� ��������, ��� �� ����� ����� �� �������� ����������� � ����� ����������� ������ middleware:

//class TimeMessageMiddleware
//{
//    private readonly RequestDelegate next;
//    ITimeService timeService;
//    public TimeMessageMiddleware(RequestDelegate next, ITimeService timeService)
//    {
//        this.next = next;
//        this.timeService = timeService;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        context.Response.ContentType = "text/html; charset=utf-8";
//        await context.Response.WriteAsync($"<h1>Time: {timeService?.GetTime()}</h1>");
//    }
//}
#endregion

#region ����� ����
class TimeMessageMiddleware
{
    private readonly RequestDelegate next;
    public TimeMessageMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context, ITimeService timeService)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync($"<h1>Time: {timeService?.GetTime()}</h1>");
    }
}
class TimeMessage
{
    ITimeService timeService;
    public TimeMessage(ITimeService timeService)
    {
        this.timeService = timeService;
    }
    public string GetTime() => $"Time: {timeService.GetTime()}";
}
interface ITimeService
{
    string GetTime();
}
class ShortTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}
#endregion