//  HttpResponse. Отправка ответа

//  Все данные запроса передаются в middleware через объект Microsoft.AspNetCore.Http.HttpContext. Этот объект инкапсулирует
//  информацию о запросе, позволяет управлять ответом и, кроме того, имеет еще много другой функциональности. Например,
//  возьмем простейшее приложение:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async context => await context.Response.WriteAsync("Hello Metanit",System.Text.Encoding.Default));
//app.Run();

//  Здесь параметр context, который передается в middleware в методе app.Run() как раз представляет объект HttpContext.
//  И через этот объект, точнее через его свойство Response мы можем отправить клиенту некоторый ответ:
//  context.Response.WriteAsync($"Hello METANIT.COM").

//  Свойство Response объекта HttpContext представляет объект HttpResponse и устанавливает, что будет отравляться в виде
//  ответа. Для установки различных аспектов ответа класс HttpResponse определяет следующие свойства:
//Body: получает или устанавливает тело ответа в виде объекта Stream
//BodyWriter: возвращает объект типа PipeWriter для записи ответа
//ContentLength: получает или устанавливает заголовок Content-Length
//ContentType: получает или устанавливает заголовок Content-Type
//Cookies: возвращает куки, отправляемые в ответе
//HasStarted: возвращает true, если отправка ответа уже началась
//Headers: возвращает заголовки ответа
//Host: получает или устанавливает заголовок Host
//HttpContext: возвращает объект HttpContext, связанный с данным объектом Response
//StatusCode: возвращает или устанавливает статусный код ответа

//  Чтобы отправить ответ, мы можем использовать ряд методов класса HttpResponse:
//Redirect(): выполняет переадресацию(временную или постоянную) на другой ресурс
//WriteAsJson()/ WriteAsJsonAsync(): отправляет ответ в виде объектов в формате JSON
//WriteAsync(): отправляет некоторое содержимое. Одна из версий метода позволяет указать кодировку.
//Если кодировка не указана, то по умолчанию применяется кодировка UTF-8
//SendFileAsync(): отправляет файл

//  Самый простой способ отправки ответа представляет метод WriteAsync(), в который передается отправляемые данные.
//  В качестве дополнительного параметра мы можем указать кодировку

#region Установка заголовков
//  Для установки заголовков применяется свойство Headers, которое представляет тип IHeaderDictionary. Для большинства
//  стандартных заголовков HTTP в этом интерфейсе определены одноименные свойства, например, для заголовка "content-type"
//  определено свойство ContentType. Другие, в том числе свои кастомные заголовки можно добавить через метод Append(). Например:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async (context) =>
//{
//    var response = context.Response;
//    response.Headers.ContentLanguage = "ru-Ru";
//    response.Headers.ContentType = "text/plain; charset=utf-8";
//    response.Headers.Append("secret-id", "256");
//    await response.WriteAsync("Hello Metanit");
//});
//app.Run();

//  Стоит отметить, что для вывода кириллицы желательно устанавливать заголовок ContentType, в том числе кодировку,
//  которая применяется в отправляемом содержимом (в примере выше это "text/plain; charset=utf-8").

//  Также стоит отметить, что вместо
//  response.Headers.ContentType = "text/plain; charset=utf-8";
//  можно было бы написать
//  response.ContentType = "text/plain; charset=utf-8";
#endregion

#region Установка кодов статуса
//  Для установки статусных кодов применяется свойство StatusCode, которому передается числовой код статуса:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();
//app.Run(async context =>
//{
//    context.Response.StatusCode = 404;
//    await context.Response.WriteAsync("Resourse not found");
//});
//app.Run();

//  В данном случае устанавливается код 404, который указывает, что ресурс не найден.
#endregion

#region Отправка html-кода
//  Если необходимо отправить html-код, то для этого необходимо установить для заголовка Content-Type значение text/html:

WebApplicationBuilder builder = WebApplication.CreateBuilder();
WebApplication app = builder.Build();

app.Run(async context =>
{
    HttpResponse? response = context.Response;
    response.ContentType = "text/html, charset=utf-8";
    await response.WriteAsync("<h2>Hello Metanit</h2><h3>Welcome to ASP.NET</h3>");
});
app.Run();
#endregion