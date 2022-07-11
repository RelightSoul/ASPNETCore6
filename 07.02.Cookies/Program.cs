//  ����

//  ���� ������������ ����� ������� ������ ��������� ������ ������������. ���� �������� �� ���������� ������������ � �����
//  ��������������� ��� �� �������, ��� � �� �������. ��� ��� ���� ���������� � ������ �������� �� ������, �� �� ������������ ������
//  ��������� 4096 �������.

//  ��� ������ � ������ ����� ����� ������������ �������� ������� HttpContext, ������� ���������� � �������� ��������� � ����������
//  middleware, � ����� �������� � ������������ � RazorPages.

//  ����� �������� ����, ������� �������� ������ � �������� � ����������, ��� ���� ������������ ��������� Request.Cookies �������
//  HttpContext. ��� ��������� ������������ ������ IRequestCookieCollection, � ������� ������ ������� - ��� ������
//  KeyValuePair<string, string>, �� ���� ��������� ���� ����-��������.

//  ��� ���� ��������� ���������� ��������� �������:
//      bool ContainsKey(string key): ���������� true, ���� � ��������� ��� ���� ���� � ������ key
//      bool TryGetValue(string key, out string value): ���������� true, ���� ������� �������� �������� ���� � ������ key �
//      ���������� value

//  ����� ��������, ��� ���� - ��� ��������� ��������. �������, ��� �� ��������� ��������� � ���� - ��� ��� ���������� ���������
//  � ������ � �������������� ��������� �� ��� �� ���� ������.

//  ��������, ������� ���� "name":
//  if (context.Request.Cookies.ContainsKey("name"))
//     string name = context.Request.Cookies["name"];
//  ���������, ��� ��������� context.Request.Cookies ������ ������ ��� ��������� �������� ���.

//  ��� ��������� ���, ������� ������������ � ����� �������, ����������� ������ context.Response.Cookies, ������� ������������
//  ��������� IResponseCookies. ���� ��������� ���������� ��� ������:
//      Append(string key, string value): ��������� ��� ���� � ������ key �������� value
//      Delete(string key): ������� ���� �� �����

//  ��������, ��������� � ���������� ��������� � ��������� ���:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    if (context.Request.Cookies.ContainsKey("name"))
    {
        await context.Response.WriteAsync($"Hello {context.Request.Cookies["name"]}");
    }
    else
    {
        context.Response.Cookies.Append("name", "Vlad");
        await context.Response.WriteAsync("Hello man");
    }
});

app.Run();

//  ��� ��������� ������� �� �������, ����������� �� ���� "name":
//  if (context.Request.Cookies.ContainsKey("name"))

//  ���� ���� �� �����������(��������, ��� ������ ��������� � ����������), �� ������������� �� � ���������� ������������ � �����
//  ������ "Hello World!".
//  context.Response.Cookies.Append("name", "Tom");
//  await context.Response.WriteAsync("Hello World!");

//  ���� ���� �����������, �� �������� �� �������� � ���������� ��� ������������
//  await context.Response.WriteAsync($"Hello {context.Request.Cookies["name"]}");

//  ����� ��������� ��� � ���������� ������� ������� � �������� ����� ����������� ���� name. � ����� �������� ������������ �
//  ���-�������� �� ������ ������� �� �������� (���� ������� ������������ ��������). ��������, � Google Chrome ���������� ����
//  ����� �� ������� Application -> Cookies

//  � ������� ���� �� ����� ����� �� ��������� ����� TryGetValue() ��� ��������� ���:

app.Run(async (context) =>
{
    if (context.Request.Cookies.TryGetValue("name", out var login))
    {
        await context.Response.WriteAsync($"Hello {login}!");
    }
    else
    {
        context.Response.Cookies.Append("name", "Tom");
        await context.Response.WriteAsync("Hello World!");
    }
});

//  ����� ��������, ��� ������������ ����� �������� ����������� (���� ���-������� ���������) ����� ������� �������� �������� ���,
//  ���� ����� ������� ��.