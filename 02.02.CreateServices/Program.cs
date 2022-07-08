//  Создание сервисов

//  Фреймворк ASP.NET Core предоставляет ряд встроенных сервисов, которые мы можем использовать. Но также мы можем создавать свои
//  собственные сервисы. Рассмотрим, как это сделать.

//  Определим новый интерфейс ITimeService, который предназначен для получения времени:

//interface ITimeService
//{
//    string GetTime();
//}

//  И также определим два класса, которые будут реализовать данный интерфейс. Первый класс будет называться ShortTimeService и будет
//  возвращать текущее время в формате hh:mm(то есть часы и минуты):

//class ShortTimeService : ITimeService
//{
//    public string GetTime() => DateTime.Now.ToShortTimeString();
//}

//  Второй класс будет называться LongTimeService - он будет возвращать время в формате hh:mm:ss:

//class LongTimeService : ITimeService
//{
//    public string GetTime() => DateTime.Now.ToLongTimeString();
//}

//  Теперь добавим в коллекцию сервисов сервис ITimeService и используем его в приложении:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();

//var app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//});

//app.Run();

//  Здесь надо выделить два момента. Во-первых, добавление сервиса в коллекцию сервисов приложения:
//      builder.Services.AddTransient<ITimeService, ShortTimeService>();

//  Благодаря вызову AddTransient<ITimeService, ShortTimeService>() система на место объектов интерфейса ITimeService будет
//  передавать экземпляры класса ShortTimeService.

//  Кроме того, сервисы добавляются до создания объекта WebApplication методом Build() объекта WebApplicationBuilder:
//      // добавление сервисов
//      builder.Services.AddTransient<ITimeService, ShortTimeService>();
//      // создание объекта WebApplication
//      var app = builder.Build();

//  После добавления сервиса его можно получить и использовать в любой части приложения. Для получения сервиса могут применяться
//  различные способы в зависимости от ситуации. В данном случае используется свойство app.Services., которое предоставляет провайдер
//  сервисов - объект IServiceProvider. Для получения сервиса у провайдера сервиса вызывается метод GetService(), который типизируется
//  типом сервиса:
//      var timeService = app.Services.GetService<ITimeService>();

//  После получения сервиса мы можем использовать его.
//      await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");

//  Поскольку метод AddTransient установил зависимость между ITimeService и ShortTimeService, то в браузере выводится текущее время
//  в формате "hh:mm". Мы можем поменять тип, сопоставляемый с ITimeService:
//      builder.Services.AddTransient<ITimeService, LongTimeService>();
//  И в этом случае мы увидим другое сообщение.

#region Сервис как конкретный класс
//  При этом необязательно разделять определение сервиса в виде интерфейса и его реализацию. Сам термин "сервис" в данном случае может
//  представлять любой объект, функциональность которого может использоваться в приложении.

//  Например, определим новый класс TimeService:

//class TimeClassService
//{
//    public string GetTime() => DateTime.Now.ToShortTimeString();
//}

//  Данный класс определяет один метод GetTime(), который возвращает текущее время.

//  Используем этот класс в качестве сервиса:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<TimeClassService>();

//var app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<TimeClassService>();
//    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//});

//app.Run();

//  Для добавления сервиса в эту коллекцию применяется метод AddTransient():
//      builder.Services.AddTransient<TimeService>();

//  После добавления сервиса мы его можем получить и использовать в любой части приложения.
#endregion

#region Расширения для добавления сервисов
//  Нередко для сервисов создают собственные методы добавления в виде методов расширения для интерфейса IServiceCollection.
//  Например, создадим подобный метод для сервиса TimeService:

//public static class ServiceProviderExtensions
//{
//    public static void AddTimeService(this IServiceCollection services)
//    {
//        services.AddTransient<TimeClassService>();
//    }
//}

//  И теперь используем этот метод:

var builder = WebApplication.CreateBuilder();

builder.Services.AddTimeService();

var app = builder.Build();

app.Run(async context =>
{
    var timeService = app.Services.GetService<TimeClassService>();
    context.Response.ContentType = "text/html; charset = utf-8";
    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
});

app.Run();
#endregion

#region Конец кода
public static class ServiceProviderExtensions
{
    public static void AddTimeService(this IServiceCollection services)
    {
        services.AddTransient<TimeClassService>();
    }
}

class TimeClassService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}
interface ITimeService
{
    string GetTime();
}

// время в формате hh::mm
class ShortTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}

// время в формате hh:mm:ss
class LongTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToLongTimeString();
}
#endregion
