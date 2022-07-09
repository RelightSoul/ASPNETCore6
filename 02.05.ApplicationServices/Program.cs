//  Применение сервисов в классах middleware

//  После добавления сервисов в коллекцию Services объекта WebApplicationBuilder они становятся доступны приложению, в том числе и в
//  кастомных компонентах middleware. В middleware мы можем получить зависимости тремя способами:
//  Через конструктор
//  Через параметр метода Invoke/InvokeAsync
//  Через свойство HttpContext.RequestServices

//  При этом надо учитывать, что компоненты middleware создаются при запуске приложения и живут в течение всего жизненного цикла
//  приложения. То есть при последующих запросах инфраструктура ASP.NET Core использует ранее созданный компонент. И это налагает
//  ограничения на использование зависимостей в middleware.

//  В частности, если конструктор передается transient-сервис, который создается при каждом обращении к нему, то при последующих
//  запросах мы будем использовать тот же самый сервис, так как конструктор middleware вызывается один раз - при создании приложения.

//  Например, определим сервис TimeService:

//public class TimeService
//{
//    public TimeService()
//    {
//        Time = DateTime.Now.ToLongTimeString();
//    }
//    public string Time { get; }
//}
//  В конструкторе устанавливается свойство, которое хранит текущее время в виде строки.

//  Добавим новый компонент TimerMiddleware, который будет использовать этот сервис для вывода времени на веб-страницу:

//public class TimerMiddleware
//{
//    RequestDelegate next;
//    TimeService timeService;

//    public TimerMiddleware(RequestDelegate next, TimeService timeService)
//    {
//        this.next = next;
//        this.timeService = timeService;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Path == "/time")
//        {
//            context.Response.ContentType = "text/html; charset=utf-8";
//            await context.Response.WriteAsync($"Текущее время: {timeService?.Time}");
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}

//  Если сервис TimeService добавляется в коллекцию сервисов приложения, то мы сможем получить его через конструктор класса TimerMiddleware.

//  Логика компонента предполагает, что, если запрос пришел по адресу "/time", то с помощью TimeService возвращается текущее время.
//  Иначе мы просто обращаемся к следующему middleware в конвейере обработки запроса.

var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<TimeService>();

var app = builder.Build();

app.UseMiddleware<TimerMiddleware>();
app.Run(async context => await context.Response.WriteAsync("Hey all"));

app.Run();

//  В итоге, если мы обратимся по пути "/time", то приложение выведет текущее время. Однако сколько бы мы раз не обращались по этому
//  пути, мы все время будем получать одно и то же время, так как объект TimerMiddleware был создан еще при первом запросе. Поэтому
//  передача через конструктор middleware больше подходит для сервисов с жизненным циклом Singleton, которые создаются один раз для
//  всех последующих запросов.

//  Если же в middleware необходимо использовать сервисы с жизненным циклом Scoped или Transient, то лучше их передавать через
//  параметр метода Invoke/InvokeAsync:

//public class TimerMiddleware
//{
//    RequestDelegate next;

//    public TimerMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }

//    public async Task InvokeAsync(HttpContext context, TimeService timeService)
//    {
//        if (context.Request.Path == "/time")
//        {
//            context.Response.ContentType = "text/html; charset=utf-8";
//            await context.Response.WriteAsync($"Текущее время: {timeService?.Time}");
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}