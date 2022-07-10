//  Анализ конфигурации

//  Для работы с конфигурацией интерфейс IConfiguration определяет следующие методы:
//      GetSection(name): возвращает объект IConfiguration, который представляет только определенную секцию name
//      GetChildren(): возвращает все подсекции текущего объекта конфигурации в виде набора объектов IConfiguration
//      GetReloadToken(): возвращает токен - объект IChangeToken, который используется для уведомления при изменении конфигурации
//      GetConnectionString(name): эквивалентен вызову GetSection("ConnectionStrings")[name] и предназначается непосредственно для работы
//      со строками подключения к различным базам даных
//      [key]: индексатор, который позволяет получить по определенному ключу key хранящееся значение

//  Например, если у нас есть следующая конфигурация в файле config.json:

//{
//    "ConnectionStrings": {
//        "DefaultConnection": "Main database",
//    "UsersContext": "Users database"
//    }
//}

//  То мы можем с помощью метода GetSection() получить отдельные секции и их значения:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("config.json");

//app.Map("/", (IConfiguration appConfig) =>
//{
//    IConfigurationSection connStrings = appConfig.GetSection("ConnectionStrings");
//    string defaultConnection = connStrings.GetSection("DefaultConnection").Value;
//    return defaultConnection;
//});

//app.Run();

//  Каждая отдельная секция представляет объект IConfigurationSection. Если секция содержит другие секция, то также можем вызвать
//  у ней метод GetSection(). Если же секция содержит только значение, то оно доступно через свойство Value.

//  Также мы могли бы использовать один вызов GetSection(), передав ему полный путь к нужной секции:
//      string defaultConnection = appConfig.GetSection("ConnectionStrings:DefaultConnection").Value;
//  Вложенные секции от родительских отделяются двоеточием.

//  Также мы могли бы получить нужное значение, используя индексаторы:
//      string defaultConnection = appConfig["ConnectionStrings:DefaultConnection"];

//  Ну и кроме того, для работы непосредственно с секцией "ConnectionStrings" нам доступен метод GetConnectionString():
//      string con = appConfig.GetConnectionString("DefaultConnection");

//  Используя выше рассмотренные методы, мы можем провести анализ всего файла конфигурации. Например, пусть в проекте определен
//  следующий конфигурационный файл project.json:

//{
//    "projectConfig": {
//        "dependencies": {
//            "Microsoft.Extensions.Configuration": "1.0.0",
//      "Microsoft.Extensions.Configuration.Json": "1.0.0",
//      "Microsoft.NETCore.App": {
//                "version": "1.0.1",
//        "type": "platform"
//      },
//      "Microsoft.AspNetCore.Diagnostics": "1.0.0",

//      "Microsoft.AspNetCore.Server.IISIntegration": "1.0.0",
//      "Microsoft.AspNetCore.Server.Kestrel": "1.0.1",
//      "Microsoft.Extensions.Logging.Console": "1.0.0"
//        },

//    "tools": {
//            "Microsoft.AspNetCore.Server.IISIntegration.Tools": "1.0.0-preview2-final"
//    },

//    "frameworks": {
//            "netcoreapp1.0": {
//                "imports": [
//                  "dotnet5.6",
//          "portable-net45+win8"
//              ]
//      }
//        },

//    "buildOptions": {
//            "emitEntryPoint": true,
//      "preserveCompilationContext": true
//    },

//    "runtimeOptions": {
//            "configProperties": {
//                "System.GC.Server": true
//            }
//        },

//    "publishOptions": {
//            "include": [
//              "wwwroot",
//        "web.config"
//      ]
//    },

//    "scripts": {
//            "postpublish": [ "dotnet publish-iis --publish-folder %publish:OutputPath% --framework %publish:FullTargetFramework%" ]
//    }
//    }
//}

//  Проанализируем и выведем все его содержимое в браузере:

using System.Text;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddJsonFile("project.json");

app.Map("/", (IConfiguration appConfig) => GetSectionContent(appConfig.GetSection("projectConfig")));

app.Run();

string GetSectionContent(IConfiguration configSection)
{
    System.Text.StringBuilder contentBuilder = new StringBuilder();
    foreach (var section in configSection.GetChildren())
    {
        contentBuilder.Append($"\"{section.Key}\":");
        if (section.Value == null)
        {
            string subSectionContent = GetSectionContent(section);
            contentBuilder.Append($"{{\n{subSectionContent}}},\n");
        }
        else
        {
            contentBuilder.Append($"\"{section.Value}\",\n");
        }
    }

    return contentBuilder.ToString();
}