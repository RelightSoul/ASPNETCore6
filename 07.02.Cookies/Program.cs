//  Куки

//  Куки представляют самый простой способ сохранить данные пользователя. Куки хранятся на компьютере пользователя и могут
//  устанавливаться как на сервере, так и на клиенте. Так как куки посылаются с каждым запросом на сервер, то их максимальный размер
//  ограничен 4096 байтами.

//  Для работы с куками также можно использовать контекст запроса HttpContext, который передается в качестве параметра в компоненты
//  middleware, а также доступен в контроллерах и RazorPages.

//  Чтобы получить куки, которые приходят вместе с запросом к приложению, нам надо использовать коллекцию Request.Cookies объекта
//  HttpContext. Эта коллекция представляет объект IRequestCookieCollection, в котором каждый элемент - это объект
//  KeyValuePair<string, string>, то есть некоторую пару ключ-значение.

//  Для этой коллекции определено несколько методов:
//      bool ContainsKey(string key): возвращает true, если в коллекции кук есть куки с ключом key
//      bool TryGetValue(string key, out string value): возвращает true, если удалось получить значение куки с ключом key в
//      переменную value

//  Стоит отметить, что куки - это строковые значения. Неважно, что вы пытаетесь сохранить в куки - все это необходимо приводить
//  к строке и соответственно получаете из кук вы тоже строки.

//  Например, получим куку "name":
//  if (context.Request.Cookies.ContainsKey("name"))
//     string name = context.Request.Cookies["name"];
//  Повторюсь, что коллекция context.Request.Cookies служит только для получения значений кук.

//  Для установки кук, которые отправляются в ответ клиенту, применяется объект context.Response.Cookies, который представляет
//  интерфейс IResponseCookies. Этот интерфейс определяет два метода:
//      Append(string key, string value): добавляет для куки с ключом key значение value
//      Delete(string key): удаляет куку по ключу

//  Например, объединим в приложении установку и получение кук:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    if (context.Request.Cookies.ContainsKey("name"))
    {
        await context.Response.WriteAsync($"Hello {context.Request.Cookies["name"]}");
    }
    else
    {
        context.Response.Cookies.Append("name", "Vlad");
        await context.Response.WriteAsync("Hello man");
    }
});

app.Run();

//  При получении запроса мы смотрим, установлена ли кука "name":
//  if (context.Request.Cookies.ContainsKey("name"))

//  Если кука не установлена(например, при первом обращении к приложению), то устанавливаем ее и отправляем пользователю в ответ
//  строку "Hello World!".
//  context.Response.Cookies.Append("name", "Tom");
//  await context.Response.WriteAsync("Hello World!");

//  Если кука установлена, то получаем ее значение и отправляем его пользователю
//  await context.Response.WriteAsync($"Hello {context.Request.Cookies["name"]}");

//  После установки кук в результате первого запроса в браузере будет установлена кука name. И через средства разработчика в
//  веб-браузере мы сможем увидеть ее значение (если браузер поддерживает подобное). Например, в Google Chrome посмотреть куки
//  можно на вкладке Application -> Cookies

//  В примере выше мы также могли бы применить метод TryGetValue() для получения кук:

app.Run(async (context) =>
{
    if (context.Request.Cookies.TryGetValue("name", out var login))
    {
        await context.Response.WriteAsync($"Hello {login}!");
    }
    else
    {
        context.Response.Cookies.Append("name", "Tom");
        await context.Response.WriteAsync("Hello World!");
    }
});

//  Стоит отметить, что пользователь через подобные инструменты (если веб-браузер позволяет) может вручную поменять значение кук,
//  либо вовсе удалить их.