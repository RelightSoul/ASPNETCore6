//  ��������� �������� ����� � ������� middleware

//  ����� �������� ����� ������ � ��������� ��������� ����� ������������ � ������ ���������� middleware. ��� ���� ���� ���������
//  ����� ������� ��������� ������� � ������ �������� �����.

//  ���, ���� ���������� �������� �������� �����, �� ������� ������������� �� ������ �������� URL matching ��� ������������� ������
//  URL � ��������� �������� �������� ��� ��������� ������������ �������� �����. ���� � ���������� ���� ����� �������� �����, �������
//  ������������� �������, �� ��������� middleware Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware ������������� � �������
//  HttpContext �������� ����� ��� ������� ��������� �������, ������� ����� �������� � ������� ������ HttpContext.GetEndpoint().
//  ����� ����, ��������������� �������� ��������, ������� ����� �������� ����� ��������� HttpRequest.RouteValues

//  ������ �������� ����� �������� ������������ ������ ������ ����� ����, ��� ��� middleware � ��������� ������ ��������� �������.
//  ��������, ������� ��������� ��� ����������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) => 
//{
//    Console.WriteLine("First Middleware starts");
//    await next.Invoke();
//    Console.WriteLine("First Middleware ends");
//});
//app.Map("/", () => 
//{
//    Console.WriteLine("Index endpoint start and ends");
//    return "Index Page";
//});
//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Second middleware starts");
//    await next.Invoke();
//    Console.WriteLine("Second middleware ends");
//});
//app.Map("/about", () =>
//{
//    Console.WriteLine("About endpoint start and ends");
//    return "About Page";
//});

//app.Run();

//  ����� �� � ����� ������ �������� ����� � ������� ������ app.Use() � �������� �������� ��� middleware. ��� ��������� ����� �������
//  ���������� ���������� ������� ���������� ����������� �� ������� ����������.

//  ���� �� �������� ����������, �� ��� ������� �� ������ "/" �������� ��� ��������� ������� ����� ������� ������ �������� �����

//  �� ������ �������� �� ������� ����������:
//First Middleware starts
//Second middleware starts
//Index endpoint start and ends
//Second middleware ends
//First Middleware ends

//  �� �����, ��� �������� ����� ����������� ����� ����, ��� ������ ����������� ��������� middleware, ������� � ���� ���� �����
//  ���������� ���� �������� �����.

//  ��� ���� � ����������� middleware ����� ����� ������������ ������� �� ������������ �������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) => 
//{
//    if (context.Request.Path == "/date")
//    {
//        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
//    }
//    else
//    {
//        await next.Invoke();
//    }
//});

//app.Map("/", () => "Index Page");
//app.Map("/about", () => "About Page");

//app.Run();

//  ����� ����, middleware ����� ���� ������� ��� ���������� ������������ - ���������� ��������� ��������, ����� �������� ����� ���
//  �������. ���, ��������, ���� �� ���� �� �������� ����� �� ���������� ������, � � middleware �� ����� ���������� ��� ��������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    await next.Invoke();
//    if (context.Response.StatusCode == 404)
//    {
//        await context.Response.WriteAsync("Resource not found");
//    }
//});

//app.Map("/", () => "Index Page");
//app.Map("/about", () => "About Page");

//app.Run();

//  ������ ���� � ����� ��������� ������������� ������������ ���������, �� �� ����� ����������� ���� ���� �������� �����
//  ������������� ������������ ����:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/", () => "Index Page");
app.Map("/about", () => "About Page");

app.Run(async context =>
{
    context.Response.StatusCode = 404;
    await context.Response.WriteAsync("Resource not found");
});

app.Run();

//  � ������ ������ �� ���������� ��������� �� �����, ��� ���� ��� �������� �� ������ "/" � "/about" ����� �����������
//  middleware �� ������ app.Run

//  ������ ����� ����������? ����� �� ������ ���, ������� ����������� ��� middleware � ���������, � ������ ����� �����������
//  �������� �����.