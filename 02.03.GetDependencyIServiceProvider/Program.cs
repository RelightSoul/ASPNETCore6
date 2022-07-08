//  Получение зависимостей

//  В ASP.NET Core мы можем получить добавленные в приложения сервисы различными способами;

//  Через свойство Services объекта WebApplication (service locator)
//  Через свойство RequestServices контекста запроса HttpContext в компонентах middleware (service locator)
//  Через конструктор класса    
//  Через параметр метода Invoke компонента middleware
//  Через свойство Services объекта WebApplicationBuilder

//  Для работы определим интерфейс ITimeService и класс ShortTimeService, который реализует данный интерфейс:

//interface ITimeService
//{
//    string GetTime();
//}
//class ShortTimeService : ITimeService
//{
//    public string GetTime() => DateTime.Now.ToShortTimeString();
//}

#region Свойство Services объекта WebApplication
//  Там, где нам доступен объект WebApplication, который представляет текущее приложение, (например, в файле Program.cs), для получения
//  сервисов мы можем использовать его свойство Services. Это свойство предоставляет объект IServiceProvider, который предоставляет ряд
//  методов для получения сервисов:
//      GetService<service>(): использует провайдер сервисов для создания объекта, который представляет тип service. В случае если в
//      провайдере сервисов для данного сервиса не установлена зависимость, то возвращает значение null
//      GetRequiredService<service>(): использует провайдер сервисов для создания объекта, который представляет тип service. В случае
//      если в провайдере сервисов для данного сервиса не установлена зависимость, то генерирует исключение

//  Данный паттерн получения сервиса еще называется service locator, и, как правило, не рекомендуется к использованию, но тем не менее
//  в рамках ASP.NET Core в принципе мы можем использовать подобную функциональность особенно там, где другие способы получения
//  зависимостей не доступны.

//  Например, определим следующий код приложения:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();

//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time {timeService?.GetTime()}");
//});

//app.Run();

//  В данном случае с помощью строки кода
//      var timeService = app.Services.GetService<ITimeService>();
//  Получаем из коллекции сервисов объект сервиса ITimeService - в данном случае он будет представлять объект ShortTimeService

//  Возможна ситуация, когда сервис не будет добавлен в коллекцию сервисов, однако в какой-то части приложения мы может попытаться
//  его получить:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();

//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time {timeService?.GetTime()}");
//});

//app.Run();
//  В этом случае переменная timeService будет иметь значение null.

//  Аналогичный образом можно использовать метод GetRequiredService() за тем исключением, что если сервис не добавлен, то метод
//  генерирует исключение:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();

//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    var timeService = app.Services.GetRequiredService<ITimeService>();
//    await context.Response.WriteAsync($"Time {timeService.GetTime()}");
//});

//app.Run();
#endregion

#region HttpContext.RequestServices
//  Там, где нам доступен объект HttpContext, мы можем использовать для получения сервисов его свойство RequestServices. Это свойство
//  предоставляет объект IServiceProvider. То есть по сути мы имеем дело с выше описанным способом получения сервисов с помощью
//  методов GetService() и GetRequiredService():

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();

//var app = builder.Build();

//app.Run(async(HttpContext context)=>
//{
//    var timeService = context.RequestServices.GetService<ITimeService>();
//    await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//});

//app.Run();
#endregion

#region Конструкторы
//  Встроенная в ASP.NET Core система внедрения зависимостей использует конструкторы классов для передачи всех зависимостей. Передача
//  сервисов через конструкторы является предпочтительным способом внедрения зависимостей.

//  Например, пусть в проекте определен следующий класс TimeMessage:

//class TimeMessage
//{
//    ITimeService timeService;
//    public TimeMessage(ITimeService timeService)
//    {
//        this.timeService = timeService;
//    }
//    public string GetTime() => $"Time: {timeService.GetTime()}";
//}
//  Здесь через конструктор класса передается зависимость от ITimeService. Причем здесь неизвестно, что это будет за реализация
//  интерфейса ITimeService. В методе GetTime() формируем сообщение, в котором из сервиса получаем текущее время.

//  Для использования класса TimeMessage определим следующее приложение:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimeService, ShortTimeService>();
//builder.Services.AddTransient<TimeMessage>();

//var app = builder.Build();

//app.Run(async context =>
//{
//    var timeServise = context.RequestServices.GetService<TimeMessage>();
//    context.Response.ContentType = "text/html; charset=utf-8";
//    await context.Response.WriteAsync($"<h2>{timeServise?.GetTime()}</h2>");
//});

//app.Run();

//  Для использования в приложении в качестве сервиса класс MessageService также добавляется в коллекцию сервисов. Поскольку это
//  самодостаточная зависимость, которая представляет конкретный класс, то метод builder.Services.AddTransient типизируется одним
//  этим типом TimeMessage. То есть классы, которые используют сервисы, сами могут выступать в качестве сервисов.

//  Но так как класс TimeMessage использует зависимость ITimeService, которая передается через конструктор, то нам надо также
//  установить и эту зависимость:
//      builder.Services.AddTransient<ITimeService, ShortTimeService>();
//  И когда при обработке запроса будет использоваться класс TimeMessage, для создания объекта этого класса будет вызываться провайдер
//  сервисов. Провайдер сервисов проверят конструктор класса TimeMessage на наличие зависимостей. Затем создает объекты для всех
//  используемых зависимостей и передает их в конструктор.
#endregion

#region Метод Invoke/InvokeAsync компонентов middleware
//  Подобно тому, как зависимости передаются в конструктор классов, точно также их можно передавать в метод Invoke/InvokeAsync()
//  компонента middleware. Например,определим следующий компонент:

//class TimeMessageMiddleware
//{
//    private readonly RequestDelegate next;
//    public TimeMessageMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext context, ITimeService timeService)
//    {
//        context.Response.ContentType = "text/html; charset=utf-8";
//        await context.Response.WriteAsync($"<h1>Time: {timeService?.GetTime()}</h1>");
//    }
//}

//  Применим компонент для обработки запроса:
var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<ITimeService, ShortTimeService>();

var app = builder.Build();

app.UseMiddleware<TimeMessageMiddleware>();

app.Run();

//  Стоит отметить, что мы также могли бы передать зависимость и через конструктор класса middleware:

//class TimeMessageMiddleware
//{
//    private readonly RequestDelegate next;
//    ITimeService timeService;
//    public TimeMessageMiddleware(RequestDelegate next, ITimeService timeService)
//    {
//        this.next = next;
//        this.timeService = timeService;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        context.Response.ContentType = "text/html; charset=utf-8";
//        await context.Response.WriteAsync($"<h1>Time: {timeService?.GetTime()}</h1>");
//    }
//}
#endregion

#region Конец кода
class TimeMessageMiddleware
{
    private readonly RequestDelegate next;
    public TimeMessageMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context, ITimeService timeService)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync($"<h1>Time: {timeService?.GetTime()}</h1>");
    }
}
class TimeMessage
{
    ITimeService timeService;
    public TimeMessage(ITimeService timeService)
    {
        this.timeService = timeService;
    }
    public string GetTime() => $"Time: {timeService.GetTime()}";
}
interface ITimeService
{
    string GetTime();
}
class ShortTimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}
#endregion