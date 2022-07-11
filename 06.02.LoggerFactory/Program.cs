//  Фабрика логгера и провайдеры логгирования

//  В примерах в прошлой теме мы получали объект логгера, который добавляется через DI. Но мы можем также использовать фабрику
//  логгера для его создания:

//using Microsoft.Extensions.Logging;

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
//ILogger logger = loggerFactory.CreateLogger<Program>();

//app.Run(async context =>
//{
//    logger.LogInformation($"Reuqest path: {context.Request.Path}");
//    await context.Response.WriteAsync("Hell prog world");
//});

//app.Run();

//  В данном случае с помощью метода LoggerFactory.Create создается фабрика логгера в виде объекта ILoggerFactory. В качестве
//  параметра метод принимает делегат, который устанавливает некоторые настройки логгирования. В частности, метод AddConsole()
//  объекта ILoggingBuilder устанавливает вывод сообщений лога на консоль. Затем метод CreateLogger() фабрики собственно создает логгер:
//  ILogger logger = loggerFactory.CreateLogger<Program>();

//  Метод CreateLogger() типизируется классом, который представляет категорию. В данном случае это класс Program, в котором неявно
//  выполняется данный код. Но в качестве альтернативы название категории можно передать в метод в качестве параметра в виде строки:
//  ILogger logger = loggerFactory.CreateLogger("WebApplication");

//  В итоге мы получим тот же вывод сообщений на консоль. Но преимущество использования фабрики логгеров состоит в том, что мы можем
//  дополнительно настроить различные параметры логгирования, в частности, провайдер логгирования

#region Получение фабрики логгера через dependency injection
//  Как и логгер, фабрика логгера доступна в приложении в виде сервиса, соответственно ее можно получит через механизм внедрения
//  зависимостей:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/hello", (ILoggerFactory loggerFactory) => 
//{
//    ILogger logger = loggerFactory.CreateLogger("MapLogger");
//    logger.LogInformation($"Path: /hello, time: {DateTime.Now.ToLongTimeString()}");
//    return "HeLL()";
//});

//app.Run();
#endregion

#region Провайдеры логгирования
//  В примере выше логгирование шло на консоль. Вообще путь логгирования определяется провайдером логгирования. По умолчанию
//  ASP.NET Core предоставляет следующие провайдеры:

//  Console: вывод информации на консоль.Устанавливается методом AddConsole()

//  Debug: использует для ведения записей лога класс System.Diagnostics.Debug и в частности его метод Debug.WriteLine.Соответственно
//  все записи лога мы можем увидеть в окне Output в Visual Studio.Устанавливается методом AddDebug(). Стоит отметить, что данный
//  способ работает только при запуске проекта в режиме отладки

//  EventSource: на Windows введет логгирование в лог ETW (Event Tracing for Windows), для просмотра которого может использоваться
//  инструмент PerfView (или аналогичный инструменты). Хотя данный провайдер задумывался как кроссплатформенный, для Linux и MacOS
//  пока назначение лога не определено. Устанавливается методом AddEventSourceLogger()

//  EventLog: записывает в Windows Event Log, соответственно работает только при запуске на Windows. Устанавливается методом AddEventLog()

//  Например, вместо консоли зададим вывод лога в окне Output в Visual Studio:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
ILogger logger = loggerFactory.CreateLogger<Program>();
app.Run(async (context) =>
{
    logger.LogInformation($"Requested Path: {context.Request.Path}");
    await context.Response.WriteAsync("Hello World!");
});

app.Run();
#endregion