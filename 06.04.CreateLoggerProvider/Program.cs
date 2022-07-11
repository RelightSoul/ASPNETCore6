//  Создание провайдера логгирования

//  Стандартная инфраструктура ASP NET Core предоставляет, возможно, не самые удобные способы логгирования - на консоль,
//  в окне Output в Visual Studio. Однако в то же время ASP.NET Core позволяет полностью определить свою логику ведения лога.
//  Допустим, мы хотим сохранять сообщения в текстовом файле.

//  Вначале добавим в проект новый класс FileLogger:

public class FileLogger : ILogger, IDisposable
{
    string filePath;
    static object _lock = new object();
    public FileLogger(string filePath)
    {
        this.filePath = filePath;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
       return this;
    }

    public void Dispose() { }

    public bool IsEnabled(LogLevel logLevel)
    {
        //return logLevel == LogLevel.Trace;
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, 
        Func<TState, Exception?, string> formatter)
    {
        lock (_lock)
        {
            File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
        }
    }
}

//  Класс логгера должен реализовать интерфейс ILogger. Этот интерфейс определяет три метода:
//      BeginScope: этот метод возвращает объект IDisposable, который представляет некоторую область видимости для логгера.
//      В данном случае нам этот метод не важен, поэтому возвращаем значение this - ссылку на текущий объект класса, который
//      реализует интерфейс IDisposable.
//      IsEnabled: возвращает значения true или false, которые указыват, доступен ли логгер для использования. Здесь можно здать
//      различную логику. В частности, в этот метод передается объект LogLevel, и мы можем, к примеру, задействовать логгер в
//      зависимости от значения этого объекта. Но в данном случае просто возвращаем true, то есть логгер доступен всегда.
//      Log: этот метод предназначен для выполнения логгирования. Он принимает пять параметров:
//          LogLevel: уровень детализации текущего сообщения
//          EventId: идентификатор события
//          TState: некоторый объект состояния, который хранит сообщение
//          Exception: информация об исключении
//          formatter: функция форматирования, которая с помощью двух предыдущих параметов позволяет получить собственно сообщение
//          для логгирования
//      И в данном методе как раз и производится запись в текстовый файл. Путь к этому файлу передается через конструктор

//  Далее добавим в проект класс FileLoggerProvider:

public class FileLoggerProvider : ILoggerProvider
{
    string path;
    public FileLoggerProvider(string path)
    {
        this.path = path;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(path);
    }

    public void Dispose() { }
}

//  Этот класс представляет провайдер логгирования. Он должен реализовать интерфейс ILoggerProvider. В этом интерфейсе определны
//  два метода:
//      CreateLogger: создает и возвращает объект логгера. Для создания логгера используется путь к файлу, который передается через
//      конструктор
//      Dispose: управляет освобождение ресурсов. В данном случае пустая реализация

//  Теперь создадим вспомогательный класс FileLoggerExtensions:

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new FileLoggerProvider(filePath));
        return builder;
    }
}

//  Этот класс добавляет к объекту ILoggingBuilder метод расширения AddFile, который будет добавлять наш провайдер логгирования.

//  Теперь используем провайдер в файле Program.cs:

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        // устанавливаем файл для логгирования
        builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
        // настройка логгирования с помошью свойства Logging идет до создания объекта WebApplication

        var app = builder.Build();

        app.Run(async (context) =>
        {
            app.Logger.LogInformation($"Path: {context.Request.Path} Time: {DateTime.Now.ToLongTimeString()}");
            await context.Response.WriteAsync("0_0 <----<<");
        });

        app.Run();
    }
}

//  За глобальную установку настроек логгирования отвечает свойство Logging класса WebApplicationBuilder. Это свойство
//  представляет объект ILoggingBuilder и предоставляет ряд методов для управления логгированием. И в данном случае с помошью
//  выше определенного метода AddFile добавляем логгирование в файл.

//  Стоит отметить, что глобальная настройка логгирования должна идти до создания объекта WebApplication.

//  Теперь для логгирования также будет использоваться файл logger.txt, который будет создаваться в папке проекта.