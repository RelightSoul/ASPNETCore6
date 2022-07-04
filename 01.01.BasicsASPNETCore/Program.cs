//  В центре приложения ASP.NET находится класс WebApplication. Например, если мы возьмем проект ASP.NET по типу
//  ASP.NET Core Empty, то в файле Program.cs мы встретим следующий код:
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
//  Переменная app в данном коде как раз представляет объект WebApplication. Однако для создания этого объекта
//  необходим другой объект - WebApplicationBuilder, который в данном коде представлен переменной builder.

#region Класс WebApplicationBuilder
//  Создание приложения по умолчанию фактически начинается с класса WebApplicationBuilder. Исходный код доступен по
//  адресу WebApplicationBuilder.cs

//  Для его создания объекта этого класса вызывается статический метод WebApplication.CreateBuilder():
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();

//  Для инициализации объекта WebApplicationBuilder в этот метод могут передаваться аргументы командной строки,
//  указанные при запуске приложения (доступны через неявно определенный параметр args):
//  WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//  Либо можно передавать объект WebApplicationOption:
//  WebApplicationOptions options = new() { Args = args };
//  WebApplicationBuilder builder = WebApplication.CreateBuilder(options);

//  Кроме создания объекта WebApplication класс WebApplicationBuilder выполняет еще ряд задач, среди которых
//  можно выделить следующие:
//      Установка конфигурации приложения
//      Добавление сервисов
//      Настройка логгирования в приложении
//      Установка окружения приложения
//      Конфигурация объектов IHostBuilder и IWebHostBuilder, которые применяются для создания хоста приложения

//  Для реализации этих задач в классе WebApplicationBuilder определены следующие свойства:
//      Configuration: представляет объект ConfigurationManager, который применяется для добавления конфигурации к приложению.
//      Environment: предоставляет информацию об окружении, в котором запущено приложение.
//      Host: объект IHostBuilder, который применяется для настройки хоста.
//      Logging: позволяет определить настройки логгирования в приложении.
//      Services: представляет коллекцию сервисов и позволяет добавлять сервисы в приложение.
//      WebHost: объект IWebHostBuilder, который позволяет настроить отдельные настройки сервера.
#endregion

#region Класс WebApplication
//  Метод build() класса WebApplicationBuilder создает объект WebApplication:
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();
//  WebApplication app = builder.Build();

//  Класс WebApplication применяется для управления обработкой запроса, установки маршрутов, получения сервисов и т.д.
//  Исходный код класса можно найти на Github по адресу WebApplication.cs.

//  Класс WebApplication применяет три интерфейса:
//      IHost: применяется для запуска и остановки хоста, который прослушивает входящие запросы
//      IApplicationBuilder: применяется для установки компонентов, которые участвуют в обработке запроса
//      IEndpointRouteBuilder: применяется для установки маршрутов, которые сопоставляются с запросами

//  Для получения доступа к функциональности приложения можно использовать свойства класса WebApplication:
//      Configuration: представляет конфигурацию приложения в виде объекта IConfiguration
//      Environment: представляет окружение приложения в виде IWebHostEnvironment
//      Lifetime: позволяет получать уведомления о событиях жизненного цикла приложения
//      Logger: представляет логгер приложения по умолчанию
//      Services: представляет сервисы приложения
//      Urls: представляет набор адресов, которые использует сервер

//  Для управления хостом класс WebApplication определяет следующие методы:
//      Run(): запускает приложение//
//      RunAsync(): асинхронно запускает приложение
//      Start(): запускает приложение
//      StartAsync(): запускает приложение
//      StopAsync(): останавливает приложение

//  Таким образом, после вызова метод Run/Start/RunAsync/StartAsync приложение будет заущено, и мы сможем к нему обращаться:
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();
//  WebApplication app = builder.Build();
//  app.Run();

//  При необходимости с помощью метода StopAsync() можно программным способом завершить выполнение приложения:
//  WebApplicationBuilder builder = WebApplication.CreateBuilder();

//  WebApplication app = builder.Build();

//  app.MapGet("/", () => "Hello World!");

//  await app.StartAsync();
//  await Task.Delay(10000);
//  await app.StopAsync();  // через 10 секунд завершаем выполнение приложения
#endregion
