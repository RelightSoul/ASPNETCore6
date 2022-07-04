//  Переадресация

//  Для выполнения переадресации у объекта HttpResponse определен метод Redirect():
//      void Redirect(string location)
//      void Redirect(string location, bool permanent)

//  Первая версия выполняет временную переадресацию. В качестве параметра получает адрес для редиректа, а клиенту
//  посылается статусный код 302.

//  Вторая версия метода также в качестве второго параметра получает булевое значение, которое указывает, будет ли
//  переадресация постоянной. Если этот параметр равен true, то переадресация будет постоянной, и в этом случае
//  посылается статусный код 301. Если равен false, то переадресация временная, и посылается статусный код 302.

//  Допустим, у нас было следующее приложение

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

//  При обращении по адресу "/old" приложение посылает сообщение "Old Page".

//  Но затем мы решили сделать переадресацию с адреса "/old" на "/new". Используем для этого первую версию метода Redirect:

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
//  Теперь при обращении по адресу "/old" произойдет перенаправление на адрес "/new".

//  В данном случае применяется редирект на локальный адрес в рамках приложение.
//  Но также можно использовать редирект на внешние ресурсы:

//  if (context.Request.Path == "/old")
//{
//    context.Response.Redirect("https://www.google.com/search?q=metanit.com");
//}