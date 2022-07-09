//  Работа со статическими файлами
//  Рассмотрим некоторые возможности, которые предоставляет нам ASP.NET Core для работы со статическими файлами.

#region Файлы по умолчанию
//  Допустим, в проекте в папке wwwroot располагается файл index.html:

//  С помощью специального метода расширения UseDefaultFiles() можно настроить отправку статических веб-страниц по умолчанию
//  без обращения к ним по полному пути:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseDefaultFiles();    // поддержка страниц html по умолчанию
//app.UseStaticFiles();

//app.Run(async (context) => await context.Response.WriteAsync("Hello World"));

//app.Run();

//  В этом случае при отправке запроса к корню веб-приложения типа http://localhost:xxxx/ приложение будет искать в папке wwwroot
//  следующие файлы:
//      default.htm
//      default.html
//      index.htm
//      index.html

//Если файл будет найден, то он будет отправлен в ответ клиенту.

//  Если же файл не будет найден, то продолжается обычная обработка запроса с помощью следующих компонентов middleware.
//  То есть фактически это будет аналогично, как будто мы обращаемся к файлу: http://localhost/index.html

//  Если же мы хотим использовать файл, название которого отличается от вышеперечисленных, то нам надо в этом случае применить
//  объект DefaultFilesOptions:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//DefaultFilesOptions options = new DefaultFilesOptions();
//options.DefaultFileNames.Clear();            // удаляем имена файлов по умолчанию
//options.DefaultFileNames.Add("hello.html");  // добавляем новое имя файла
//app.UseDefaultFiles();    // поддержка страниц html по умолчанию

//app.UseStaticFiles();

//app.Run(async (context) => await context.Response.WriteAsync("Hello World"));

//app.Run();

//  В этом случае в качестве страницы по умолчанию будет использоваться файл hello.html, который должен располагаться в папке wwwroot.
#endregion

#region Метод UseDirectoryBrowser
//  Метод UseDirectoryBrowser позволяет пользователям просматривать содержимое каталогов на сайте:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseDirectoryBrowser();
//app.UseStaticFiles();

//app.Run();

//  Данный метод имеет перегрузку, которая позволяет сопоставить определенный каталог на жестком диске или в проекте с некоторой
//  строкой запроса и тем самым потом отобразить содержимое этого каталога:

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

//  Чтобы задействовать новый функционал, надо подключить пространство имен using Microsoft.Extensions.FileProviders.

//  В качестве параметра метод UseDirectoryBrowser() принимает объект DirectoryBrowserOptions, который позволяет настроить сопоставление
//  путей к файлам с каталогами. Так, в данном случае путь типа http://localhost:xxxx/pages/ будет сопоставляться с каталогом
//  "wwwroot\html".
#endregion

#region Сопоставление каталогов с путями
//  Перегрузка метода UseStaticFiles() позволяет сопоставить пути с определенными каталогами:

//using Microsoft.Extensions.FileProviders;

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions()   // обрабатывает запросы к каталогу wwwroot/html
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\html")),

//    RequestPath = new PathString("/pages")
//});
//app.Run();

//  Первый вызов app.UseStaticFiles() обрабатывает запросы к файлам в папке wwwroot. Второй вызов принимает те же параметры, что и
//  метод app.UseDirectoryBrowser() в предыдущем примере. И в отличие от первого вызова он обрабатывает запросы по пути
//  http://localhost:xxxx/pages, сопоставляя данные запросы с папкой wwwroot/html. К примеру, по запросу
//  http://localhost:xxxx/pages/index.html мы можем обратиться к файлу wwwroot/html/index.html.
#endregion

#region Метод UseFileServer
//  Метод UseFileServer() объединяет функциональность сразу всех трех вышеописанных методов UseStaticFiles, UseDefaultFiles
//  и UseDirectoryBrowser:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseFileServer();

//app.Run();

//  По умолчанию этот метод позволяет обрабатывать статические файлы и отправлять файлы по умолчанию типа index.html. Если
//  нам надо еще включить просмотр каталогов, то мы можем использовать перегрузку данного метода:
//      app.UseFileServer(enableDirectoryBrowsing: true);

//  Еще одна перегрузка метода позволяет более точно задать параметры:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseFileServer(new FileServerOptions()
//{
//    EnableDirectoryBrowsing = true,
//    EnableDefaultFiles = false
//});

//app.Run();

//  Также можно настроить сопоставление путей запроса с каталогами:

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

//  В этом случае будет разрешен обзор каталога по пути http://localhost:xxxx/pages/, но при этом путь
//  http://localhost:xxxx/html/ работать не будет.
#endregion