//  ������ �� ������������ �������
//  ���������� ��������� �����������, ������� ������������� ��� ASP.NET Core ��� ������ �� ������������ �������.

#region ����� �� ���������
//  ��������, � ������� � ����� wwwroot ������������� ���� index.html:

//  � ������� ������������ ������ ���������� UseDefaultFiles() ����� ��������� �������� ����������� ���-������� �� ���������
//  ��� ��������� � ��� �� ������� ����:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseDefaultFiles();    // ��������� ������� html �� ���������
//app.UseStaticFiles();

//app.Run(async (context) => await context.Response.WriteAsync("Hello World"));

//app.Run();

//  � ���� ������ ��� �������� ������� � ����� ���-���������� ���� http://localhost:xxxx/ ���������� ����� ������ � ����� wwwroot
//  ��������� �����:
//      default.htm
//      default.html
//      index.htm
//      index.html

//���� ���� ����� ������, �� �� ����� ��������� � ����� �������.

//  ���� �� ���� �� ����� ������, �� ������������ ������� ��������� ������� � ������� ��������� ����������� middleware.
//  �� ���� ���������� ��� ����� ����������, ��� ����� �� ���������� � �����: http://localhost/index.html

//  ���� �� �� ����� ������������ ����, �������� �������� ���������� �� �����������������, �� ��� ���� � ���� ������ ���������
//  ������ DefaultFilesOptions:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//DefaultFilesOptions options = new DefaultFilesOptions();
//options.DefaultFileNames.Clear();            // ������� ����� ������ �� ���������
//options.DefaultFileNames.Add("hello.html");  // ��������� ����� ��� �����
//app.UseDefaultFiles();    // ��������� ������� html �� ���������

//app.UseStaticFiles();

//app.Run(async (context) => await context.Response.WriteAsync("Hello World"));

//app.Run();

//  � ���� ������ � �������� �������� �� ��������� ����� �������������� ���� hello.html, ������� ������ ������������� � ����� wwwroot.
#endregion

#region ����� UseDirectoryBrowser
//  ����� UseDirectoryBrowser ��������� ������������� ������������� ���������� ��������� �� �����:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseDirectoryBrowser();
//app.UseStaticFiles();

//app.Run();

//  ������ ����� ����� ����������, ������� ��������� ����������� ������������ ������� �� ������� ����� ��� � ������� � ���������
//  ������� ������� � ��� ����� ����� ���������� ���������� ����� ��������:

//using Microsoft.Extensions.FileProviders;

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseDirectoryBrowser(new DirectoryBrowserOptions()
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\html")),

//    RequestPath = new PathString("/pages")
//});

//app.UseStaticFiles();

//app.Run();

//  ����� ������������� ����� ����������, ���� ���������� ������������ ���� using Microsoft.Extensions.FileProviders.

//  � �������� ��������� ����� UseDirectoryBrowser() ��������� ������ DirectoryBrowserOptions, ������� ��������� ��������� �������������
//  ����� � ������ � ����������. ���, � ������ ������ ���� ���� http://localhost:xxxx/pages/ ����� �������������� � ���������
//  "wwwroot\html".
#endregion

#region ������������� ��������� � ������
//  ���������� ������ UseStaticFiles() ��������� ����������� ���� � ������������� ����������:

//using Microsoft.Extensions.FileProviders;

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions()   // ������������ ������� � �������� wwwroot/html
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\html")),

//    RequestPath = new PathString("/pages")
//});
//app.Run();

//  ������ ����� app.UseStaticFiles() ������������ ������� � ������ � ����� wwwroot. ������ ����� ��������� �� �� ���������, ��� �
//  ����� app.UseDirectoryBrowser() � ���������� �������. � � ������� �� ������� ������ �� ������������ ������� �� ����
//  http://localhost:xxxx/pages, ����������� ������ ������� � ������ wwwroot/html. � �������, �� �������
//  http://localhost:xxxx/pages/index.html �� ����� ���������� � ����� wwwroot/html/index.html.
#endregion

#region ����� UseFileServer
//  ����� UseFileServer() ���������� ���������������� ����� ���� ���� ������������� ������� UseStaticFiles, UseDefaultFiles
//  � UseDirectoryBrowser:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseFileServer();

//app.Run();

//  �� ��������� ���� ����� ��������� ������������ ����������� ����� � ���������� ����� �� ��������� ���� index.html. ����
//  ��� ���� ��� �������� �������� ���������, �� �� ����� ������������ ���������� ������� ������:
//      app.UseFileServer(enableDirectoryBrowsing: true);

//  ��� ���� ���������� ������ ��������� ����� ����� ������ ���������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseFileServer(new FileServerOptions()
//{
//    EnableDirectoryBrowsing = true,
//    EnableDefaultFiles = false
//});

//app.Run();

//  ����� ����� ��������� ������������� ����� ������� � ����������:

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseFileServer(new FileServerOptions()
{
    EnableDirectoryBrowsing = true,
    EnableDefaultFiles = false,
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\html")),
    RequestPath = new PathString("/pages")
});

app.Run();

//  � ���� ������ ����� �������� ����� �������� �� ���� http://localhost:xxxx/pages/, �� ��� ���� ����
//  http://localhost:xxxx/html/ �������� �� �����.
#endregion