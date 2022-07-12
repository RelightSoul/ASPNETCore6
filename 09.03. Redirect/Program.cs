//  ������������� � Results API

#region LocalRedirect
//  ��� ������������� �� ��������� ����� � ������ ���������� ����������� ����� LocalRedirect():
//      public static IResult LocalRedirect (string localUrl, bool permanent = false, bool preserveMethod = false);

//  ��������� ������:
//      localUrl: ��������� ����� ��� �������������
//      permanent: ���������, ����� �� ������������� ���������� (������������ ��������� ��� 301) ��� ���������(����������� ���������
//      ��� 302).
//      preserveMethod: ���� ����� true, �� ����������� ������������ ����� HTTP-�������.

//  ���������� ������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/old", () => Results.LocalRedirect("/new"));
//app.Map("/new", () => Results.Text("New address"));

//app.Map("/", () => "Hello World");

//app.Run();

//  � ������ ������ ��� ��������� �� ���� "/old" ����� �������������� �������� �� ����� "/new". ��������� ������ �������� �� �����,
//  �� ����� ����������� ��������� �������������.
#endregion

#region ����� Redirect
//  ����� Redirect() ����� ������������ ������������� � ��������� �� �� ���������, ��� � LocalRedirect(), ������ ����� ���
//  ������������� ����� �� ������ ���������, �� � �������:
//      public static IResult Redirect(string url, bool permanent = false, bool preserveMethod = false);

//  ������ ������:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/old", () => Results.Redirect("https://metanit.com"));
app.Map("/", () => Results.Text("Hello <3"));

app.Run();
#endregion

#region RedirectToRoute
//  ����� RedirectToRoute() ��������� ������������� �� ������������ �������:
//      public static IResult RedirectToRoute (string? routeName = default, object? 
//          routeValues = default, bool permanent = false, bool preserveMethod = false, string? fragment = default);

//  ��������� ������:
//      routeName: �������� ��������
//      routeValues: �������� ��� ���������� ��������
//      permanent: ���������, ����� �� ������������� ���������� (������������ ��������� ��� 301) ��� ���������(����������� ���������
//      ��� 302).
//      preserveMethod: ���� ����� true, �� ����������� ������������ ����� HTTP-�������.
//      fragment: ��������, ������� ����������� � ������ ��� �������������
#endregion