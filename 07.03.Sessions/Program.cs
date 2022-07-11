//  Сессии

//  Сессия представляет собой ряд последовательных запросов, совершенных в одном браузере в течение некоторого времени. Сессия может
//  использоваться для сохранения каких-то временных данных, которые должны быть доступны, пока пользователь работает с приложением,
//  и не требуют постоянного хранения.

//  Для хранения состояния сессии на сервере создается словарь или хеш-таблица, которая хранится в кэше и которая существует для всех
//  запросов из одного браузера в течение некоторого времени. На клиенте хранится идентификатор сессии в куках. Этот идентификатор
//  посылается на сервер с каждым запросом. Сервер использует этот идентификатор для извлечения нужных данных из сессии. Эти куки
//  удаляются только при завершении сессии. Но если сервер получает куки, которые установлены уже для истекшей сессии, то для этих
//  кук создается новая сессия.

//  Сервер хранит данные сессии в течение ограниченного промежутка времени после последнего запроса. По умолчанию этот промежуток
//  равен 20 минутам, хотя его также можно изменить.

//  Следует учитывать, что данные сессии специфичны для одного браузера и не разделяются между браузерами. То есть для каждого
//  браузера на одном компьютере будет создаваться свой набор данных.

//  Все сессии работают поверх объекта IDistributedCache, и ASP.NET Core предоставляет встроенную реализацию IDistributedCache,
//  которую мы можем использовать. Для этого определим в приложении следующий код:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
//builder.Services.AddSession(); // добавляем сервисы сессии

//var app = builder.Build();

//app.UseSession();  // добавляем middleware для работы с сессиями

//app.Run(async context => 
//{
//    if (context.Session.Keys.Contains("name"))
//    {
//        await context.Response.WriteAsync($"Hello {context.Session.GetString("name")}");
//    }
//    else
//    {
//        context.Session.SetString("name", "Tom");
//        await context.Response.WriteAsync("Hello World!");
//    }
//});

//app.Run();

//  Сначала добавляем необходимые сервисы:
//      builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
//      builder.Services.AddSession();  // добавляем сервисы сессии

//  Затем с помощью метода UseSession() встраиваем в конвейер обработки запроса middleware для работы с сессиями:

//app.UseSession();   // добавляем middleware для работы с сессиями

//app.Run(async (context) =>
//{
//    if (context.Session.Keys.Contains("name"))
//        await context.Response.WriteAsync($"Hello {context.Session.GetString("name")}!");
//    else
//    {
//        context.Session.SetString("name", "Tom");
//        await context.Response.WriteAsync("Hello World!");
//    }
//});

//  Если мы вдруг не используем app.UseSession() или попробуем обратиться к сессии до применения этого метода, то получим
//  исключение InvalidOperationException.

//  После вызова app.UseSession() мы сможем управлять сессиями через свойство HttpContext.Session, которое представляет объект
//  интерфейса ISession. В данном случае мы проверяем, определен ли в сессии ключ "name". Если ключ определен, то передаем ответ
//  значение по этому ключу. Если ключ не определен, устанавливаем его.

#region Управление сессией
//  Объект ISession определяет ряд свойств и методов, которые мы можем использовать:
//      Keys: свойство, представляющее список строк, который хранит все доступные ключи
//      Clear(): очищает сессию
//      Get(string key): получает по ключу key значение, которое представляет массив байтов
//      GetInt32(string key): получает по ключу key значение, которое представляет целочисленное значение
//      GetString(string key): получает по ключу key значение, которое представляет строку
//      Set(string key, byte[] value): устанавливает по ключу key значение, которое представляет массив байтов
//      SetInt32(string key, int value): устанавливает по ключу key значение, которое представляет целочисленное значение value
//      SetString(string key, string value): устанавливает по ключу key значение, которое представляет строку value
//      Remove(string key): удаляет значение по ключу
#endregion

#region Настройка сессии
//  Для разграничения сессий для них устанавливается идентификатор. Каждая сессия имеет свой идентификатор, который сохраняется в куках.
//  По умолчанию эти куки имеют название ".AspNet.Session". И также по умолчанию куки имеют настройку CookieHttpOnly=true, поэтому
//  они не доступны для клиентских скриптов из браузера. Но мы можем переопределить ряд настроек сессии с помощью свойств объекта
//  SessionOptions:
//      Cookie.Name: имя куки
//      Cookie.Domain: домен, для которого устаналиваются куки
//      Cookie.HttpOnly: доступны ли куки только при передаче через HTTP-запрос
//      Cookie.Path: путь, который используется куками
//      Cookie.Expiration: время действия куки в виде объекта System.TimeSpan
//      Cookie.IsEssential: при значении true указывает, что куки критичны и необходимы для работы этого приложения
//      IdleTimeout: время действия сессии в виде объекта System.TimeSpan прм неактивности пользователя. При каждом новом запросе
//      таймаут сбрасывается. Этот параметр не зависит от Cookie.Expiration.

//Для использования этих свойств изменим установку сервисов:

//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = ".Myapp.Session";
//    options.IdleTimeout = TimeSpan.FromSeconds(60);
//    options.Cookie.IsEssential = true;
//});
#endregion

#region Хранение сложных объектов
//  В случае выше в сессиях хранились простые строки. Если же надо сохранить какой-то сложный объект, то его надо сериализовать в
//  строку, а при получении из сессии - обратно десериализовать. Как правило, для этого определяются методы расширения для объекта
//  ISession. В частности, добавим следующий класс:

using System.Text.Json;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize<T>(value));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
    }
}
//  Метод Set сохраняет в сессию данные, а метод Get извлекает их из сессии.

//  Допустим, у нас есть класс Person:
class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

//  В приложении выполним сериализацию и десериализовацию объекта в сессию:

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();

        var app = builder.Build();

        app.UseSession();

        app.Run(async (context) =>
        {
            if (context.Session.Keys.Contains("person"))
            {
                Person? person = context.Session.Get<Person>("person");
                await context.Response.WriteAsync($"Hello {person?.Name}, your age: {person?.Age}!");
            }
            else
            {
                Person person = new Person { Name = "Tom", Age = 22 };
                context.Session.Set<Person>("person", person);
                await context.Response.WriteAsync("Hello World!");
            }
        });

        app.Run();
    }
}
#endregion