//  Scoped-������� � singleton-��������

//  ��� �������, ������� ������������ � ASP.NET Core, ����� ��� �������� ���������� �����. Singleton-������� ��������� ���� ��� ���
//  ������� ����������, � ��� ���� �������� � ���������� ��� ���������� ���� � ��� �� singleton-������. � �������� singleton-��������
//  ���������, � �������, ���������� middleware ��� �������, ������� �������������� � ������� ������ AddSingleton().

//  Transient - ������� ��������� ������ ���, ����� ��� ��������� ��������� ������������� ������. � scoped-������� ��������� �� ������
//  �� ������ ������.

//  ���� ������� ��� ������� � ������� ����������� ��������� dependency injection ����� �������� � ������ �������. ��������
//  ���������������� ������ ��������� �������� ����������� �������� ����� �����������. ������ ������� � ������ ASP.NET Core 2.0 �� ��
//  ����� ���������� scoped-������� � ����������� singleton-��������.

//  ��������, ����� ����� ��������� ��������� ������:

//public interface ITimer
//{
//    string Time { get; }
//}
//public class Timer : ITimer
//{
//    public Timer()
//    {
//        Time = DateTime.Now.ToLongTimeString();
//    }
//    public string Time { get; }
//}
//public class TimeService
//{
//    private ITimer timer;
//    public TimeService(ITimer timer)
//    {
//        this.timer = timer;
//    }
//    public string GetTime() => timer.Time;
//}
//  TimeService �������� ����� ����������� ������ ITimer � ���������� ��� ��� ��������� �������� �������.

//  ����� ����� ����� ��������� ��������� middleware TimerMiddleware:

//public class TimerMiddleware
//{
//    TimeService timeService;
//    public TimerMiddleware(RequestDelegate next, TimeService timeService)
//    {
//        this.timeService = timeService;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//    }
//}

//  ��������� TimerMiddleware �������� ������ TimeService � ���������� � ����� ������� ���������� � ������� �������.

//  TimerMiddleware �������� singleton-��������. � ������ �������������� ������ TimeService ��� scoped-������:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimer, Timer>();
//builder.Services.AddScoped<TimeService>();

//var app = builder.Build();

//app.UseMiddleware<TimerMiddleware>();

//app.Run();

//  �� ���� �� ������ �������� ������� TimerMiddleware scoped-������ TimeService ��� �� ����������, �������������� �� ��������������
//  �� �����. � ��� �������� ������� TimeService ������ ������� ������ TimerMiddleware.

//  ����������� �������� ����� ����������, ���� TimeService ����������� ��� Transient, � ������ ITimer ��������� ��� Scoped.

//  ��� ������ �� ���� �������� �� TimeService, �� ITimer �� ������ ����� ��������� ���� Scoped. �� ���� ��� ����� ���� Transient
//  ��� Singleton.

//  ���������� ��� ���� ��������, � ������� ����� ����������� � ����� ����� ����������, � �� ������ � ������������ middleware, �����
//  ������ TimeService ������������ singleton, � ITimer - scoped-������:

var builder = WebApplication.CreateBuilder();

builder.Services.AddScoped<ITimer, Timer>();
builder.Services.AddSingleton<TimeService>();

var app = builder.Build();

app.UseMiddleware<TimerMiddleware>();

app.Run();

//  �, ��������, ��� ������� ������������ � TimerMiddleware ��������������� ��� ��������� ������� � ������ Invoke/InvokeAsync:

//public class TimerMiddleware
//{
//    public TimerMiddleware(RequestDelegate next) { }

//    public async Task Invoke(HttpContext context, TimeService timeService)
//    {
//        await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//    }
//}

//  ��� ������� ���������� �� ����� �� ���������� � �������, ������ ������� ������ "Cannot consume scoped service 'DIApp.ITimer'
//  from singleton 'DIApp.TimeService'"

//  �� ���� ����� �� �� ����� - �� �� ����� �� ��������� ���������� � ����������� singleton-������� scoped-������.