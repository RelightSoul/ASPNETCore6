//  HttpRequest. ��������� ������ �������

//  �������� Request ������� HttpContext ������������ ������ HttpRequest � ������ ���������� � ������� � ���� ��������� �������:
//Body: ������������� ���� ������� � ���� ������� Stream
//BodyReader: ���������� ������ ���� PipeReader ��� ������ ���� �������
//ContentLength: �������� ��� ������������� ��������� Content-Length
//ContentType: �������� ��� ������������� ��������� Content-Type
//Cookies: ���������� ��������� ���� (������ Cookies), ��������������� � ������ ��������
//Form: �������� ��� ������������� ���� ������� � ���� ����
//HasFormContentType: ��������� ������� ��������� Content-Type
//Headers: ���������� ��������� �������
//Host: �������� ��� ������������� ��������� Host
//HttpContext: ���������� ��������� � ������ �������� ������ HttpContext
//IsHttps: ���������� true, ���� ����������� �������� https
//Method: �������� ��� ������������� ����� HTTP
//Path: �������� ��� ������������� ���� ������� � ���� ������� RequestPath
//PathBase: �������� ��� ������������� ������� ���� �������. ����� ���� �� ������ ��������� ����������� ����
//Protocol: �������� ��� ������������� ��������, ��������, HTTP
//Query: ���������� ��������� ���������� �� ������ �������
//QueryString: �������� ��� ������������� ������ �������
//RouteValues: �������� ������ �������� ��� �������� �������
//Scheme: �������� ��� ������������� ����� ������� HTTP

//  ���������� ���������� ��������� �� ���� �������.

#region ��������� ���������� �������
//  ��� ��������� ���������� ����������� �������� Headers, ������� ������������ ��� IHeaderDictionary. ��������, ������� ���
//  ��������� ������� � ������� �� �� ���-��������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    var stringBuilder = new System.Text.StringBuilder("<table>");
//    foreach (var header in context.Request.Headers)
//    {
//        stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
//    }
//    stringBuilder.Append("</table>");
//    await context.Response.WriteAsync(stringBuilder.ToString());

//});
//app.Run();

//  ��� ����������� ����������� ���������� HTTP � ���� ���������� ���������� ����������� ��������, ��������, ��� ���������
//  "content-type" ���������� �������� ContentType, � ��� ��������� "accept" - �������� Accept:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context => 
//{
//    var acceptHeaderValue = context.Request.Headers.Accept;
//    //var acceptHeaderValue = context.Request.Headers["accept"];
//    await context.Response.WriteAsync(acceptHeaderValue);
//});
//app.Run();

//  ����� ������� ���������, � ����� �����-�� ��������� ���������, ��� ������� �� ���������� �������� ��������, �����
//  �������� ��� � ����� ����� ������� �������:
//  var acceptHeaderValue = context.Request.Headers["accept"];
//  ��� ���� ���������� � ������ HttpRequest ���������� ��������� ��������: Host, Method, ContentType, ContentLength.
#endregion

#region ��������� ���� �������
//  �������� path ��������� �������� ����������� ����, �� ���� �����, � �������� ���������� ������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

////  !!�����������!! ��� �������� ����������� middleware ������������ ������� RequestDelegate, ������� ��������� ���������
////  �������� � ��������� �������� ������� - ������ HttpContext:
////  public delegate Task RequestDelegate(HttpContext context);
//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync($"Path: {context.Request.Path}");
//});
//app.Run();

//  �������� path ��������� �������� ����������� ����, �� ���� �����, � �������� ���������� ������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async (HttpContext context) =>
//{
//    var path = context.Request.Path;
//    var now = DateTime.Now;
//    var response = context.Response;

//    if (path == "/date")
//    {
//        await response.WriteAsync($"Date: {now.ToShortDateString()}");
//    }
//    else if (path == "/time")
//    {
//        await response.WriteAsync($"Time: {now.ToShortTimeString()}");
//    }
//    else
//    {
//        await response.WriteAsync("Hey =)");
//    }
//});
//app.Run();

//  � ������ ������, ���� ������������ ���������� �� ������ "/date", �� ��� ������������ ������� ����, � ����
//  ���������� �� ������ "/time" - ������� �����. � ��������� ������� ������������ ��������� ������������� ���������:

//  �������� ������� ����� ���������� ���� ������� �������������, ������ � ASP.NET Core �� ��������� ���� �����������,
//  ������� ����� ������������ ��� �������� ������� ������������� � ���������� � ������� ����� ����������� � �����������
//  �������.
#endregion

#region ������ �������
//  �������� QueryString ��������� �������� ������ �������. ������ ������� ������������ �� ����� ������������ ������,
//  ������� ���� ����� ������� ? � ������������ ����� ����������, ����������� �������� ���������� &:
//  ? ��������1 = ��������1 & ��������2 = ��������2 & ��������3 = ��������3
//  ������� ��������� � ������� ����� ����� ���������� ��������� ��������.

//  ����� ��������, ��� ������ ������� (query string) �� ������ � ���� ������� (path):

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.WriteAsync($"<p>Path: {context.Request.Path}</p>" +
//        $"<p>QueryString: {context.Request.QueryString}</p>");
//});
//app.Run();

//  ��������� �� ������ https://localhost:7159/time?name=Tom&age=33&sex=male
//  �������
//  Path: / time
//  QueryString: ? name = Tom & age = 33 & sex = male

//  ���� ������� ��� path ������������ �� ����� ������, ������� ���� ����� ������/����� � �� ������� ?.
//  ������ ������� ��� query string ������������ �� ����� ������, ������� ���� ������� � ������� ?.
//  �� ���� � ������ ������ ����� ������ ������� ���������� ��� ���������: name = Tom,  age = 33,  sex = male

//  � ������� �������� Query ����� �������� ��� ��������� ������ ������� � ���� �������:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<h3>��������� ������</h3><table>");
    foreach (var param in context.Request.Query)
    {
        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
    }
    stringBuilder.Append("</table>");
    await context.Response.WriteAsync(stringBuilder.ToString());
});
app.Run();

//  �������������� ����� �������� �� ������� Query �������� ��������� ����������:
//app.Run(async context => 
//{
//    string name = context.Request.Query["name"];
//    string age = context.Request.Query["age"];
//    await context.Response.WriteAsync($"{name} - {age}");
//});
//app.Run();
#endregion

