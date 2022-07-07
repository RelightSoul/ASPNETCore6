//  ���������� ��������� ��������� �������

//  ������ ��� ��������� ������� ����������� �� ����, � ��������� ����������� middleware. � � ���� ������ ������� ���� ����� ������
//  ������� �� ��������� � �������� ��������� �������, � ����� ��, ��� ��� ��������������� � ������� ������������.

//  ����� ����, ������ ��������� middleware ����� ������������ ������ �� � ����� ����������� � ��������� �����������. ������
//  �������������� ��������� ���������� ����������� �������������� ��������� ��������� ����������� �����������.

//  ���������� ���������� ������. ��������� ��������� ����� RoutingMiddleware ��� ��������� �������:

//public class RoutingMiddleware
//{
//    readonly RequestDelegate next;
//    public RoutingMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        string path = context.Request.Path;
//        if (path == "/index")
//        {
//            await context.Response.WriteAsync("Index Page");
//        }
//        if (path == "/about")
//        {
//            await context.Response.WriteAsync("About Page");
//        }
//        else
//        {
//            context.Response.StatusCode = 404;
//            await context.Response.WriteAsync("Not found");
//        }
//    }
//}

//  ���� ��������� � ����������� �� ������ ������� ���������� ���� ������������ ������, ���� ������������� ��� ������.

//  ��������, �� �����, ����� ������������ ��� ���������������� ��� ��������� � ������ ����������. ��� ����� ������� ����� �����
//  AuthenticationMiddleware:

//public class AuthenticationMiddleware
//{
//    readonly RequestDelegate next;
//    public AuthenticationMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        var token = context.Request.Query["token"];
//        if (string.IsNullOrWhiteSpace(token))
//        {
//            context.Response.StatusCode = 403;
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}

//  ������� ����� �������, ��� ���� � ������ ������� ���� �������� token � �� ����� �����-������ ��������, �� ������������
//  ����������������. � ���� �� �� ����������������, �� ���� ���������� ���������� ������ ������������� � ����������. ����
//  ������������ �� ����������������, �� ������������� ��������� ��� 403, ����� �������� ���������� ������� ���������� �
//  ��������� ��������.

//  ��������� ���������� RoutingMiddleware ��� ������ ������������ ������, ���� ������������ �� ����������������, �� � ���������
//  ��������� AuthenticationMiddleware ������ ���� ������� ����� ����������� RoutingMiddleware:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<RoutingMiddleware>();

app.Run();

//  ����� �������, ���� �� ������ �������� ������ � ��������� �� ���� "/index" ��� "/about" � �� ��������� �������� token, �� ��
//  ������� ������. ���� �� ��������� �� ���� /index ��� /about � ��������� �������� ��������� token, �� ������ ������� �����
//  https://localhost:7297/about?token=33424   // About Page 
//  https://localhost:7297/about               // Access Denied
//  https://localhost:7297/main?token=33424    // Not found

//  ������� ��� ���� ��������� middleware, ������� ������� ErrorHandlingMiddleware:

public class ErrorHandlingMiddleware
{
    readonly RequestDelegate next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        await next.Invoke(context);
        if (context.Response.StatusCode == 403)
        {
            await context.Response.WriteAsync("Access Denied");
        }
        else if (context.Response.StatusCode == 404)
        {
            await context.Response.WriteAsync("Not Found");
        }
    }
}
//  � ������� �� ���������� ���� ����������� ErrorHandlingMiddleware ������� �������� ������ �� ���������� ����������� ���������,
//  � ����� ��� ��� ������������. ��� ��������, ��������� ������ ��������� ������������ ������ ��� ����: ������� ���������� �� �����
//  ����, ������� ���� �� await next.Invoke(context);, � ����� ���������� ��������� ����������� ����������� ���������� �� ����� ����,
//  ������� ���� ����� await next.Invoke(context);. � � ������ ������ ��� ErrorHandlingMiddleware ����� ��������� ��������� �������
//  ������������ ������������. � ���������, �� ������������� ��������� �� ������� � ����������� �� ����, ��� ��������� ��� ����������
//  ������ ����������. ������� ErrorHandlingMiddleware ������ ���� ������� ������ �� ���� ���� �����������

#region ����� ����
public class RoutingMiddleware
{
    readonly RequestDelegate next;
    public RoutingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;
        if (path == "/index")
        {
            await context.Response.WriteAsync("Index Page");
        }
        if (path == "/about")
        {
            await context.Response.WriteAsync("About Page");
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }
}
public class AuthenticationMiddleware
{
    readonly RequestDelegate next;
    public AuthenticationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = 403;
        }
        else
        {
            await next.Invoke(context);
        }
    }
}
#endregion