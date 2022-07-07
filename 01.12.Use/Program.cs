//  ����� Use

//  ����� ���������� Use() ��������� ��������� middleware, ������� ��������� �������� ��������� ������� ����� ��������� � ���������
//  �����������. �� ����� ��������� ������

//  public static IApplicationBuilder Use(this IApplicationBuilder app, Func<HttpContext, Func<Task>, Task> middleware);
//  public static IApplicationBuilder Use(this IApplicationBuilder app, Func<HttpContext, RequestDelegate, Task> middleware)

//  ����� Use() ���������� ��� ����� ���������� ��� ���� IApplicationBuilder, �������������� �� ����� ������� ������ ����� � �������
//  WebApplication ��� ���������� middleware � ����������. � ����� ������� ����� Use ��������� ��������� ��������, ������� ����� ���
//  ��������� � ���������� ������ Task.

//  ������ �������� �������� Func, ������� ���������� � ����� Use(), ������������ ������ HttpContext. ���� ������ ��������� ��������
//  ������ ������� � ��������� ������� �������.

//  ������ �������� �������� ������������ ������ ������� - Func<Task> ��� RequestDelegate. ���� ������� ������������ ��������� �
//  ��������� ��������� middleware, �������� ����� ������������ ��������� �������.

//  � ����� ������ ���������� ������ Use() �������� ��������� �������

//app.Use(async (context, next) =>
//{
//    // �������� ����� �������� ������� � ��������� middleware
//    await next.Invoke();
//    // �������� ����� ��������� ������� ��������� middleware
//});

//  ������ middleware ����������� �� ��� �����:
//  Middleware ��������� ��������� ��������� ��������� ������� �� ������ await next.Invoke()
//  ����� ���������� ����� next.Invoke(), ������� �������� ��������� ������� ���������� ���������� � ���������
//  ����� ��������� � ��������� ��������� �������� ��������� ������ ������������ � ������� � ������� ���������, � ����������� ��������,
//  ������� ���� ����� ������ await next.Invoke(

//  ����� �������, middleware � ������ Use ��������� �������� �� ���������� � ��������� ���������� � ����� ����.

//  ���������� ����� Use() �� �������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//string date = "";

//app.Use(async (context, next) =>
//{
//    date = DateTime.Now.ToShortDateString();
//    await next.Invoke();   // �������� middleware �� app.Run
//    Console.WriteLine($"Current date: {date}");
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"Date: {date}");
//});

//app.Run();

//  � ������ ������ �� ���������� ���������� ������ Use, ������� � �������� ���������� ��������� �������� ������� - ������ HttpContext
//  � ������� Func<Task>, ������� ������������ ����� ������ �� ��������� � ��������� ��������� middleware.

//  Middleware � ������ app.Use() ��������� ���������� ������ - ����������� ���������� date ������� ���� � ���� ������ � ����� ��������
//  ��������� ������� ��������� ����������� middleware � ���������. �� ���� ��� ������ await next.Invoke() ��������� ������� ��������
//  � ���� ����������, ������� ���������� � ������ app.Run(). � ����� ��������� ������� ����� ��������� ��������� �������:

//      ����� ���������� app.Use

//      ��������� �������� ���������� date:
//      date = DateTime.Now.ToShortDateString();

//      ����� await next.Invoke(). ���������� ��������� ���������� ���������� � ��������� - � app.Run.

//      � middleware �� app.Run() ��������� ������� ������� ���� � �������� ������ � ������� ������ context.Response.WriteAsync():
//      await context.Response.WriteAsync($"Date: {date}");

//      ����� app.Run �������� ���� ������, � ���������� ���������� ������������ � middleware � ������ app.Use.�������� ����������� ��
//      ����� ����, ������� ���� ����� await next.Invoke(). � ���� ����� ����������� �������� ������������ - �� ������� ��������� ��������
//      ���������� date:
//      Console.WriteLine($"Current date: {date}");

//      ����� ����� ��������� ������� ���������

