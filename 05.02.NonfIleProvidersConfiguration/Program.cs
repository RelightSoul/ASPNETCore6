//  Нефайловые провайдеры конфигурации

#region Загрузка аргументов командной строки
//  Для загрузки аргументов командной строки в конфигурацию приложения применяется провайдер CommandLineConfigurationProvider.
//  Для применения этого провайдера у объекта ConfigurationManager (свойство Configuration объекта WebApplicationBuilder) вызывается
//  метод AddCommandLine(), в который передаются аргументы командной строки. Но поскольку приложение по умолчанию загружает аргументы
//  командной строки в объект конфигурации, который передается в классы приложения через dependency injection, то нам нет смысла
//  явным образом его вызывать.

//  Каждый аргумент командной строки, который мы хотим использовать в качестве параметра конфигурации, должен представлять пару
//  ключ-значение. Есть разные способы определения таких параметров. Самый распространненый - после ключа после знака равно =
//  указывается значение, а через пробел последующие параметры:
//      key1 = value1 key2 = value2 key3 = value3

//  Также можно использовать другие способы:
//      key1 = value1--key2 = value2 / key3 = value3
//      --key1 value1 /key2 value2
//      key1= key2=value2

//  Перед ключом можно указать префикс -- или /, тогда между ключом и значением можно опустить знак равно = и оставить просто пробел.

//  Также если мы хотим указать ключ, но не хотим указывать значение, то после знака равно можно не указывать значение.
#endregion

#region Тестирование в Visual Studio
//  Для тестирования передачи параметров командной строки изменим в проекте в папке Properties файл launchSettings.json.

//  По умолчанию он выглядит примерно следующим образом:

//{
//    "iisSettings": {
//        "windowsAuthentication": false,
//    "anonymousAuthentication": true,
//    "iisExpress": {
//            "applicationUrl": "http://localhost:51654",
//      "sslPort": 44377
//    }
//    },
//  "profiles": {
//        "_05._02.NonfIleProvidersConfiguration": {
//            "commandName": "Project",
//      "dotnetRunMessages": true,
//      "launchBrowser": true,
//      "applicationUrl": "https://localhost:7235;http://localhost:5235",
//      "environmentVariables": {
//                "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//        },
//    "IIS Express": {
//            "commandName": "IISExpress",
//      "launchBrowser": true,
//      "environmentVariables": {
//                "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//        }
//    }
//}

//  Перейдем в этом файле к элементу "profiles". Этот элемент по умолчанию содержит два профиля, которые могут применяться для запуска.
//  Один профиль называется по имени проекта (в моем случае HelloApp). Второй профиль называется "IIS". Выберем профиль, который мы
//  используем для запуска проекта и добавим в этот элемент строку
//      "commandLineArgs": "name=Bob age=37",

//  Параметр "commandLineArgs" позволяет определить данные командной строки, которые будут передаваться приложению. В данном случае
//  предполагается, что через командную строку будут передаваться параметр name со значением "Bob" и параметр age со значением "37".
//  Например, я запускаю проект как консольное приложение, поэтому я изменяю профиль по имени проекта - HelloApp. Который после
//  изменения будет выглядеть следующим образом:

//"profiles": {
//    "_05._02.NonfIleProvidersConfiguration": {
//        "commandName": "Project",
//      "dotnetRunMessages": true,
//      "launchBrowser": true,
//      "commandLineArgs": "name=Bob age=37",
//      "applicationUrl": "https://localhost:7235;http://localhost:5235",
//      "environmentVariables": {
//            "ASPNETCORE_ENVIRONMENT": "Development"
//      }
//    },

//  Определим в приложении простейший код для получения значений конфигурации:

//var builder = WebApplication.CreateBuilder(args);   //передаём аргументы
//var app = builder.Build();

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

//app.Run();

//  Здесь важно отметить передачу параметров командной строки - args в метод WebApplication.CreateBuilder(args). Благодаря этому
//  приложение автоматически может подхватить параметры командной строки. И после запуска мы увидим указанные значения параметров
//  конфигурации
#endregion

#region Запуск через консоль
//  Используя CLI, мы можем запустить приложение из командной строки и передать ему параметры.

//  Итак, удалим выше определенные параметры и выполним перестроение проекта. Откроем командную строку и перейдем в консоли с
//  помощью команды cd в папку проекта. В моем случае это папка
//  C: \Users\Vlad\source\repos\ASPNETCore6\05.02.NonfIleProvidersConfiguration

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

//app.Run();

//  Далее введем следующую команду:  // dotnet run name=Tom age=35
//  по адресу https://localhost:7235 в моём случае, будет Tom - 35
#endregion

#region Программная симуляция аргументов командной строки
//  Также мы можем на уровне кода симулировать передачу параметров командной строки:

//string[] commandLineArgs = { "name=Alice", "age=27" };  // псевдопараметры командной строки
//var builder = WebApplication.CreateBuilder(commandLineArgs);   //передаю параметры
//var app = builder.Build();

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["name"]} - {appConfig["age"]}");

//app.Run();
#endregion

#region Применение метода AddCommandLine
//  Также можно было бы передать параметры командной строки через метод AddCommandLine():

//var builder = WebApplication.CreateBuilder();

//string[] commandLineArgs = { "name=Vlad", "age=32" };   // псевдопараметры командной строки
//builder.Configuration.AddCommandLine(commandLineArgs);  // передаем параметры в качестве конфигурации

//var app = builder.Build();

//app.Map("/", (IConfiguration config) => $"{config["name"]} - {config["age"]}");

//app.Run();
#endregion

#region Переменные среды окружения как источник конфигурации
//  Для загрузки переменных среды окружения в качестве параметров конфигурации применяется провайдер
//  EnvironmentVariablesConfigurationProvider. Для его использования у объекта ConfigurationManager вызывается метод
//  AddEnvironmentVariables(). Однако в реальности вряд ли его придется часто использовать, так как среда ASP.NET Core уже
//  загружает переменные среды окружения в объект конфигурации по умолчанию.

//  Например, получим переменную окружения "JAVA_HOME", которая указывает на папку установки java sdk, если эта переменная определена:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", (IConfiguration config) => $" Java_home = {config["JAVA_HOME"] ?? "not set"}");

//app.Run();

//  Явное применение метода AddEnvironmentVariables():
//          builder.Configuration.AddEnvironmentVariables();
#endregion

#region Хранение конфигурации в памяти
//  Провайдер MemoryConfigurationProvider позволяет использовать в качестве конфигурации коллекцию IEnumerable<KeyValuePair<string,
//  string>>, которая хранит данные в виде пары ключ-значение (пример - объект Dictionary). Для добавления источника конфигурации
//  применяется метод AddInMemoryCollection(), в который передается словарь конфигурационных настроек:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    { "name", "Nelly"},
    { "age", "27"}
});

app.Map("/", (IConfiguration appConfig) => 
{
    var name = appConfig["name"];
    var age = appConfig["age"];
    return $"{name} - {age}";
});

app.Run();
#endregion