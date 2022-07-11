//  Состояние приложения.Куки.Сессии
//  HttpContext.Items

//  Приложение ASP.NET Core может хранить некоторое состояние.Это могут быть как какие-то глобальные данные, так и данные,
//  которые непосредственно относятся к запросу и пользователю.И в зависимости от вида данных, существуют различные способы
//  для их хранения.

//  Один из простейших способов хранения данных представляет коллекция HttpContext.Items - объект типа IDictionary<object, object>.
//  Эта коллекция предназначена для таких данных, которые непосредственно связаны с текущим запросом. После завершения запроса все
//  данные из HttpContext.Items удаляются. Каждый объект в этой коллекции имеет ключ и значение.И с помощью ключей можно управлять
//  объектами коллекции.

//  В каком случае мы можем применить данную коллекцию? Например, если у нас обработка запроса вовлекает множество компонентов
//  middleware, и мы хотим, чтобы для этих компонентов были доступны общие данные, то как раз можем применить эту коллекцию. Например,
//  пусть в приложении определен следующий код:   

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    context.Items["text"] = "Hello from HttpContext.Items";
//    await next.Invoke();
//});

//app.Run(async context => 
//{
//    await context.Response.WriteAsync($"Text: {context.Items["text"]}");
//});

//app.Run();

//  Здесь в одном middleware опредляется ключ "text" со значением "Hello from HttpContext.Items":
//      context.Items["text"] = "Hello from HttpContext.Items";
//  В другом middleware этот объект используется для установки отправляемого ответа.

//  HttpContext.Items предоставляет ряд методов для управления элементами:
//      void Add(object key, object value): добавляет объект value с ключом key
//      void Clear(): удаляет все объекты
//      bool ContainsKey(object key): возвращает true, если словарь содержит объект с ключом key
//      bool Remove(object key): удаляет объект с ключом key, в случае удачного удаления возвращает true
//      bool TryGetValue(object key, out object value): возвращает true, если значение объекта с ключом key успешно получено в
//      объект value

//  Применение некоторых методов:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Items.Add("ninja", "(-_-) Ninja");
    await next.Invoke();
});

app.Run(async (context) => 
{
    if (context.Items.ContainsKey("ninja"))
    {
        await context.Response.WriteAsync($"Message: {context.Items["ninja"]}");
    }
    else
    {
        await context.Response.WriteAsync("Huhuhu, all ninja dead");
    }
});

app.Run();