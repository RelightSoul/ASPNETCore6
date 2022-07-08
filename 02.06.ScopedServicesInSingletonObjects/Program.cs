//  Scoped-сервисы в singleton-объектах

//  Все объекты, которые используются в ASP.NET Core, имеет три варианта жизненного цикла. Singleton-объекты создаются один раз при
//  запуске приложения, и при всех запросах к приложению оно использует один и тот же singleton-объект. К подобным singleton-объектам
//  относятся, к примеру, компоненты middleware или сервисы, которые регистрируются с помощью метода AddSingleton().

//  Transient - объекты создаются каждый раз, когда нам требуется экземпляр определенного класса. А scoped-объекты создаются по одному
//  на каждый запрос.

//  Одни объекты или сервисы с помощью встроенного механизма dependency injection можно передать в другие объекты. Наиболее
//  распространенный способ внедрения объектов предсталяет инъекция через конструктор. Однако начиная с версии ASP.NET Core 2.0 мы не
//  можем передавать scoped-сервисы в конструктор singleton-объектов.

//  Например, пусть будут опеделены следующие классы:

//public interface ITimer
//{
//    string Time { get; }
//}
//public class Timer : ITimer
//{
//    public Timer()
//    {
//        Time = DateTime.Now.ToLongTimeString();
//    }
//    public string Time { get; }
//}
//public class TimeService
//{
//    private ITimer timer;
//    public TimeService(ITimer timer)
//    {
//        this.timer = timer;
//    }
//    public string GetTime() => timer.Time;
//}
//  TimeService получает через конструктор сервис ITimer и использует его для получения текущего времени.

//  Также пусть будет определен компонент middleware TimerMiddleware:

//public class TimerMiddleware
//{
//    TimeService timeService;
//    public TimerMiddleware(RequestDelegate next, TimeService timeService)
//    {
//        this.timeService = timeService;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//    }
//}

//  Компонент TimerMiddleware получает сервис TimeService и отправляет в ответ клиенту информацию о текущем времени.

//  TimerMiddleware является singleton-объектом. И теперь зарегистрируем сервис TimeService как scoped-объект:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ITimer, Timer>();
//builder.Services.AddScoped<TimeService>();

//var app = builder.Build();

//app.UseMiddleware<TimerMiddleware>();

//app.Run();

//  То есть на момент создания объекта TimerMiddleware scoped-сервис TimeService еще не установлен, соответственно он использоваться
//  не может. А без создания объекта TimeService нельзя создать объект TimerMiddleware.

//  Аналогичная ситуация может возникнуть, если TimeService добавляется как Transient, а сервис ITimer определен как Scoped.

//  Для выхода из этой ситуации ни TimeService, ни ITimer не должны иметь жизненный цикл Scoped. То есть это может быть Transient
//  или Singleton.

//  Рассмотрим еще одну ситуацию, с которой можно столкнуться в любой части приложения, а не только в конструкторе middleware, когда
//  сервис TimeService представляет singleton, а ITimer - scoped-объект:

var builder = WebApplication.CreateBuilder();

builder.Services.AddScoped<ITimer, Timer>();
builder.Services.AddSingleton<TimeService>();

var app = builder.Build();

app.UseMiddleware<TimerMiddleware>();

app.Run();

//  И, допустим, эти сервисы используются в TimerMiddleware непосредственно при обработке запроса в методе Invoke/InvokeAsync:

//public class TimerMiddleware
//{
//    public TimerMiddleware(RequestDelegate next) { }

//    public async Task Invoke(HttpContext context, TimeService timeService)
//    {
//        await context.Response.WriteAsync($"Time: {timeService?.GetTime()}");
//    }
//}

//  При запуске приложения мы опять же столкнемся с ошибкой, только немного другой "Cannot consume scoped service 'DIApp.ITimer'
//  from singleton 'DIApp.TimeService'"

//  Но суть будет та же самая - мы не можем по умолчанию передавать в конструктор singleton-объекта scoped-сервис.