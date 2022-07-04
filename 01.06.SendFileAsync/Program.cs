//  Отправка файлов

//  Для отправки файлов применяется метод SendFileAsync(), который получает либо путь к файлу в виде строки,
//  либо информацию о файле в виде объекта IFileInfo. Например, допустим нам надо отправить файл по адресу "D:\\forest.jpg":

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    await context.Response.SendFileAsync("forest.jpg");
//});
//app.Run();

//  По умолчанию браузер попытается открыть файл. Так, в случае с изображениями они отображаются в браузере.

//  Также мы можем использовать относительные пути. Например, добавим в проект какой-нибудь файл
//  (в моем случае это файл forest.jpg). Для этого файла в окне свойств установим для опции Copy to Output Directory
//  значение Copy if newer или Copy always, чтобы файл автоматически копировался в выходной каталог при построении
//  приложения. И установим относительный путь относительно корня приложения "forest.jpg".

#region Отправка html-страницы
//  Подобным образом мы можно отправлять и другие типы файлов, например, html-страницу. Так, определим в проекте новую
//  папку, которую назовем html. В эту папку добавим новый файл index.html:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context => 
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.SendFileAsync("html/index.html");
//});
//app.Run();

//  Теперь немного усложним задачу. Добавим в проект в папку html еще пару файлов. Назовем их, к примеру, about.html
//  и contact.html. Для отправки этих файлов определим следующий код:

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

//  Когда приходит запрос, мы сопоставляем путь запроса (path) с файлами в папке html. То есть если path = about.html,
//  то нам надо оправить в ответ файл about.html. При этом проверяем наличие файла. Если он есть в папке, то отправляем
//  данный файл. Если нет, то отправляем статусный код 404 и сообщение, что ресус не найден

//  Стоит отметить, что в ASP.NET Core уже имеется встроенный middleware, который позволяет упростить работу со
//  статическими файлами.
#endregion

#region Загрузка файла
//  По умолчанию браузер пытается открыть отправляемый файл, что может быть полезно в случае файлов html - мы можем
//  определить файл html и таким образом отправить клиенту веб-страницу. Но также может быть необходимо, чтобы браузер
//  загружал файл без его открытия. В этом случае мы можем установить для заголовка "Content-Disposition" значение
//  "attachment":

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    context.Response.Headers.ContentDisposition = "attachment;filename=my_forest.jpg";
//    await context.Response.SendFileAsync("forest.jpg");
//});
//app.Run();

//  В этом случае загруженный файл получит имя "my_forest.jpg"
#endregion

#region IFileInfo
//  В примерах выше применялась версия метода SendFileAsync(), которая получает путь к файлу в виде строки. Также
//  можно использовать другую версию, которая получает информацию о файле в виде объекта IFileInfo:

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

//  В этом случае сначала необходимо определить объект PhysicalFileProvider, конструктор которого получает
//  каталог для поиска файлов. В его метод fileProvider.GetFileInfo() передается путь к файлу в рамках этого каталога.
//  А результатом метода является объект IFileInfo, который передается в SendFileAsync()
#endregion