#region �������� ������
//  ��� ������������� ������ Use � �������� ���������� ���������� �������� ������� ���������, ��� �� ������������� �������� �����
//  next.Invoke ����� ������ Response.WriteAsync(). ��������� middleware ������ ���� ������������ ����� � ������� Response.WriteAsync,
//  ���� �������� ��������� ������� ����������� next.Invoke, �� �� ��������� ��� ���� �������� ������������. ��� ��� ��������
//  ������������ ����������� ��������� ������� Response ����� �������� � ��������� ���������, ��������, ����� ������� ������ ����,
//  ��� ������� � ��������� Content-Length, ���� ����� �������� � ��������� ���� ������, ��������, ����� �������� HTML ��������� �
//  CSS-����.

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("<p>Hello world!</p>");
//    await next.Invoke();
//});

//app.Run(async (context) =>
//{
//    //await Task.Delay(10000); // ����� ��������� ��������
//    await context.Response.WriteAsync("<p>Good bye, World...</p>");
//});

//app.Run();
#endregion

#region ������������� ������� RequestDelegate
//  � ������� ���� �������������� ������ ������ Use(), ������� ���������� ������� Func<Task>. �������� ������� ����� ������������
//  � ������ ������, ��� ������������ ������� RequestDelegate. ������������ - ��� ������ �������� ( �� ���� ���������� ����������
//  � ��������� ����������) ���������� ���������� �������� ������ HttpContext:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//string date = "";

//app.Use(async (context, next) =>
//{
//    date = DateTime.Now.ToShortDateString();
//    await next.Invoke(context);                 // ����� next - RequestDelegate
//    Console.WriteLine($"Current date: {date}");  // Current date: 08.12.2021
//});

//app.Run(async (context) => await context.Response.WriteAsync($"Date: {date}"));

//app.Run();
#endregion

#region ������������ ��������� middleware
//  Middleware � ������ Use() ������������� ������ ������� � ���������� � ��������� ����������. ������ ����� �� ����� ���������
//  ��������� �������. � ���� ������ �� ����� ��������� � ���� ������ �� ������������� ���������� middleware, ��� � ���������� ��
//  ������ Run(). ��������:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

string date = "";

app.Use(async (context, next) =>
{
    string? path = context.Request.Path.Value?.ToLower();
    if (path == "/date")
    {
        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
    }
    else
    {
        await next.Invoke();
    }
});

app.Run(async (context) => await context.Response.WriteAsync($"Hello METANIT.COM"));

app.Run();
//  ����� middleware � app.Use ��������� ����������� ����� - ���� �� �������� "/date", �� ������� ������������ ������� ����.
//  ����� ��������� ������� ���������� ������ � app.Run.

//  ������ � �������� �� ����� ������������ ��������� � app.Use ��� ������������ � �������������� ������������:

//app.Use(async (HttpContext context, Func<Task> next) =>
//{
//    await context.Response.WriteAsync("Hello Work!");
//});

//  ������ � ������ ������ ��� ������� ����������������� ����� ������������ app.Run(), ���� ��� ���� ���������� ���� ���� ���������,
//  ������� � �������� �� �������� ������ ������ �� ���������.
#endregion

#region ��������� ����������� � ������
//  ����� ����� ������� ��� inline-���������� � ��������� ������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(GetDate);
//app.Run(async (context) => await context.Response.WriteAsync("Hello METANIT.COM"));
//app.Run();
async Task GetDate(HttpContext context, Func<Task> next)
{
    string? path = context.Request.Path.Value?.ToLower();
    if (path == "/date")
    {
        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
    }
    else
    {
        await next.Invoke();
    }
}

//  �������� ������� ����� ������������ � ������ ������ ������ Use, � ������� ������������ ������� RequestDelegate:

async Task GetDate2(HttpContext context, RequestDelegate next)
{
    string? path = context.Request.Path.Value?.ToLower();
    if (path == "/date")
    {
        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
    }
    else
    {
        await next.Invoke(context);
    }
}
#endregion