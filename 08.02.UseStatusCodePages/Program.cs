//  Обработка ошибок HTTP

//  В отличие от исключений стандартный функционал проекта ASP.NET Core почти никак не обрабатывает ошибки HTTP, например, в
//  случае если ресурс не найден. При обращении к несуществующему ресурсу мы увидим в браузере пустую страницу, и только через
//  консоль веб-браузера мы сможем увидеть статусный код. Либо браузер может отобразить какую-то стандартную страницу.

//  Но с помощью компонента StatusCodePagesMiddleware можно добавить в проект отправку информации о статусном коде. Для этого
//  применяется метод app.UseStatusCodePages():

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//// обработка ошибок HTTP
//app.UseStatusCodePages();

//app.Map("/hello", () => "Hello ASP.NET Core");

//app.Run();

//  Здесь мы можем обращаться только по адресу "/hello". При обращении ко всем остальным адресам браузер отобразит базовую
//  информацию об ошибке:

//  Метод UseStatusCodePages() следует вызывать ближе к началу конвейера обработки запроса, в частности, до добавления middleware
//  для работы со статическими файлами и до добавления конечных точек.

#region Настройка сообщения
//  Сообщение, отправляемое методом UseStatusCodePages() по умолчанию, не очень информативное. Однако одна из версий метода
//  позволяет настроить отправляемое пользователю сообщение. В частности, мы можем изменить вызов метода так:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePages("text/plain","Error: Resource Not Found. Status code: {0}");

//app.Map("/hello", () => "Hello ASP.NET Core");

//app.Run();

//  В качестве первого параметра указывается MIME-тип ответа - в данном случае простой текст (""text/plain""). В качестве второго
//  параметра передается собственно то сообщение, которое увидит пользователь. В сообщение мы можем передать код ошибки через
//  плейсхолдер "{0}".
#endregion

#region Установка обработчика ошибок
//  Еще одна версия метода UseStatusCodePages() позволяет более детально задать обаботчик ошибок. В частности, она принимает делегат,
//  параметр которого - объект StatusCodeContext. В свою очередь, объект StatusCodeContext имеет свойство HttpContext, из которого
//  мы можем получить всю информацию о запросе и настроить отправку ответа. Например:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePages(async statusCodeContext => 
//{
//    var response = statusCodeContext.HttpContext.Response;
//    var path = statusCodeContext.HttpContext.Request.Path;

//    response.ContentType = "text/plain; charset=UTF-8";
//    if (response.StatusCode == 403)
//    {
//        await response.WriteAsync($"Path: {path}. Access Denied");
//    }
//    else if (response.StatusCode == 404)
//    {
//        await response.WriteAsync($"Resource {path} Not Found");
//    }
//});

//app.Map("/hello", () => "Hello ASP.NET Core");

//app.Run();
#endregion

#region Методы UseStatusCodePagesWithRedirects и UseStatusCodePagesWithReExecute
//  Вместо метода UseStatusCodePages() мы также можем использовать еще пару других, которые также обрабатывают ошибки HTTP.

//  С помощью метода app.UseStatusCodePagesWithRedirects() можно выполнить переадресацию на определенный метод, который
//  непосредственно обработает статусный код:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePagesWithRedirects("/error/{0}");

//app.Map("/hello", () => "Hello ASP.NET Core");
//app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

//app.Run();

//  Здесь будет идти перенаправление по адресу "/error/{0}". В качестве параметра через плейсхолдер "{0}" будет передаваться
//  статусный код ошибки.

//  Но теперь при обращении к несуществующему ресурсу клиент получит статусный код 302 / Found. То есть формально несуществующий
//  ресурс будет существовать, просто статусный код 302 будет указывать, что ресурс перемещен на другое место - по пути "/error/404".

//  Подобное поведение может быть неудобно, особенно с точки зрения поисковой индексации, и в этом случае мы можем применить
//  другой метод app.UseStatusCodePagesWithReExecute():

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.Map("/hello", () => "Hello ASP.NET Core");
app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

app.Run();

//  В качестве параметра метод UseStatusCodePagesWithReExecute() приминает путь к ресурсу, который будет обрабатывать ошибку. И
//  также с помощью плейсхолдера {0} можно передать статусный код ошибки. То есть в данном случае при возникновении ошибки будет
//  вызываться конечная точка
//      app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

//  Формально мы получим тот же ответ, так как так же будет идти перенаправление на путь "/error/404". Но теперь браузер получит
//  оригинальный статусный код 404.

//  Также можно задействовать другую версию метода, которая в качестве второго параметра принимает параметры строки запроса
//      app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

//  Первый параметр метода указывает на путь перенаправления, а второй задает параметры строки запроса, которые будут передаваться
//  при перенаправлении. Вместо плейсхолдера {0} опять же будет передаваться статусный код ошибки. Пример использования:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

//app.Map("/hello", () => "Hello ASP.NET Core");
//app.Map("/error", (string code) => $"Error Code: {code}");


//app.Run();
#endregion