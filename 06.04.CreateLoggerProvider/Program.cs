//  �������� ���������� ������������

//  ����������� �������������� ASP NET Core �������������, ��������, �� ����� ������� ������� ������������ - �� �������,
//  � ���� Output � Visual Studio. ������ � �� �� ����� ASP.NET Core ��������� ��������� ���������� ���� ������ ������� ����.
//  ��������, �� ����� ��������� ��������� � ��������� �����.

//  ������� ������� � ������ ����� ����� FileLogger:

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

//  ����� ������� ������ ����������� ��������� ILogger. ���� ��������� ���������� ��� ������:
//      BeginScope: ���� ����� ���������� ������ IDisposable, ������� ������������ ��������� ������� ��������� ��� �������.
//      � ������ ������ ��� ���� ����� �� �����, ������� ���������� �������� this - ������ �� ������� ������ ������, �������
//      ��������� ��������� IDisposable.
//      IsEnabled: ���������� �������� true ��� false, ������� ��������, �������� �� ������ ��� �������������. ����� ����� �����
//      ��������� ������. � ���������, � ���� ����� ���������� ������ LogLevel, � �� �����, � �������, ������������� ������ �
//      ����������� �� �������� ����� �������. �� � ������ ������ ������ ���������� true, �� ���� ������ �������� ������.
//      Log: ���� ����� ������������ ��� ���������� ������������. �� ��������� ���� ����������:
//          LogLevel: ������� ����������� �������� ���������
//          EventId: ������������� �������
//          TState: ��������� ������ ���������, ������� ������ ���������
//          Exception: ���������� �� ����������
//          formatter: ������� ��������������, ������� � ������� ���� ���������� ��������� ��������� �������� ���������� ���������
//          ��� ������������
//      � � ������ ������ ��� ��� � ������������ ������ � ��������� ����. ���� � ����� ����� ���������� ����� �����������

//  ����� ������� � ������ ����� FileLoggerProvider:

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

//  ���� ����� ������������ ��������� ������������. �� ������ ����������� ��������� ILoggerProvider. � ���� ���������� ���������
//  ��� ������:
//      CreateLogger: ������� � ���������� ������ �������. ��� �������� ������� ������������ ���� � �����, ������� ���������� �����
//      �����������
//      Dispose: ��������� ������������ ��������. � ������ ������ ������ ����������

//  ������ �������� ��������������� ����� FileLoggerExtensions:

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new FileLoggerProvider(filePath));
        return builder;
    }
}

//  ���� ����� ��������� � ������� ILoggingBuilder ����� ���������� AddFile, ������� ����� ��������� ��� ��������� ������������.

//  ������ ���������� ��������� � ����� Program.cs:

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        // ������������� ���� ��� ������������
        builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
        // ��������� ������������ � ������� �������� Logging ���� �� �������� ������� WebApplication

        var app = builder.Build();

        app.Run(async (context) =>
        {
            app.Logger.LogInformation($"Path: {context.Request.Path} Time: {DateTime.Now.ToLongTimeString()}");
            await context.Response.WriteAsync("0_0 <----<<");
        });

        app.Run();
    }
}

//  �� ���������� ��������� �������� ������������ �������� �������� Logging ������ WebApplicationBuilder. ��� ��������
//  ������������ ������ ILoggingBuilder � ������������� ��� ������� ��� ���������� �������������. � � ������ ������ � �������
//  ���� ������������� ������ AddFile ��������� ������������ � ����.

//  ����� ��������, ��� ���������� ��������� ������������ ������ ���� �� �������� ������� WebApplication.

//  ������ ��� ������������ ����� ����� �������������� ���� logger.txt, ������� ����� ����������� � ����� �������.