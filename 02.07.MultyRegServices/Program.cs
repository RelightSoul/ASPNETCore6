//  Множественная регистрация сервисов

//  По умолчанию при внедрении зависимостей в ASP.NET Core одна зависимость сопоставляется с одним типом. Однако бывают ситуации,
//  когда требуется отойти от этой привязки один к одному. Первая ситуация: для одной зависимости необходимо зарегистрировать сразу
//  несколько конкретных реализаций. Вторая ситуация: для нескольких зависимостей необходимо зарегистрировать один и тот же объект.

#region Регистрация для одной зависимости нескольких типов
//  ASP.NET Core позволяет зарегистрировать для одной зависимости сразу несколько типов. Рассмотрим простейший пример:

//var builder = WebApplication.CreateBuilder();
//builder.Services.AddTransient<IHelloService, RuHelloService>();
//builder.Services.AddTransient<IHelloService, EnHelloService>();

//var app = builder.Build();

//app.UseMiddleware<HelloMiddleware>();

//app.Run();


//interface IHelloService
//{
//    string Message { get; }
//}

//class RuHelloService : IHelloService
//{
//    public string Message => "Привет METANIT.COM";
//}
//class EnHelloService : IHelloService
//{
//    public string Message => "Hello METANIT.COM";
//}

//class HelloMiddleware
//{
//    readonly IEnumerable<IHelloService> helloServices;

//    public HelloMiddleware(RequestDelegate _, IEnumerable<IHelloService> helloServices)
//    {
//        this.helloServices = helloServices;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        context.Response.ContentType = "text/html; charset=utf-8";
//        string responseText = "";
//        foreach (var service in helloServices)
//        {
//            responseText += $"<h3>{service.Message}</h3>";
//        }
//        await context.Response.WriteAsync(responseText);
//    }
//}

//  Здесь интерфейс IHelloService с помощью свойства Message определяет некоторое сообщение. Этот интерфейс реализуется двумя классами:
//  RuHelloService и EnHelloService, каждый из которых определяет свое сообщение. И оба этих класса регистрируются в коллекции сервисов
//  в качестве реализаций для сервиса IHelloService. Далее все эти реализации можно получить в виде коллекции IEnumerable<IHelloService>.

//  Этот сервис применяется в middleware HelloMiddleware, который внедряется в конвейер обработки запроса и обрабатывает все запросы
//  к приложению. И в его конструкторе получаем все зарегистрированные реализации сервиса IHelloService через объект
//  IEnumerable<IHelloService>
#endregion

#region Регистрация одного объекта для нескольких зависимостей
//  Теперь рассмотрим другую ситуацию: использование несколькими зависимостями одного и то же объекта. Сначала рассмотрим ситуацию, с
//  которой мы можем столкнуться. Допустим, у нас есть следующие объекты:

//interface IGenerator
//{
//    int GenerateValue();
//}
//interface IReader
//{
//    int ReadValue();
//}
//class ValueStorage : IGenerator, IReader
//{
//    int value;
//    public int GenerateValue()
//    {
//        value = new Random().Next();
//        return value;
//    }

//    public int ReadValue() => value;
//}

//  Здесь интерфейс IGenerator предназначен для генерации некоторого числа, а интерфейс IReader - для чтения некоторого числа. Класс
//  ValueStorage реализует оба этих интерфейса: в методе Generate изменяет значение переменной value и возвращает его, а в методе
//  Read просто возвращает значение переменной value. То есть метод Read считывает текущее значение value, а метод Generate изменяет его.

//var builder = WebApplication.CreateBuilder();
//builder.Services.AddSingleton<IGenerator, ValueStorage>();
//builder.Services.AddSingleton<IReader, ValueStorage>();

//var app = builder.Build();

//app.UseMiddleware<GeneratorMiddleware>();
//app.UseMiddleware<ReaderMiddleware>();

//app.Run();

