//  Метод Map

//  Метод Map() применяется для создания ветки конвейера, которая будет обрабатывать запрос по определенному пути. Этот метод
//  реализован как метод расширения для типа IApplicationBuilder и имеет ряд перегруженных версий. Например:

//  public static IApplicationBuilder Map (this IApplicationBuilder app, string pathMatch, Action<IApplicationBuilder> configuration);

//  В качестве параметра pathMatch метод принимает путь запроса, с которым будет сопоставляться ветка. А параметр configuration
//  представляет делегат, в который передается объект IApplicationBuilder и в котором будет создаваться ветка конвейера.

//  Рассмотрим простой пример:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Map("/time", appBuilder =>
//{
//    var time = DateTime.Now.ToShortTimeString();

//    // логгируем данные - выводим на консоль приложения
//    appBuilder.Use(async (context, next) =>
//    {
//        Console.WriteLine($"Time: {time}");
//        await next();  // вызываем следующий middleware
//    });

//    appBuilder.Run(async context =>
//    {
//        await context.Response.WriteAsync($"Time: {time}");
//    });
//});

//app.Run(async (context) => await context.Response.WriteAsync("Hello"));

//app.Run();

//  В данном случае метод app.Map() создает ответвление конвейера, которое будет обрабатывать запросы по пути "/time":

//appBuilder =>
//{
//    var time = DateTime.Now.ToShortTimeString();
//    // логгируем данные - выводим на консоль приложения
//    appBuilder.Use(async (context, next) =>
//    {
//        Console.WriteLine($"Time: {time}");
//        await next();   // вызываем следующий middleware
//    });

//    appBuilder.Run(async context => await context.Response.WriteAsync($"Time: {time}"));
//}

//  Созданная ветка конвейера содержит два middleware, встраиваемые с помощью методов Use() и Run(). Вначале получаем текущее
//  время и в первом middleware логгируем это время на консоль. Во втором - терминальном компоненте middleware отправляем
//  информацию о времени в ответ клиенту.

//  При других путях запросах, отличных от "/time", запрос будет обрабатываться основным потоком конвейера, который состоит в
//  данном случае из одного компонента:
//  app.Run(async (context) => await context.Response.WriteAsync("Hello"));

//  Подобным образом можно создавать ветки для разных путей:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

//app.Map("/index", appBuild =>
//{
//    appBuild.Run(async context =>
//    {
//        await context.Response.WriteAsync("Index page");
//    });
//});

//app.Map("/about", appBuild =>
//{
//    appBuild.Run(async context =>
//    {
//        await context.Response.WriteAsync("About Page");
//    });
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Page not found");
//});

//app.Run();

//  При необходимости создание веток конвейера можно вынести в отдельные методы:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/index", Index);
//app.Map("/about", About);

//app.Run(async (context) => await context.Response.WriteAsync("Page Not Found"));

//app.Run();

void Index(IApplicationBuilder appBuild)
{
    appBuild.Run(async context =>
    {
        await context.Response.WriteAsync("Index Page");
    });
}

void About(IApplicationBuilder appBluid)
{
    appBluid.Run(async context =>
    {
        await context.Response.WriteAsync("About Page");
    });
}

#region Вложенные методы Map
//  Ветка конвейера, которая создается в методе Map(), может иметь вложенные ветки, которые обрабатывают подзапросы. Например:

app.Map("/home", appBuilder =>
{
    appBuilder.Map("/index",Index);
    appBuilder.Map("/about", About);
    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Home Page");
    });
});

app.Run(async (context) => await context.Response.WriteAsync("Page Not Found"));

app.Run();

//  Здесь ветка создается с помощью вызова
//  app.Map("/home", appBuilder =>

//  Эта ветка будет обрабатывать запросы по пути "/home".

//  Внутри этой ветки создаются две вложенные ветки, которые будут обрабатывать запросы по путям относительно пути основной ветки.
//  То есть теперь метод About будет обрабатывать запрос по пути "/home/about", а не "/about"
#endregion