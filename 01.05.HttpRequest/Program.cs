//  HttpRequest. Получение данных запроса

//  Свойство Request объекта HttpContext представляет объект HttpRequest и хранит информацию о запросе в виде следующих свойств:
//Body: предоставляет тело запроса в виде объекта Stream
//BodyReader: возвращает объект типа PipeReader для чтения тела запроса
//ContentLength: получает или устанавливает заголовок Content-Length
//ContentType: получает или устанавливает заголовок Content-Type
//Cookies: возвращает коллекцию куки (объект Cookies), ассоциированных с данным запросом
//Form: получает или устанавливает тело запроса в виде форм
//HasFormContentType: проверяет наличие заголовка Content-Type
//Headers: возвращает заголовки запроса
//Host: получает или устанавливает заголовок Host
//HttpContext: возвращает связанный с данным запросом объект HttpContext
//IsHttps: возвращает true, если применяется протокол https
//Method: получает или устанавливает метод HTTP
//Path: получает или устанавливает путь запроса в виде объекта RequestPath
//PathBase: получает или устанавливает базовый путь запроса. Такой путь не должен содержать завершающий слеш
//Protocol: получает или устанавливает протокол, например, HTTP
//Query: возвращает коллекцию параметров из строки запроса
//QueryString: получает или устанавливает строку запроса
//RouteValues: получает данные маршрута для текущего запроса
//Scheme: получает или устанавливает схему запроса HTTP

//  Рассмотрим применение некоторых из этих свойств.

#region Получение заголовков запроса
//  Для получения заголовков применяется свойство Headers, которое представляет тип IHeaderDictionary. Например, получим все
//  заголовки запроса и выведем их на веб-страницу:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    var stringBuilder = new System.Text.StringBuilder("<table>");
//    foreach (var header in context.Request.Headers)
//    {
//        stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
//    }
//    stringBuilder.Append("</table>");
//    await context.Response.WriteAsync(stringBuilder.ToString());

//});
//app.Run();

//  Для большинства стандартных заголовков HTTP в этом интерфейсе определены одноименные свойства, например, для заголовка
//  "content-type" определено свойство ContentType, а для заголовка "accept" - свойство Accept:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context => 
//{
//    var acceptHeaderValue = context.Request.Headers.Accept;
//    //var acceptHeaderValue = context.Request.Headers["accept"];
//    await context.Response.WriteAsync(acceptHeaderValue);
//});
//app.Run();

//  Также подобые заголовки, а также какие-то кастомные заголовки, для которых не определены подобные свойства, можно
//  получить как и любой дугой элемент словаря:
//  var acceptHeaderValue = context.Request.Headers["accept"];
//  Для ряда заголовков в классе HttpRequest определены отдельные свойства: Host, Method, ContentType, ContentLength.
#endregion

#region Получение пути запроса
//  Свойство path позволяет получить запрошенный путь, то есть адрес, к которому обращается клиент:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

////  !!Напоминание!! Для создания компонентов middleware используется делегат RequestDelegate, который выполняет некоторое
////  действие и принимает контекст запроса - объект HttpContext:
////  public delegate Task RequestDelegate(HttpContext context);
//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync($"Path: {context.Request.Path}");
//});
//app.Run();

//  Свойство path позволяет получить запрошенный путь, то есть адрес, к которому обращается клиент:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async (HttpContext context) =>
//{
//    var path = context.Request.Path;
//    var now = DateTime.Now;
//    var response = context.Response;

//    if (path == "/date")
//    {
//        await response.WriteAsync($"Date: {now.ToShortDateString()}");
//    }
//    else if (path == "/time")
//    {
//        await response.WriteAsync($"Time: {now.ToShortTimeString()}");
//    }
//    else
//    {
//        await response.WriteAsync("Hey =)");
//    }
//});
//app.Run();

//  В данном случае, если пользователь обращается по адресу "/date", то ему отображается текущая дата, а если
//  обращается по адресу "/time" - текущее время. В остальных случаях отображается некоторое универсальное сообщение:

//  Подобным образом можно определить свою систему маршрутизации, однако в ASP.NET Core по умолчанию есть инструменты,
//  которые проще использовать для создания системы маршрутизации в приложении и которые будут рассмотрены в последующих
//  статьях.
#endregion

#region Строка запроса
//  Свойство QueryString позволяет получить строку запроса. Строка запроса представляет ту часть запрошенного адреса,
//  которая идет после символа ? и представляет набор параметров, разделенных символом амперсанда &:
//  ? параметр1 = значение1 & параметр2 = значение2 & параметр3 = значение3
//  Каждому параметру с помощью знака равно передается некоторое значение.

//  Стоит отметить, что строка запроса (query string) НЕ входит в путь запроса (path):

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.WriteAsync($"<p>Path: {context.Request.Path}</p>" +
//        $"<p>QueryString: {context.Request.QueryString}</p>");
//});
//app.Run();

//  Обратимся по адресу https://localhost:7159/time?name=Tom&age=33&sex=male
//  Получим
//  Path: / time
//  QueryString: ? name = Tom & age = 33 & sex = male

//  Путь запроса или path представляет ту часть адреса, которая идет после домена/порта и до символа ?.
//  Строка запроса или query string представляет ту часть адреса, которая идет начиная с символа ?.
//  То есть в данном случае через строку запроса передаются три параметра: name = Tom,  age = 33,  sex = male

//  С помощью свойства Query можно получить все параметры строки запроса в виде словаря:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<h3>Параметры строки</h3><table>");
    foreach (var param in context.Request.Query)
    {
        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
    }
    stringBuilder.Append("</table>");
    await context.Response.WriteAsync(stringBuilder.ToString());
});
app.Run();

//  Соответственно можно вытащить из словаря Query значения отдельных параметров:
//app.Run(async context => 
//{
//    string name = context.Request.Query["name"];
//    string age = context.Request.Query["age"];
//    await context.Response.WriteAsync($"{name} - {age}");
//});
//app.Run();
#endregion

