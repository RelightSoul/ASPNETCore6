//  Сочетание конечных точек с другими middleware

//  Кроме конечных точек запрос в конвейере обработки могут обрабатывать и другие компоненты middleware. При этом надо учитывать
//  общий процесс обработки запроса и вызова конечных точек.

//  Так, если приложение содержит конечные точки, то система маршрутизации на основе процессе URL matching или сопоставления адреса
//  URL с шаблонами маршрута выбирает для обработки определенную конечную точку. Если в приложении есть такая конечная точка, которая
//  соответствует запросу, то компонент middleware Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware устанавливает у объекта
//  HttpContext конечную точку для будущей обработки запроса, которую можно получить с помощью метода HttpContext.GetEndpoint().
//  Кроме того, устанавливаются значения маршрута, которые можно получить через коллекцию HttpRequest.RouteValues

//  Однако конечная точка начинает обрабатывать запрос только после того, как все middleware в конвейере начнут обработку запроса.
//  Например, возьмем следующий код приложения:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) => 
//{
//    Console.WriteLine("First Middleware starts");
//    await next.Invoke();
//    Console.WriteLine("First Middleware ends");
//});
//app.Map("/", () => 
//{
//    Console.WriteLine("Index endpoint start and ends");
//    return "Index Page";
//});
//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Second middleware starts");
//    await next.Invoke();
//    Console.WriteLine("Second middleware ends");
//});
//app.Map("/about", () =>
//{
//    Console.WriteLine("About endpoint start and ends");
//    return "About Page";
//});

//app.Run();

//  Здесь до и после первой конечной точки с помощью метода app.Use() в конвейер встроены два middleware. Для получения общей картины
//  выполнения приложения процесс выполнения логгируется на консоль приложения.

//  Если мы запустим приложение, то при запросе по адресу "/" ожидаемо для обработки запроса будет выбрана первая конечная точка

//  Но теперь взглянем на консоль приложения:
//First Middleware starts
//Second middleware starts
//Index endpoint start and ends
//Second middleware ends
//First Middleware ends

//  Мы видим, что конечная точка выполняется после того, как начнет выполняться компонент middleware, который в коде идет после
//  добавления этой конечной точки.

//  При этом в компонентах middleware также можно обрабатывать запросы по определенным адресам:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) => 
//{
//    if (context.Request.Path == "/date")
//    {
//        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
//    }
//    else
//    {
//        await next.Invoke();
//    }
//});

//app.Map("/", () => "Index Page");
//app.Map("/about", () => "About Page");

//app.Run();

//  Кроме того, middleware могут быть полезны для некоторого постдействия - выполнения некоторых действий, когда конечная точка уже
//  выбрана. Или, наоборот, если ни одна из конечных точек не обработала запрос, и в middleware мы можем обработать эту ситуацию:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    await next.Invoke();
//    if (context.Response.StatusCode == 404)
//    {
//        await context.Response.WriteAsync("Resource not found");
//    }
//});

//app.Map("/", () => "Index Page");
//app.Map("/about", () => "About Page");

//app.Run();

//  Однако если в конце конвейера располагается терминальный компонент, то он будет выполняться даже если конечная точка
//  соответствует запрошенному пути:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/", () => "Index Page");
app.Map("/about", () => "About Page");

app.Run(async context =>
{
    context.Response.StatusCode = 404;
    await context.Response.WriteAsync("Resource not found");
});

app.Run();

//  В данном случае по результату программы мы видим, что даже при запросах по адресу "/" и "/about" будет выполняться
//  middleware из метода app.Run

//  Почему такое происходит? Опять же потому что, сначала выполняются все middleware в конвейере, и только потом выполняется
//  конечная точка.