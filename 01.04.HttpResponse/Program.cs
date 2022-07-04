//  HttpResponse. �������� ������

//  ��� ������ ������� ���������� � middleware ����� ������ Microsoft.AspNetCore.Http.HttpContext. ���� ������ �������������
//  ���������� � �������, ��������� ��������� ������� �, ����� ����, ����� ��� ����� ������ ����������������. ��������,
//  ������� ���������� ����������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async context => await context.Response.WriteAsync("Hello Metanit",System.Text.Encoding.Default));
//app.Run();

//  ����� �������� context, ������� ���������� � middleware � ������ app.Run() ��� ��� ������������ ������ HttpContext.
//  � ����� ���� ������, ������ ����� ��� �������� Response �� ����� ��������� ������� ��������� �����:
//  context.Response.WriteAsync($"Hello METANIT.COM").

//  �������� Response ������� HttpContext ������������ ������ HttpResponse � �������������, ��� ����� ����������� � ����
//  ������. ��� ��������� ��������� �������� ������ ����� HttpResponse ���������� ��������� ��������:
//Body: �������� ��� ������������� ���� ������ � ���� ������� Stream
//BodyWriter: ���������� ������ ���� PipeWriter ��� ������ ������
//ContentLength: �������� ��� ������������� ��������� Content-Length
//ContentType: �������� ��� ������������� ��������� Content-Type
//Cookies: ���������� ����, ������������ � ������
//HasStarted: ���������� true, ���� �������� ������ ��� ��������
//Headers: ���������� ��������� ������
//Host: �������� ��� ������������� ��������� Host
//HttpContext: ���������� ������ HttpContext, ��������� � ������ �������� Response
//StatusCode: ���������� ��� ������������� ��������� ��� ������

//  ����� ��������� �����, �� ����� ������������ ��� ������� ������ HttpResponse:
//Redirect(): ��������� �������������(��������� ��� ����������) �� ������ ������
//WriteAsJson()/ WriteAsJsonAsync(): ���������� ����� � ���� �������� � ������� JSON
//WriteAsync(): ���������� ��������� ����������. ���� �� ������ ������ ��������� ������� ���������.
//���� ��������� �� �������, �� �� ��������� ����������� ��������� UTF-8
//SendFileAsync(): ���������� ����

//  ����� ������� ������ �������� ������ ������������ ����� WriteAsync(), � ������� ���������� ������������ ������.
//  � �������� ��������������� ��������� �� ����� ������� ���������

#region ��������� ����������
//  ��� ��������� ���������� ����������� �������� Headers, ������� ������������ ��� IHeaderDictionary. ��� �����������
//  ����������� ���������� HTTP � ���� ���������� ���������� ����������� ��������, ��������, ��� ��������� "content-type"
//  ���������� �������� ContentType. ������, � ��� ����� ���� ��������� ��������� ����� �������� ����� ����� Append(). ��������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async (context) =>
//{
//    var response = context.Response;
//    response.Headers.ContentLanguage = "ru-Ru";
//    response.Headers.ContentType = "text/plain; charset=utf-8";
//    response.Headers.Append("secret-id", "256");
//    await response.WriteAsync("Hello Metanit");
//});
//app.Run();

//  ����� ��������, ��� ��� ������ ��������� ���������� ������������� ��������� ContentType, � ��� ����� ���������,
//  ������� ����������� � ������������ ���������� (� ������� ���� ��� "text/plain; charset=utf-8").

//  ����� ����� ��������, ��� ������
//  response.Headers.ContentType = "text/plain; charset=utf-8";
//  ����� ���� �� ��������
//  response.ContentType = "text/plain; charset=utf-8";
#endregion

#region ��������� ����� �������
//  ��� ��������� ��������� ����� ����������� �������� StatusCode, �������� ���������� �������� ��� �������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();
//app.Run(async context =>
//{
//    context.Response.StatusCode = 404;
//    await context.Response.WriteAsync("Resourse not found");
//});
//app.Run();

//  � ������ ������ ��������������� ��� 404, ������� ���������, ��� ������ �� ������.
#endregion

#region �������� html-����
//  ���� ���������� ��������� html-���, �� ��� ����� ���������� ���������� ��� ��������� Content-Type �������� text/html:

WebApplicationBuilder builder = WebApplication.CreateBuilder();
WebApplication app = builder.Build();

app.Run(async context =>
{
    HttpResponse? response = context.Response;
    response.ContentType = "text/html, charset=utf-8";
    await response.WriteAsync("<h2>Hello Metanit</h2><h3>Welcome to ASP.NET</h3>");
});
app.Run();
#endregion