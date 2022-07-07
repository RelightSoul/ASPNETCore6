//  ������ middleware

//  � ������� ����� ������������ ���������� middleware ���������� ������������ ������, �� ���� ��� ���������� inline middleware.
//  ������ ����� ASP.NET Core ��������� ���������� middleware � ���� ��������� �������.

//  ����, ������� � ������ ����� �����, ������� ������� TokenMiddleware � ������� ����� ����� ��������� ���:

//public class TokenMiddleware
//{
//    private readonly RequestDelegate next;

//    public TokenMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        var token = context.Request.Query["token"];
//        if (token != "12345678")
//        {
//            context.Response.StatusCode = 403;
//            await context.Response.WriteAsync("Token is invalid");
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}

//  ����� middleware ������ ����� �����������, ������� ��������� �������� ���� RequestDelegate. ����� ���� �������� ����� ��������
//  ������ �� ��� ������� �������, ������� ����� ��������� � ��������� ��������� �������.

//  ����� � ������ ������ ���� ��������� �����, ������� ������ ���������� ���� Invoke, ���� InvokeAsync. ������ ���� ����� ������
//  ���������� ������ Task � ��������� � �������� ��������� �������� ������� - ������ HttpContext. ������ ����� ���������� � �����
//  ������������ ������.

//  ���� �������� ������ ����������� � ���, ��� �� �������� �� ������� �������� "token". ���� ���������� ����� ����� ������ "12345678",
//  �� �������� ������ ������ ���������� ����������, ������ ����� _next.Invoke(). ����� ���������� ������������ ��������� �� ������.

//  ��� ���������� ���������� middleware, ������� ������������ �����, � �������� ��������� ������� ����������� ����� UseMiddleware().
//  ���, ������� ���� Program.cs ��������� �������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseMiddleware<TokenMiddleware>();

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello all");
//});
//app.Run();

//  � ������� ������ UseMiddleware<T> � ����������� ������� TokenMiddleware ����� ���������� ������ ��� ��������� RequestDelegate next.
//  ������� ����� ������� ���������� �������� ��� ����� ��������� ��� �� �����.

//  �������� ������.� ���� �� �� ��������� ����� ������ ������� �������� token ��� ��������� ��� ���� ��������, �������� �� "12345678",
//  �� ������� ��������� ������. ���� �� ����� ������� ���������� ����� https://localhost:7097/?token=12345678, �� �����
//  app.UseMiddleware<TokenMiddleware>() �������� ��������� ������� � ��������� middleware �� app.Run()

#region ����� ���������� ��� ����������� middleware
//  ����� ������� ��� ����������� �������� ����������� middleware ������������ ����������� ������ ����������. ���, ������� � ������ �����
//  �����, ������� ������� TokenExtensions:

//public static class TokenExtensions
//{
//    public static IApplicationBuilder UseToken(this IApplicationBuilder builder)
//    {
//        return builder.UseMiddleware<TokenMiddleware>();
//    }
//}

//  ����� ��������� ����� ���������� ��� ���� IApplicationBuilder. � ���� ����� ���������� ��������� TokenMiddleware � �������� ���������
//  �������. ��� �������, �������� ������ ���������� ������ IApplicationBuilder.

//  ������ �������� ���� ����� � ���� ��������� � Program.cs:

//  var builder = WebApplication.CreateBuilder();
//  var app = builder.Build();

//  app.UseToken();

//  app.Run(async (context) => await context.Response.WriteAsync("Hello METANIT.COM"));

//  app.Run();
#endregion

#region �������� ����������
var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseToken("55555");

app.Run(async context =>
{
    await context.Response.WriteAsync("Hey all");
});
app.Run();

//  ������� ����� TokenMiddleware, ����� �� ����� ������� ������� ������ ��� ���������:

public class TokenMiddleware
{
    private readonly RequestDelegate next;
    string pattern;
    public TokenMiddleware(RequestDelegate next, string pattern)
    {
        this.next = next;
        this.pattern = pattern; 
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"];
        if (token != pattern)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid Token");
        }
        else
        {
            await next.Invoke(context);
        }
    }
}

//  ������� ������, � ������� ���� ���������, ��������������� ����� �����������. ����� �������� ��� � �����������, �������
//  ����� TokenExtensions:

public static class TokenExtensions
{
    public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string pattern)
    {
        return builder.UseMiddleware<TokenMiddleware>(pattern);
    }
}

//  � ����� builder.UseMiddleware ����� �������� ����� ��������, ������� ���������� � ����������� ���������� middleware.
#endregion