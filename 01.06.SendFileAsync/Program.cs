//  �������� ������

//  ��� �������� ������ ����������� ����� SendFileAsync(), ������� �������� ���� ���� � ����� � ���� ������,
//  ���� ���������� � ����� � ���� ������� IFileInfo. ��������, �������� ��� ���� ��������� ���� �� ������ "D:\\forest.jpg":

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    await context.Response.SendFileAsync("forest.jpg");
//});
//app.Run();

//  �� ��������� ������� ���������� ������� ����. ���, � ������ � ������������� ��� ������������ � ��������.

//  ����� �� ����� ������������ ������������� ����. ��������, ������� � ������ �����-������ ����
//  (� ���� ������ ��� ���� forest.jpg). ��� ����� ����� � ���� ������� ��������� ��� ����� Copy to Output Directory
//  �������� Copy if newer ��� Copy always, ����� ���� ������������� ����������� � �������� ������� ��� ����������
//  ����������. � ��������� ������������� ���� ������������ ����� ���������� "forest.jpg".

#region �������� html-��������
//  �������� ������� �� ����� ���������� � ������ ���� ������, ��������, html-��������. ���, ��������� � ������� �����
//  �����, ������� ������� html. � ��� ����� ������� ����� ���� index.html:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context => 
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.SendFileAsync("html/index.html");
//});
//app.Run();

//  ������ ������� �������� ������. ������� � ������ � ����� html ��� ���� ������. ������� ��, � �������, about.html
//  � contact.html. ��� �������� ���� ������ ��������� ��������� ���:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    var path = context.Request.Path;
//    var fullPath = $"html/{path}";
//    var response = context.Response;

//    response.ContentType = "text/html; charset=utf-8";
//    if (File.Exists(fullPath))
//    {
//        await response.SendFileAsync(fullPath);
//    }
//    else
//    {
//        response.StatusCode = 404;
//        await response.WriteAsync("<h2>Not Found</h2>");
//    }
//});
//app.Run();

//  ����� �������� ������, �� ������������ ���� ������� (path) � ������� � ����� html. �� ���� ���� path = about.html,
//  �� ��� ���� �������� � ����� ���� about.html. ��� ���� ��������� ������� �����. ���� �� ���� � �����, �� ����������
//  ������ ����. ���� ���, �� ���������� ��������� ��� 404 � ���������, ��� ����� �� ������

//  ����� ��������, ��� � ASP.NET Core ��� ������� ���������� middleware, ������� ��������� ��������� ������ ��
//  ������������ �������.
#endregion

#region �������� �����
//  �� ��������� ������� �������� ������� ������������ ����, ��� ����� ���� ������� � ������ ������ html - �� �����
//  ���������� ���� html � ����� ������� ��������� ������� ���-��������. �� ����� ����� ���� ����������, ����� �������
//  �������� ���� ��� ��� ��������. � ���� ������ �� ����� ���������� ��� ��������� "Content-Disposition" ��������
//  "attachment":

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    context.Response.Headers.ContentDisposition = "attachment;filename=my_forest.jpg";
//    await context.Response.SendFileAsync("forest.jpg");
//});
//app.Run();

//  � ���� ������ ����������� ���� ������� ��� "my_forest.jpg"
#endregion

#region IFileInfo
//  � �������� ���� ����������� ������ ������ SendFileAsync(), ������� �������� ���� � ����� � ���� ������. �����
//  ����� ������������ ������ ������, ������� �������� ���������� � ����� � ���� ������� IFileInfo:

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
    var fileinfo = fileProvider.GetFileInfo("forest.jpg");

    context.Response.Headers.ContentDisposition = "attachment; filename=my_forest2.jpg";
    await context.Response.SendFileAsync(fileinfo);
});

app.Run();

//  � ���� ������ ������� ���������� ���������� ������ PhysicalFileProvider, ����������� �������� ��������
//  ������� ��� ������ ������. � ��� ����� fileProvider.GetFileInfo() ���������� ���� � ����� � ������ ����� ��������.
//  � ����������� ������ �������� ������ IFileInfo, ������� ���������� � SendFileAsync()
#endregion