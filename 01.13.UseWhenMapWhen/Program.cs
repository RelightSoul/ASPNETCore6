//  Создание ветки конвейера. UseWhen и MapWhen

#region UseWhen
//  Метод UseWhen() на основании некоторого условия позволяет создать ответвление конвейера при обработке запроса:

//  public static IApplicationBuilder UseWhen (this IApplicationBuilder app, Func<HttpContext,bool> predicate,
//                                              Action<IApplicationBuilder> configuration);

//  Как и Use(), метод UseWhen() реализован как метод расширения для типа IApplicationBuilder.

//  В качестве параметра он принимает делегат Func>HttpContext,bool> - некоторое условие, которому должен соответствовать запрос.
//  В этот делегат передается объект HttpContext. А возвращаемым типом должен быть тип bool - если запрос соответствует условию,
//  то возвращается true, иначе возвращаеся false.

//  Последний параметр метода - делегат Action<IApplicationBuilder> представляет некоторые действия над объектом IApplicationBuilder,
//  который передается в делегат в качестве параметра.

//  Рассмотрим небольшой пример:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(context => context.Request.Path == "/time", // если путь запроса "/time"
//    appBuilder =>
//    {
//        // логгируем данные - выводим на консоль приложения
//        appBuilder.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"Time: {time}");
//            await next();
//        });
//        appBuilder.Run(async context => 
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await context.Response.WriteAsync($"Time: {time}");
//        });
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello man!=)");
//});

//app.Run();

//  В данном случае метод app.UseWhen() в качестве первого параметра получает следующее условие:
//  context => context.Request.Path == "/time"

//  Второй параметр определяет действие, в котором создается ответвление конвейера:
//
//  appBuilder =>
//{
//    // логгируем данные - выводим на консоль приложения
//    appBuilder.Use(async (context, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        Console.WriteLine($"Time: {time}");
//        await next();   // вызываем следующий middleware
//    });

//    appBuilder.Run(async context =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        await context.Response.WriteAsync($"Time: {time}");
//    });
//}

//  В данном действии в конвейер обработки запроса встраиваются два middleware - с помощью методов Use() и Run(). В первом middleware
//  логгируем это время на консоль приложения. Во втором - терминальном компоненте middleware отправляем информацию о времени в
//  ответ клиенту.

//  Если мы обращаемся к приложению по пути, который отличается от "/time", то условие в методе UseWhen() ложно, поэтому ответвления
//  конвейера не выполняется. И выполняется middleware из метода app.Run().   // Hello man!=)
//  Однако если мы обращаемся по пути "/time", то условие в методе app.UseWhen() будет истинно. Соответственно будет выполняться
//  ответвление конвейера, который будет обрабатывать запрос. В итоге на консоль приложения, а также в браузере будет выводиться
//  текущее время.

//  Стоит отметить, что создание ветки происходит один раз при запуске приложения. Например, в примере выше мы видим, что получение
//  времени производится в обоих middleware во встраиваемой ветки. Но что будет, если вынести получение времени во вне и не дублировать
//  в каждом middleware, например, следующим образом:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(
//    context => context.Request.Path == "/time", // если путь запроса "/time"
//    appBuilder =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        // логгируем данные - выводим на консоль приложения
//        appBuilder.Use(async (context, next) =>
//        {
//            Console.WriteLine($"Time: {time}");
//            await next();   // вызываем следующий middleware
//        });

//        // отправляем ответ
//        appBuilder.Run(async context =>
//        {
//            await context.Response.WriteAsync($"Time: {time}");
//        });
//    });

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello METANIT.COM");
//});

//app.Run();

//  В этом случае время будет устанавливаться один раз - при запуске приложения и создании ветки в конвейер. Соответственно вне
//  зависимости от того, сколько раз мы будем обращаться к приложению по пути "/time", мы будем получать одно и то же время.

//  В примере выше ветка конвейера завершалась терминальным компонентом, поэтому остальные действия в основной части конвейера не
//  выполнялись. Однако мы можем также передать запрос на обработку из ветки в основной поток конвейера:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(
//    context => context.Request.Path == "/time",
//    appBuilder =>
//    {
//        appBuilder.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"Time: {time}");
//            await next();
//        });
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hey =)");
//});
//app.Run();

//  В данном случае, если запрос идет по пути "/time", сначала срабатывает ветка конвейера с компонентом, который логгирует время
//  на консоль. А затем выполняется компоненте из app.Run(), который отправляет сообщение "Hey =)"

//  Для большей читабельности также можно было бы вынести действия по созданию ветки конвейера в отдельный метод:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(
//    context => context.Request.Path == "/time",
//    HandleTimeRequest);

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hey =)");
//});
//app.Run();

//void HandleTimeRequest(IApplicationBuilder appBuilder)
//{
//    appBuilder.Use(async (context, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        Console.WriteLine($"Current time: {time}");
//        await next();
//    });
//}
#endregion

#region MapWhen
//  Метод MapWhen(), как и метод UseWhen(), на основании некоторого условия позволяет создать ответвление конвейера:

//  public static IApplicationBuilder MapWhen(this IApplicationBuilder app, Func<HttpContext, bool> predicate,
//                                              Action<IApplicationBuilder> configuration);

//  Метод MapWhen() также реализован как метод расширения для типа IApplicationBuilder, принимает те же параметры, что и UseWhen(),
//  и работает во многом аналогичным образом:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.MapWhen(
    context => context.Request.Path == "/time",   // условие: если путь запроса "/time"
    appBuilder => appBuilder.Run(async context =>
    {
        var time = DateTime.Now.ToShortTimeString();
        await context.Response.WriteAsync($"Current time: {time}");
    })
);

app.Run(async context =>
{
    await context.Response.WriteAsync("Hey all");
});

app.Run();

//  Здесь опять же, если запрошен путь "/time", то срабатывает ветка конвейера, созданная методом app.MapWhen(), в которой клиенту
//  отправляется текущее время. Если путь запроса другой, то срабатывается основной поток конвейера, в котором отправляется
//  сообщение  Hey all
#endregion