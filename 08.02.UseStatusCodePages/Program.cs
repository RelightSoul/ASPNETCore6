//  ��������� ������ HTTP

//  � ������� �� ���������� ����������� ���������� ������� ASP.NET Core ����� ����� �� ������������ ������ HTTP, ��������, �
//  ������ ���� ������ �� ������. ��� ��������� � ��������������� ������� �� ������ � �������� ������ ��������, � ������ �����
//  ������� ���-�������� �� ������ ������� ��������� ���. ���� ������� ����� ���������� �����-�� ����������� ��������.

//  �� � ������� ���������� StatusCodePagesMiddleware ����� �������� � ������ �������� ���������� � ��������� ����. ��� �����
//  ����������� ����� app.UseStatusCodePages():

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//// ��������� ������ HTTP
//app.UseStatusCodePages();

//app.Map("/hello", () => "Hello ASP.NET Core");

//app.Run();

//  ����� �� ����� ���������� ������ �� ������ "/hello". ��� ��������� �� ���� ��������� ������� ������� ��������� �������
//  ���������� �� ������:

//  ����� UseStatusCodePages() ������� �������� ����� � ������ ��������� ��������� �������, � ���������, �� ���������� middleware
//  ��� ������ �� ������������ ������� � �� ���������� �������� �����.

#region ��������� ���������
//  ���������, ������������ ������� UseStatusCodePages() �� ���������, �� ����� �������������. ������ ���� �� ������ ������
//  ��������� ��������� ������������ ������������ ���������. � ���������, �� ����� �������� ����� ������ ���:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePages("text/plain","Error: Resource Not Found. Status code: {0}");

//app.Map("/hello", () => "Hello ASP.NET Core");

//app.Run();

//  � �������� ������� ��������� ����������� MIME-��� ������ - � ������ ������ ������� ����� (""text/plain""). � �������� �������
//  ��������� ���������� ���������� �� ���������, ������� ������ ������������. � ��������� �� ����� �������� ��� ������ �����
//  ����������� "{0}".
#endregion

#region ��������� ����������� ������
//  ��� ���� ������ ������ UseStatusCodePages() ��������� ����� �������� ������ ��������� ������. � ���������, ��� ��������� �������,
//  �������� �������� - ������ StatusCodeContext. � ���� �������, ������ StatusCodeContext ����� �������� HttpContext, �� ��������
//  �� ����� �������� ��� ���������� � ������� � ��������� �������� ������. ��������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePages(async statusCodeContext => 
//{
//    var response = statusCodeContext.HttpContext.Response;
//    var path = statusCodeContext.HttpContext.Request.Path;

//    response.ContentType = "text/plain; charset=UTF-8";
//    if (response.StatusCode == 403)
//    {
//        await response.WriteAsync($"Path: {path}. Access Denied");
//    }
//    else if (response.StatusCode == 404)
//    {
//        await response.WriteAsync($"Resource {path} Not Found");
//    }
//});

//app.Map("/hello", () => "Hello ASP.NET Core");

//app.Run();
#endregion

#region ������ UseStatusCodePagesWithRedirects � UseStatusCodePagesWithReExecute
//  ������ ������ UseStatusCodePages() �� ����� ����� ������������ ��� ���� ������, ������� ����� ������������ ������ HTTP.

//  � ������� ������ app.UseStatusCodePagesWithRedirects() ����� ��������� ������������� �� ������������ �����, �������
//  ��������������� ���������� ��������� ���:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePagesWithRedirects("/error/{0}");

//app.Map("/hello", () => "Hello ASP.NET Core");
//app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

//app.Run();

//  ����� ����� ���� ��������������� �� ������ "/error/{0}". � �������� ��������� ����� ����������� "{0}" ����� ������������
//  ��������� ��� ������.

//  �� ������ ��� ��������� � ��������������� ������� ������ ������� ��������� ��� 302 / Found. �� ���� ��������� ��������������
//  ������ ����� ������������, ������ ��������� ��� 302 ����� ���������, ��� ������ ��������� �� ������ ����� - �� ���� "/error/404".

//  �������� ��������� ����� ���� ��������, �������� � ����� ������ ��������� ����������, � � ���� ������ �� ����� ���������
//  ������ ����� app.UseStatusCodePagesWithReExecute():

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.Map("/hello", () => "Hello ASP.NET Core");
app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

app.Run();

//  � �������� ��������� ����� UseStatusCodePagesWithReExecute() ��������� ���� � �������, ������� ����� ������������ ������. �
//  ����� � ������� ������������ {0} ����� �������� ��������� ��� ������. �� ���� � ������ ������ ��� ������������� ������ �����
//  ���������� �������� �����
//      app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

//  ��������� �� ������� ��� �� �����, ��� ��� ��� �� ����� ���� ��������������� �� ���� "/error/404". �� ������ ������� �������
//  ������������ ��������� ��� 404.

//  ����� ����� ������������� ������ ������ ������, ������� � �������� ������� ��������� ��������� ��������� ������ �������
//      app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

//  ������ �������� ������ ��������� �� ���� ���������������, � ������ ������ ��������� ������ �������, ������� ����� ������������
//  ��� ���������������. ������ ������������ {0} ����� �� ����� ������������ ��������� ��� ������. ������ �������������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

//app.Map("/hello", () => "Hello ASP.NET Core");
//app.Map("/error", (string code) => $"Error Code: {code}");


//app.Run();
#endregion