//  �������������

//  ��� ���������� ������������� � ������� HttpResponse ��������� ����� Redirect():
//      void Redirect(string location)
//      void Redirect(string location, bool permanent)

//  ������ ������ ��������� ��������� �������������. � �������� ��������� �������� ����� ��� ���������, � �������
//  ���������� ��������� ��� 302.

//  ������ ������ ������ ����� � �������� ������� ��������� �������� ������� ��������, ������� ���������, ����� ��
//  ������������� ����������. ���� ���� �������� ����� true, �� ������������� ����� ����������, � � ���� ������
//  ���������� ��������� ��� 301. ���� ����� false, �� ������������� ���������, � ���������� ��������� ��� 302.

//  ��������, � ��� ���� ��������� ����������

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    if (context.Request.Path == "/old")
//    {
//        await context.Response.WriteAsync("Old Page");
//    }
//    else
//    {
//        await context.Response.WriteAsync("Main Page");
//    }
//});
//app.Run();

//  ��� ��������� �� ������ "/old" ���������� �������� ��������� "Old Page".

//  �� ����� �� ������ ������� ������������� � ������ "/old" �� "/new". ���������� ��� ����� ������ ������ ������ Redirect:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    if (context.Request.Path == "/old")
    {
        context.Response.Redirect("/new");
        //context.Response.Redirect("https://www.google.com/search?q=metanit.com");
    }
    else if (context.Request.Path == "/new")
    {
        await context.Response.WriteAsync("New Page");
    }
    else
    {
        await context.Response.WriteAsync("Main Page");
    }
});
app.Run();
//  ������ ��� ��������� �� ������ "/old" ���������� ��������������� �� ����� "/new".

//  � ������ ������ ����������� �������� �� ��������� ����� � ������ ����������.
//  �� ����� ����� ������������ �������� �� ������� �������:

//  if (context.Request.Path == "/old")
//{
//    context.Response.Redirect("https://www.google.com/search?q=metanit.com");
//}