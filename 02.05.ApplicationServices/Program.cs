//  ���������� �������� � ������� middleware

//  ����� ���������� �������� � ��������� Services ������� WebApplicationBuilder ��� ���������� �������� ����������, � ��� ����� � �
//  ��������� ����������� middleware. � middleware �� ����� �������� ����������� ����� ���������:
//  ����� �����������
//  ����� �������� ������ Invoke/InvokeAsync
//  ����� �������� HttpContext.RequestServices

//  ��� ���� ���� ���������, ��� ���������� middleware ��������� ��� ������� ���������� � ����� � ������� ����� ���������� �����
//  ����������. �� ���� ��� ����������� �������� �������������� ASP.NET Core ���������� ����� ��������� ���������. � ��� ��������
//  ����������� �� ������������� ������������ � middleware.

//  � ���������, ���� ����������� ���������� transient-������, ������� ��������� ��� ������ ��������� � ����, �� ��� �����������
//  �������� �� ����� ������������ ��� �� ����� ������, ��� ��� ����������� middleware ���������� ���� ��� - ��� �������� ����������.

//  ��������, ��������� ������ TimeService:

//public class TimeService
//{
//    public TimeService()
//    {
//        Time = DateTime.Now.ToLongTimeString();
//    }
//    public string Time { get; }
//}
//  � ������������ ��������������� ��������, ������� ������ ������� ����� � ���� ������.

//  ������� ����� ��������� TimerMiddleware, ������� ����� ������������ ���� ������ ��� ������ ������� �� ���-��������:

//public class TimerMiddleware
//{
//    RequestDelegate next;
//    TimeService timeService;

//    public TimerMiddleware(RequestDelegate next, TimeService timeService)
//    {
//        this.next = next;
//        this.timeService = timeService;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Path == "/time")
//        {
//            context.Response.ContentType = "text/html; charset=utf-8";
//            await context.Response.WriteAsync($"������� �����: {timeService?.Time}");
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}

//  ���� ������ TimeService ����������� � ��������� �������� ����������, �� �� ������ �������� ��� ����� ����������� ������ TimerMiddleware.

//  ������ ���������� ������������, ���, ���� ������ ������ �� ������ "/time", �� � ������� TimeService ������������ ������� �����.
//  ����� �� ������ ���������� � ���������� middleware � ��������� ��������� �������.

var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<TimeService>();

var app = builder.Build();

app.UseMiddleware<TimerMiddleware>();
app.Run(async context => await context.Response.WriteAsync("Hey all"));

app.Run();

//  � �����, ���� �� ��������� �� ���� "/time", �� ���������� ������� ������� �����. ������ ������� �� �� ��� �� ���������� �� �����
//  ����, �� ��� ����� ����� �������� ���� � �� �� �����, ��� ��� ������ TimerMiddleware ��� ������ ��� ��� ������ �������. �������
//  �������� ����� ����������� middleware ������ �������� ��� �������� � ��������� ������ Singleton, ������� ��������� ���� ��� ���
//  ���� ����������� ��������.

//  ���� �� � middleware ���������� ������������ ������� � ��������� ������ Scoped ��� Transient, �� ����� �� ���������� �����
//  �������� ������ Invoke/InvokeAsync:

//public class TimerMiddleware
//{
//    RequestDelegate next;

//    public TimerMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }

//    public async Task InvokeAsync(HttpContext context, TimeService timeService)
//    {
//        if (context.Request.Path == "/time")
//        {
//            context.Response.ContentType = "text/html; charset=utf-8";
//            await context.Response.WriteAsync($"������� �����: {timeService?.Time}");
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}