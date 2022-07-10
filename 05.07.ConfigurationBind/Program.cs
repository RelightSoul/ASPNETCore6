//  Проекция конфигурации на классы
//  Фреймворк ASP.NET Core позволяет проецировать конфигурационные настройки на классы C#.

//  Например, определим в проекте новый файл person.json, который будет хранить данные пользователя:
//{
//    "name": "Tom",
//    "age": "22"
//}

//  Под эти данные определим в проекте класс Person:
//public class Person
//{
//    public string Name { get; set; } = "";
//    public int Age { get; set; } = 0;
//}

//  Теперь свяжем конфигурацию из файла person.json с объектом класса Person:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("person.json");
//var tom = new Person();
//// связываем конфигурацию с объектом tom
//app.Configuration.Bind(tom);

//app.Run(async context => await context.Response.WriteAsync($"Name: {tom.Name}, Age: {tom.Age}")); ;

//app.Run();

//  Ключевой момент заключается в применении метода Bind:
//      var tom = new Person();
//      app.Configuration.Bind(tom);

//  Для объекта IConfiguration определен метод Bind(), который в качестве параметра принимает объект, который надо связать с данными.
//  Стоит отметить, что между конфигурацией в json и классом Person имеется соответствие по названию свойств, благодаря чему может
//  осуществляться связка (регистр в данном случае роли не играет).

//  В качестве альтернативы методу Bind мы могли бы использовать метод Get<T>(), который возвращает объект созданного класса:
//      Person tom = app.Configuration.Get<Person>();

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("person.json");

//app.Map("/", (IConfiguration appConfig) =>
//{
//    // связываем конфигурацию с объектом tom
//    var tom = appConfig.Get<Person>();
//    return $"{tom.Name} - {tom.Age}";
//});

//app.Run();

#region Привязка сложных объектов
//  Рассмотрим привязку более сложных по структуре данных. Определим следующий файл person.json:

//{
//    "age": "28",
//  "name": "Tom",
//  "languages": [
//    "English",
//    "German",
//    "Spanish"
//  ],
//  "company": {
//        "title": "Microsoft",
//    "country": "USA"
//  }
//}

//  Для представления этих данных в коде C# определим следующие классы:

//public class Person
//{
//    public string Name { get; set; } = "";
//    public int Age { get; set; }
//    public List<string> Languages { get; set; } = new();
//    public Company? Company { get; set; }
//}
//public class Company
//{
//    public string Title { get; set; } = "";
//    public string Country { get; set; } = "";
//}

//  Теперь выполним в приложении привязку из конфигурации json в объекты классов C#:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("person.json");

//var tom = new Person();
//app.Configuration.Bind(tom);

//app.Run(async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    string name = $"<p>Name: {tom.Name}</p>";
//    string age = $"<p>Age: {tom.Age}</p>";
//    string company = $"<p>Company: {tom.Company?.Title}</p>";
//    string langs = "<p>Languages:</p><ul>";
//    foreach (var lang in tom.Languages)
//    {
//        langs += $"<li><p>{lang}</p></li>";
//    }
//    langs += "</ul>";

//    await context.Response.WriteAsync($"{name}{age}{company}{langs}");
//});

//app.Run();
#endregion

#region Привязка конфигурации из xml
//  Возьмем выше определенные классы Person и Company. И добавим в проект файл person.xml, который будет содержать аналогичные данные:

//<? xml version = "1.0" encoding = "utf-8" ?>
//< person >
//  < name > Tom </ name >
//  < age > 35 </ age >
//  < languages name = "0" > English </ languages >
//  < languages name = "1" > German </ languages >
//  < languages name = "2" > Chinese </ languages >
//  < company >
//    < title > Microsoft </ title >
//    < country > USA </ country >
//  </ company >
//</ person >

//  Обратите внимание на установку в файле xml массивов - они имеют атрибут name, который определяет условный индекс.

//  Применим конфигурацию из выше определенного файла xml в приложении:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddXmlFile("person.xml");

//var tom = new Person();
//app.Configuration.Bind(tom);

//app.Run(async context =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    string name = $"<p>Name: {tom.Name}</p>";
//    string age = $"<p>Age: {tom.Age}</p>";
//    string company = $"<p>Company: {tom.Company?.Title}</p>";
//    string langs = "<p>Languages:</p><ul>";
//    foreach (var lang in tom.Languages)
//    {
//        langs += $"<li><p>{lang}</p></li>";
//    }
//    langs += "</ul>";

//    await context.Response.WriteAsync($"{name}{age}{company}{langs}");
//});

//app.Run();

//  Как можно заметить из кода, меняется только подключение файла с json на xml, а весь остальной код остается прежним.
#endregion

#region Привязка секций конфигурации
//  В примерах выше выполнялась привязка корневого объекта конфигурации, однако также можно осуществлять привязку отдельных секций.
//  Например, выше в файле json и xml была определена секция company, которая хранит компанию пользователя.
//  Выполним привязку отдельно этой секции к объекту класса Company:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddJsonFile("person.json");

Company company = app.Configuration.GetSection("company").Get<Company>();
//var company = new Company();
//app.Configuration.GetSection("company").Bind(company);

app.Run(async context => 
{
    await context.Response.WriteAsync($"{company.Title} - {company.Country}");
});

app.Run();

//  С помощью метода GetSection() получаем нужную нам секцию конфигурации и затем также можно вызвать методы Bind или Get
//  и выполнить привязку.
#endregion