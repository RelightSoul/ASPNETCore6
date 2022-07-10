//  Конфигурация в файлах JSON, XML и Ini

#region Конфигурация в JSON
//  Как правило, для хранения конфигурации в приложении ASP.NET Core используются файлы json. Для работы с файлами json применяется
//  провайдер JsonConfigurationProvider, а для загрузки конфигурации из json применяется метод расширения AddJsonFile().

//  По умолчанию в проекте уже есть файл конфигурации json - appsettings.json, а также appsettings.Development.json, которые
//  загружаются по умолчанию в приложении и которые мы можем использовать для хранения конфигурационных настроек.

//  Например, код файла appsettings.json:

//{
//    "Logging": {
//        "LogLevel": {
//            "Default": "Information",
//      "Microsoft.AspNetCore": "Warning"
//        }
//    },
//  "AllowedHosts": "*"
//}

//  Здесь определяются настройки логгирования (элемент "Logging") и разрешенные хосты (элемент "AllowedHosts"). Одни элементы могут
//  иметь вложенные элементы. Аналогичным образом можно задать другие необходимые настройки или удалить ранее определенные.

//  Однако в данном случае для примера возьмем новый файл json. Итак, добавим в проект новый файл config.json
//  И определим в нем следующее содержимое:

//{
//    "person": "Tom",
//    "company": "Microsoft"
//}
//  Здесь задается два элемента с ключами "person" и "company". Используем эти настройки в приложении:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

//app.Run();

//  Для установки конфигурации из json-файла название файла передается в метод AddJsonFile().

//  При определении настроек в файле json нам надо учитывать, что они должны иметь уникальные ключи. Но при этом мы можем
//  использовать для конфигурации более чем одного файла:

//builder.Configuration
//    .AddJsonFile("config.json")
//    .AddJsonFile("otherconfig.json");

//  И если во втором файле есть настройки, которые имеют тот же ключ, что и настройки первого файла, то происходит переопределение настроек: настройки из второго файла заменяют настройки первого.

//  Но json может хранить также более сложные по составу объекты, например:

//{
//    "person": { "profile": { "name" : "Tomas", "email" : "tom@gmail.com"} },
//    "company": { "name": "Mocrosoft"}
//}

//  И чтобы обратиться к этой настройке, нам надо использовать знак двоеточия для обращения к иерархии настроек:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) => 
//{
//    var personName = appConfig["person:profile:name"];
//    var companyName = appConfig["company:name"];
//    return $"{personName} - {companyName}";
//});

//app.Run();
#endregion

#region Конфигурация в XML
//  За использование конфигурации в XML-файле отвечает провайдер XmlConfigurationProvider. Для загрузки xml-файла применяется
//  метод расширения AddXmlFile().

//  Итак, добавим в проект новый xml-файл, который назовем config.xml. Затем изменим его код на следующий:

//<? xml version = "1.0" encoding = "utf-8" ?>
//< configuration >
//  < person > Tom </ person >
//  < company > Microsoft </ company >
//</ configuration >

//  Здесь определены два элемента person и company, которые представляют конфигурационные настройки.
//  Обратите внимание, что у файла xml в свойствах должно быть выставлено копирование при компиляции в выходную папку приложения

//  Теперь используем этот файл:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddXmlFile("config.xml");

//app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

//app.Run();

//Если у нас файл конфигурации имеет разные уровни вложенности, например:

//<? xml version = "1.0" encoding = "utf-8" ?>
//< configuration >
//  < person >
//      < profile >
//          < name > Tomas </ name >
//          < email > toma@gmail.com </ email >
//      </ profile >
//  </ person >
//  < company >
//      < name > Microsoft </ name >
//  </ company >
//</ configuration >

//  то мы можем обращаться к этим уровням также, как и в файле json:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddXmlFile("config.xml");

//app.Map("/", (IConfiguration appConfig) => 
//{
//    var name = appConfig["person:profile:name"];
//    var company = appConfig["company:name"];
//    return $"{name} - {company}";
//});

//app.Run();
#endregion

#region Конфигурация в ini-файлах
//  Для работы с конфигурацией INI применяется провайдер IniConfigurationProvider. А для загрузки конфигурации из INI-файла нам
//  надо использовать метод расширения AddIniFile().

//  Например, добавим в проект текстовый файл и переименуем его в config.ini.
//  Определим в этом файле следующее содержимое:
//      person = "Bob"
//      company = "Microsoft"

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddIniFile("config.ini");

app.Map("/", (IConfiguration appConfig) => $"{appConfig["person"]} - {appConfig["company"]}");

app.Run();
#endregion