//  Жизненный цикл зависимостей

//  ASP.NET Core позволяет управлять жизненным циклом внедряемых в приложении сервисов. С точки зрения жизненного цикла сервисы могут
//  представлять один из следующих типов:
//      Transient: при каждом обращении к сервису создается новый объект сервиса. В течение одного запроса может быть несколько обращений
//      к сервису, соответственно при каждом обращении будет создаваться новый объект. Подобная модель жизненного цикла наиболее подходит
//      для легковесных сервисов, которые не хранят данных о состоянии
//      Scoped: для каждого запроса создается свой объект сервиса. То есть если в течение одного запроса есть несколько обращений к одному
//      сервису, то при всех этих обращениях будет использоваться один и тот же объект сервиса.
//      Singleton: объект сервиса создается при первом обращении к нему, все последующие запросы используют один и тот же ранее созданный
//      объект сервиса

//  Для создания каждого типа сервиса предназначен соответствующий метод AddTransient(), AddScoped() и AddSingleton().

//  Для рассмотрения механизма внедрения зависимостей и жизненного цикла возьмем следующий интерфейс ICounter:

//public interface ICounter
//{
//    int Value { get; }
//}

//  Также определим реализацию этого интерфейса - класс RandomCounter

//public class RandomCounter : ICounter
//{
//    static Random rnd = new Random();
//    private int _value;
//    public RandomCounter()
//    {
//        _value = rnd.Next(0, 1000000);
//    }
//    public int Value
//    {
//        get => _value;
//    }
//}
//  Суть класса RandomCounter состоит в генерации некоторого случайного числа в диапазоне от 0 до 1000000.

//  И также определим новый класс, который нам более детально поможет разобраться в механизме Depedency Injection. Этот класс
//  назовем CounterService и определим в нем следующий код:

//public class CounterService
//{
//    public ICounter Counter { get; }
//    public CounterService(ICounter counter)
//    {
//        Counter = counter;  
//    }
//}
//  Данный класс просто устанавливает объект ICounter, передаваемый через конструктор.

//  Для работы с сервисами определим компонент middleware, который назовем CounterMiddleware:

//public class CounterMiddleware
//{
//    RequestDelegate next;
//    int i = 0; // счётчик запросов
//    public CounterMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext httpContext, ICounter counter, CounterService counterService)
//    {
//        i++;
//        httpContext.Response.ContentType = "text/html; charset=utf-8";
//        await httpContext.Response.WriteAsync($"Запрос {i}; Counter: {counter.Value}; Service: {counterService.Counter.Value}");
//    }
//}

//  Для получения зависимостей здесь используется метод InvokeAsync, в котором передаются две зависимости ICounter и CounterService.
//  В самом методе выводятся значения Value из обоих зависимостей. Причем сервис CounterService сам использует зависимость ICounter.

//  Теперь на примере этих классов рассмотрим уравление жизненным циклом сервисов.

#region AddTransient
//  Метод AddTransient() создает transient-объекты. Такие объекты создаются при каждом обращении к ним. Данный метод имеет ряд
//  перегруженных версий:
//  AddTransient(Type serviceType)
//  AddTransient(Type serviceType, Type implementationType)
//  AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
//  AddTransient<TService>()
//  AddTransient<TService, TImplementation>()
//  AddTransient<TService>(Func < IServiceProvider, TService > implementationFactory)
//  AddTransient<TService, TImplementation>(Func < IServiceProvider, TImplementation > implementationFactory)

//  Используем данный метод для добавления сервисов:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddTransient<ICounter, RandomCounter>();
//builder.Services.AddTransient<CounterService>();

//var app = builder.Build();

//app.UseMiddleware<CounterMiddleware>();

//app.Run();

//  В нашем случае CounterMiddleware получает объект ICounter, для которого создается один экземпляр класса RandomCounter.
//  CounterMiddleware также получает объект CounterService, который также использует ICounter. И для этого ICounter будет
//  создаваться второй экземпляр класса RandomCounter. Поэтому генерируемые случайные числа обоими экземплярами не совпадают.
//  Таким образом, применение AddTransient создаст два разных объекта RandomCounter.

//  При втором и последующих запросах к контроллеру будут создаваться новые объекты RandomCounter.
#endregion

#region AddScoped
//  Метод AddScoped создает один экземпляр объекта для всего запроса. Он имеет те же перегруженные версии, что и AddTransient.
//  Для его применения изменим код приложения следующим образом:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddScoped<ICounter, RandomCounter>();
//builder.Services.AddScoped<CounterService>();

//var app = builder.Build();

//app.UseMiddleware<CounterMiddleware>();

//app.Run();

//  Теперь в рамках одного и того же запроса и CounterMiddleware и сервис CounterService будут использовать один и тот же объект
//  RandomCounter. При следующем запросе к приложению будет генерироваться новый объект RandomCounter.
#endregion

#region AddSingleton
//  AddSingleton создает один объект для всех последующих запросов, при этом объект создается только тогда, когда он непосредственно
//  необходим. Этот метод имеет все те же перегруженые версии, что и AddTransient и AddScoped.

//  Для применения AddSingleton изменим код приложения:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddSingleton<ICounter, RandomCounter>();
//builder.Services.AddSingleton<CounterService>();

//var app = builder.Build();

//app.UseMiddleware<CounterMiddleware>();

//app.Run();

//  Для создания singleton-объектов необязательно полагаться на механизм Depedency Injection. Мы его можем сами создать и
//  передать в нужный метод:

var builder = WebApplication.CreateBuilder();

RandomCounter rndCounter = new RandomCounter();
builder.Services.AddSingleton<ICounter>(rndCounter);
builder.Services.AddSingleton<CounterService>(new CounterService(rndCounter));

var app = builder.Build();

app.UseMiddleware<CounterMiddleware>();

app.Run();
//  Работа приложения в данном случае будет аналогична предыдущему примеру.
#endregion