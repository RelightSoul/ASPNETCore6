//  Маршрутизация
//  Конечные точки. Метод Map

//  Система маршрутизации отвечает за сопоставление входящих запросов с маршрутами и на основании результатов сопоставления выбирает для
//  обработки запроса определенную конечную точку приложения. Конечная точка или endpoint представляет некоторый код, который обрабатывает
//  запрос. По сути конечная точка объединяет шаблон маршрута, которому должен соответствовать запрос, и обработчик запроса по этому
//  маршруту.

//  ASP.NET Core по умолчанию предоставляет простой и удобный функционал для создания конечных точек. Ключевым типом в этом функционале
//  является интерфейс Microsoft.AspNetCore.Routing.IEndpointRouteBuilder. Он определяет ряд методов для добавления конечных точек в
//  приложение. И поскольку класс WebApplication также реализует данный интерфейс, то соответственно все методы интерфейса мы можем
//  вызывать и у объекта WebApplication.

//  Для использования системы маршрутизации в конвейер обработки запроса добавляются два встроенных компонента middleware:

//  Microsoft.AspNetCore.Routing.EndpointMiddleware добавляет в конвейер обработки запроса конечные точки. Добавляется в конвейер с
//  помощью метода UseEndpoints()

//  Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware добавляет в конвейер обработки запроса функциональность сопоставления запросов
//  и маршрутов. Данный middleware выбирает конечную точку, которая соответствует запросу и которая затем обрабатывает запрос. Добавляется
//  в конвейер с помощью метода UseRouting()

//  Причем обычно не требуется явным образом подключать эти два компонента middleware. Объект WebApplicationBuilder автоматически
//  сконфигурирует конвейер таким образом, что эти два middleware добавляются при использовании конечных точек.

#region Метод Map
//  Самым простым способом определения конечной точки в приложении является метод Map, который реализован как метод расширения для типа
//  IEndpointRouteBuilder. Он добавляет конечные точки для обработки запросов типа GET. Данный метод имеет три версии:
//  public static RouteHandlerBuilder Map(this IEndpointRouteBuilder endpoints, RoutePattern pattern, Delegate handler);
//  public static IEndpointConventionBuilder Map(this IEndpointRouteBuilder endpoints, string pattern, RequestDelegate requestDelegate);
//  public static RouteHandlerBuilder Map(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler);

//  В всех трех реализациях этот метод в качестве параметра pattern принимает шаблон маршрута, которому должен соответствовать запрос.
//  Данный параметр может представлять тип RoutePattern или string.

//  Последний параметр представляет действие, которое будет обрабатывать запрос. Это может быть делегат типа RequestDelegate, либо делегат
//  Delegate.

//  Стоит отметить, что не стоит путать этот метод с одноименным методом Map(), который реализован как метод расширения для типа
//  IApplicationBuilder

//app.Map("/time", appBuilder =>                            // не путать с этим методом для IApplicationBuilder
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

//  Например, определим следующее приложение:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/",() => "Index Page");
//app.Map("/about", () => "About Page");
//app.Map("/contact", () => "Contacts Page");

//app.Run();

//  Здесь определено три конечных точки с помощью трех методов app.Map(). Первый вызов добавляет конечную точку, которая будет обрабатывать
//  запрос по пути "/". В качестве обработчика выступает действие
//      () => "Index Page"

//  Результат этого действия - строка "Index Page" - это то, что будет отправляться в ответ клиенту.

//  Аналогично второй и третий вызовы метода Map добавляют конечные точки для обработки запросов по путям "/about" и "/contact":

//  Выше в примере обрабочики маршрутов возвращали строки, но в принципе это может быть любое значение, например:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => "Index Page");
//app.Map("/about", () => "About Page");
//app.Map("/contact", () => "Contacts Page");
//app.Map("/user", () => new Person ("Tom", 33));

//app.Run();

//  Здесь обрабочик запроса второй конечной точки возвращает в ответ объект Person. По умолчанию подобные объекты при отправке
//  сериализируются в JSON

//  В принципе можно ничего из обработчика не возвращать, посто выполнять некоторые действия
//      app.Map("/user", ()=>Console.WriteLine("Request Path: /user"));
//  В данном случае в обработчике второй конечной точки просто логгируем на консоль некоторую информацию.

//  При необходимости обработчик маршрута можно вынести в полноценный метод:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => "Index Page");
//app.Map("/user", () => GetPerson);
//app.Map("/contact", async context =>   // принимает делегат
//{
//    await context.Response.WriteAsync("Contacts Page");
//});

//app.Run();

//Person GetPerson()
//{
//    return new Person("Tom", 33);
//}

//  В примерах выше обработчик запроса представлял делегат Delegate. Если же необходимо получить полный доступ к контексту HttpContext,
//  то можно использовать другую версию метода, которая принимает делегат RequestDelegate:

//  app.Map("/about", async (context) =>
//  {
//        await context.Response.WriteAsync("About Page");
//  });
#endregion

#region Получение всех маршрутов приложения
//  ASP.NET Core позволяет легко получить все имеющиеся в приложении конечные точки. Так, определим следующий код:

using System.Text;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/",() => "Index Page");
app.Map("/user",() => "User Page");
app.Map("/about", () => "About Page");

app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
{
    string.Join("\n", endpointSources.SelectMany(source => source.Endpoints));
});

app.Run();

//  Здесь определены четыре конечных точки. Три первых конечных точки стандартные. Поэтому рассмотрим четвертую конечную точку, которая
//  обрабатывает запросы по маршруту "/routes" и которая и будет выводить список всех конечных точек.

//  Через механизм внедрения зависимостей в обработчик маршрута четвертой конечной точки передается объект IEnumerable<EndpointDataSource>
//  - некоторый набор данных о конечных точках. Каждый отдельный элемент этого набора - объект EndpointDataSource, который хранит набор
//  конечных точек в свойстве-списке Endpoints. Каждая конечная точка в этом списке представляет класс Endpoint

//  С помощью метода endpointSources.SelectMany() выбираем из коллекции Endpoints все конечные точки. С помощью метода Join() они
//  склеиваются в одну строку и разделяются переводом строки \n.

//  В итоге мы увидим в браузере список из четырех конечных точек

//  При необходимости можно получить более детальную и подробную информацию по каждой конечной точке

//app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
//{
//    var sb = new StringBuilder();
//    var endpoints = endpointSources.SelectMany(es => es.Endpoints);
//    foreach (var endpoint in endpoints)
//    {
//        sb.AppendLine(endpoint.DisplayName);

//        // получим конечную точку как RouteEndpoint
//        if (endpoint is RouteEndpoint routeEndpoint)
//        {
//            sb.AppendLine(routeEndpoint.RoutePattern.RawText);
//        }

//        // получение метаданных
//        // данные маршрутизации
//        // var routeNameMetadata = endpoint.Metadata.OfType<Microsoft.AspNetCore.Routing.RouteNameMetadata>().FirstOrDefault();
//        // var routeName = routeNameMetadata?.RouteName;
//        // данные http - поддерживаемые типы запросов
//        //var httpMethodsMetadata = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault();
//        //var httpMethods = httpMethodsMetadata?.HttpMethods; // [GET, POST, ...]
//    }
//    return sb.ToString();
//});
#endregion

#region Конец кода
record class Person(string Name, int Age);
#endregion