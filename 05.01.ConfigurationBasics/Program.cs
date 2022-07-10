//  Конфигурация
//  Основы конфигурации

//  Важную роль в приложении играет конфигурация, которая определяет базовые настройки приложения. Приложение ASP.NET Core может
//  получать конфигурационные настройки из следующих источников:
//      Аргументы командной строки
//      Переменные среды окружения
//      Объекты .NET в памяти
//      Файлы (json, xml, ini)
//      Azure
//      Можно использовать свои кастомные источники и под них создавать провайдеры конфигурации

#region Интерфейс IConfiguration
//  Конфигурация приложения в ASP.NET Core представляет объект интерфейса IConfiguration:

//using Microsoft.Extensions.Primitives;

//public interface IConfiguration
//{
//    string this[string key] { get; set; }
//    IEnumerable<IConfigurationSection> GetChildren();
//    IChangeToken GetReloadToken();
//    IConfigurationSection GetSection(string key);
//}

//  Данный интерфейс содержит следующие компоненты:
//      this[string key]: индексатор, через который можно получить по ключу значение параметра конфигурации. Стоит отметить, что и ключ,
//      и значение параметра конфигурации представляет собой объект типа string
//      GetChildren(): возвращает набор подсекций текущей секции конфигурации в виде объекта IEnumerable<IConfigurationSection>
//      GetReloadToken(): возвращает объект IChangeToken, который применяется для отслеживания изменения конфигурации
//      GetSection(string key): возвращает секцию конфигурации, которая соответствует ключу key

//  Также конфигурация может быть представлена интерфейсом IConfigurationRoot, который наследуется от IConfiguration:

//public interface IConfigurationRoot : IConfiguration
//{
//    IEnumerable<IConfigurationProvider> Providers { get; }
//    void Reload();
//}
//      Свойство Providers возвращает коллекцию применяемых провайдеров конфигурации. Каждый провайдер конфигурации представляет объект
//      IConfigurationProvider
//      Метод Reload() перезагружает значения из всех применяемых источников конфигурации

//  Итак, объект IConfiguration по сути хранит все конфигурационные настройки в виде набора пар "ключ"-"значение".
#endregion

#region Получение данных конфигурации
//  В приложении настройки конфигурации хранятся в свойстве Configuration объекта WebApplication. Соответственно через это свойство
//  мы можем установить или получить настройки конфигурации:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//// Установка настроек конфигурации
//app.Configuration["name"] = "Tom";
//app.Configuration["age"] = "37";

//app.Run(async context =>
//{
//    string name = app.Configuration["name"];
//    string age = app.Configuration["age"];
//    await context.Response.WriteAsync($"Name: {name} \nAge: {age}");
//});

//app.Run();

//  Для установки передаем значение по определенному ключу:
//  app.Configuration["name"] = "Tom";
//  В данном случае в конфигурацию устанавливается элемент с ключом "name". Он получает в качестве значения строку "Tom".
//  При этом неважно, что изначально в конфигурации нет настройки с именем "name". Если ее нет, она добавляется. Если она
//  уже существует, ее значение переустанавливается.

//  И затем мы можем получить нужную настройку по ключу:
//  string name = app.Configuration["name"];

//  Стоит обратить внимание, что в качестве значений передаются строки. Поэтому в случае с сохранением в конфигурации
//  условного возраста по ключу "age" в качестве значения также передается строка:
//  app.Configuration["age"] = "37";
#endregion

#region Добавление источника конфигурации
//  В примере выше настройки конфигурации устанавливались по отдельности - сначала настройка "name", затем настройка "age". Однако
//  если настроек много или если они имеют сложную структуру, гораздо проще установить их одним скопом, особенно в случае, когда
//  настройки хранятся в файле json, xml или берутся из какого-то другого источника конфигурации. Для добавления источника
//  конфигурации в приложении можно применять свойство Configuration объекта WebApplicationBuilder. Это свойство представляет
//  класс ConfigurationManager, для которого определен ряд методов для добавления конфигурации.

//var builder = WebApplication.CreateBuilder();

//builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
//{
//    { "name", "Tom" },
//    { "age", "37" }
//});

//var app = builder.Build();  

//app.Run(async context =>
//{
//    // получение настроек конфигураци
//    string name = app.Configuration["name"];
//    string age = app.Configuration["age"];
//    await context.Response.WriteAsync($"{name} - {age}");
//});

//app.Run();

//  Здесь для добавления конфигурации применяется метод AddInMemoryCollection(). Этот метод добавляет набор настроек в виде
//  коллекции пар ключ-значение:
//  public static IConfigurationBuilder AddInMemoryCollection(this IConfigurationBuilder configurationBuilder,
//          IEnumerable<KeyValuePair<string, string>> initialData)

//  Как раз таким набором является стандартный словарь Dictionary<string, string>

//  После добавления источника конфигурации мы также можем получить настройки конфигурации через свойство app.Configuration.
#endregion

#region Получение конфигурации через Dependancy Injection
//  Конфигурация приложения в виде объекта IConfiguration представляет один из сервисов, которые добавляются в приложение по умолчанию.
//  Соответственно всю конфигурацию приложения мы можем получить как и любой другой сервис через механизм внедрения зависимостей.
//  Например:

WebApplicationBuilder builder = WebApplication.CreateBuilder();

WebApplication app = builder.Build();

//  установка настроек конфигурации

app.Configuration["name"] = "Tom";
app.Configuration["age"] = "33";

//  через механизм внедрения зависимостей получим сервис IConfiguration
app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

app.Run();

//  В данном случае обработчик запроса в методе app.Map() в качестве параметра appConfig получает сервис IConfiguration - по сути
//  это тот же самый объект IConfiguration, что и app.Configuration. Подобным способом мы можем получить конфигурацию в других
//  частях приложения, особенно там, где объект WebApplication нам недоступен.
#endregion