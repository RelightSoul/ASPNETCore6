//  Передача конфигурации через IOptions

//  Фреймворк ASP.NET Core реализует паттерн Options, который позволяет передавать конфигурацию не просто как набор настроек в виде
//  пар ключ-значение, а как объекты определенных классов.

//  Для применения этого паттерна в приложении у объекта IServiceCollection, который представляет коллекцию сервисов приложения,
//  определен метод Configure():

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          IConfiguration config) where TOptions : class

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          IConfiguration config, Action<BinderOptions> configureBinder) where TOptions : class

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          string name, IConfiguration config) where TOptions : class

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          string name, IConfiguration config, Action<BinderOptions> configureBinder)

//  Этот метод реализован как метод расширения для типа IServiceCollection. И все версии метода типизируются типом, объект
//  которого надо передавать через механизм внедрения зависимостей. И также все версии метода принимают в качестве одного из
//  параметров объект конфигурации, на основе которой будет создаваться объект TOptions.

//  Допустим, у нас в проекте определен файл конфигурации person.json со следующим содержимым:

//{
//    "age": "37",
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

//  Данный файл по сути описывает одного пользователя. Элемент name сопоставляется с именем пользвателя, age - с возрастом,
//  languages представляет языки, которыми владеет пользователь, а элемент company - компания, в которой пользователь работает.
//  И мы хотим эти данные использовать как настройки в приложении и целостный объект. Для этого добавим вначале в проект класс Person:

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

//  Для представления компании пользователя определен дополнительный класс Company. Но, как можно заметить, определение класса
//  Person совпадает со структурой json-файла.

//  И чтобы передать конфигурационные настройки через объект Person, мы можем использовать сервис IOptions<TOptions>:

//using Microsoft.Extensions.Options;

//var builder = WebApplication.CreateBuilder();
//builder.Configuration.AddJsonFile("person.json");
//// устанавливаем объект Person по настройкам из конфигурации
//builder.Services.Configure<Person>(builder.Configuration);

//var app = builder.Build();

//app.Map("/", (IOptions<Person> options) =>
//{
//    Person person = options.Value;  // получаем переданные через Options объект Person
//    return person;
//});
//app.Run();

//  Прежде всего необходимо связать объект Person, который будет передаваться через механизм внедреия зависимостей, с конфигурацией
//  файла json. Для этого метод builder.Services.Configure() типизирует типом Person и в качестве параметра получает конфигурацию
//  приложения (свойство builder.Configuration реализует интерфейс IConfiguration и поэтому может передаваться в качестве параметра):
//      builder.Services.Configure<Person>(builder.Configuration);

//  Далее через механиз внедрения зависимостей мы можем получить созданный объект через сервис IOptions<Person>:
//      app.Map("/", (IOptions<Person> options) =>
//      {
//           Person person = options.Value;  // получаем переданные через Options объект Person
//           return person;
//      });

//  Причем через механизм DI передается не просто объект Person, а объект IOptions<Person>, из которого мы можем получим
//  непосредственно сам объект Person с помощью свойства Value.

//  Другой пример: определим в проекте новый класс middleware - PersonMiddleware, который фактически будет выводить информацию о
//  пользователе на веб-станицу:

using Microsoft.Extensions.Options;
 
var builder = WebApplication.CreateBuilder();
builder.Configuration.AddJsonFile("person.json");
builder.Services.Configure<Person>(builder.Configuration);
//builder.Services.Configure<Person>(opt => { opt.Age = 33; });

var app = builder.Build();

app.UseMiddleware<PersonMiddleware>();
app.Run();

public class PersonMiddleware
{
    private readonly RequestDelegate _next;
    public Person Person { get; }
    public PersonMiddleware(RequestDelegate next, IOptions<Person> options)
    {
        _next = next;
        Person = options.Value;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        System.Text.StringBuilder stringBuilder = new();
        stringBuilder.Append($"<p>Name: {Person.Name}</p>");
        stringBuilder.Append($"<p>Age: {Person.Age}</p>");
        stringBuilder.Append($"<p>Company: {Person.Company?.Title}</p>");
        stringBuilder.Append("<h3>Languages</h3><ul>");
        foreach (string lang in Person.Languages)
            stringBuilder.Append($"<li>{lang}</li>");
        stringBuilder.Append("</ul>");

        await context.Response.WriteAsync(stringBuilder.ToString());
    }
}

#region Настройка привязки конфгурации
//  При необходимости мы можем переопределить настройки с помощью перегрузки метода services.Configure():

//using Microsoft.Extensions.Options;

//var builder = WebApplication.CreateBuilder();
//builder.Configuration.AddJsonFile("person.json");
//builder.Services.Configure<Person>(builder.Configuration);
//builder.Services.Configure<Person>(opt =>
//{
//    opt.Age = 22;
//});

//var app = builder.Build();

//app.Map("/", (IOptions<Person> options) =>
//{
//    Person person = options.Value;  // получаем переданные через Options объект Person
//    return person;
//});
//app.Run();

//  Также можно передавать отдельные секции конфигурации. Например, передадим секцию Company:

//using Microsoft.Extensions.Options;
 
//var builder = WebApplication.CreateBuilder();
//builder.Configuration.AddJsonFile("person.json");
//builder.Services.Configure<Person>(builder.Configuration);
//builder.Services.Configure<Company>(builder.Configuration.GetSection("company"));

//var app = builder.Build();

//app.Map("/", (IOptions<Company> options) => options.Value);

//app.Run();
#endregion