//class GeneratorMiddleware
//{
//    RequestDelegate next;
//    IGenerator generator;

//    public GeneratorMiddleware(RequestDelegate next, IGenerator generator)
//    {
//        this.next = next;
//        this.generator = generator;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Path == "/generate")
//            await context.Response.WriteAsync($"New Value: {generator.GenerateValue()}");
//        else
//            await next.Invoke(context);
//    }
//}
//class ReaderMiddleware
//{
//    IReader reader;

//    public ReaderMiddleware(RequestDelegate _, IReader reader) => this.reader = reader;

//    public async Task InvokeAsync(HttpContext context)
//    {
//        await context.Response.WriteAsync($"Current Value: {reader.ReadValue()}");
//    }
//}

//interface IGenerator
//{
//    int GenerateValue();
//}
//interface IReader
//{
//    int ReadValue();
//}
//class ValueStorage : IGenerator, IReader
//{
//    int value;
//    public int GenerateValue()
//    {
//        value = new Random().Next();
//        return value;
//    }

//    public int ReadValue() => value;
//}

//  Здесь для обоих зависимостей - IGenerator и IReader определена одна реализация - ValueStorage. При обращении по адресу
//  "/generate" срабатывает middleware GenerateMiddleware, который получает сервис IGenerator и с его помощью генерирует новое значение.

//  При обращении по всем иным адресам срабатывает middleware ReaderMiddleware, который получает сервис IReader и возвращает текущее
//  значение. Однако при запуске проекта мы увидим, что генерируемое значение и возвращаемое значения никак не синхронизированы,
//  потому что, несмотря на то, что оба сервиса представляют синглтоны, они используют два разных экземпляра класса ValueStorage

//  Для исправления ситуации нам надо определить один объект для обоих зависимостей. Это можно сделать, например, следующим образом:

var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton<ValueStorage>();
builder.Services.AddSingleton<IGenerator>(serv => serv.GetRequiredService<ValueStorage>());
builder.Services.AddSingleton<IReader>(serv => serv.GetRequiredService<ValueStorage>());

var app = builder.Build();

app.UseMiddleware<GeneratorMiddleware>();
app.UseMiddleware<ReaderMiddleware>();

app.Run();

class GeneratorMiddleware
{
    RequestDelegate next;
    IGenerator generator;

    public GeneratorMiddleware(RequestDelegate next, IGenerator generator)
    {
        this.next = next;
        this.generator = generator;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/generate")
            await context.Response.WriteAsync($"New Value: {generator.GenerateValue()}");
        else
            await next.Invoke(context);
    }
}
class ReaderMiddleware
{
    IReader reader;

    public ReaderMiddleware(RequestDelegate _, IReader reader) => this.reader = reader;

    public async Task InvokeAsync(HttpContext context)
    {
        await context.Response.WriteAsync($"Current Value: {reader.ReadValue()}");
    }
}

interface IGenerator
{
    int GenerateValue();
}
interface IReader
{
    int ReadValue();
}
class ValueStorage : IGenerator, IReader
{
    int value;
    public int GenerateValue()
    {
        value = new Random().Next();
        return value;
    }

    public int ReadValue() => value;
}

//  Теперь определяем один объект ValueStorage в виде синглтон-сервиса:
//          builder.Services.AddSingleton<ValueStorage>();

//  Затем получаем его из коллекции сервисов и устанавливаем в качестве реализации для обоих зависимостей:
//      builder.Services.AddSingleton<IGenerator>(serv => serv.GetRequiredService<ValueStorage>());
//      builder.Services.AddSingleton<IReader>(serv => serv.GetRequiredService<ValueStorage>());

//  В качестве альтернативы можно было бы создать объект во вне и передать его сервисам:
//      var valueStorage = new ValueStorage();
//      builder.Services.AddSingleton<IGenerator>(_ => valueStorage);
//      builder.Services.AddSingleton<IReader>(_ => valueStorage);
#endregion