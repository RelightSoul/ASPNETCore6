//  Отправка текста и json в Results API

#region Отправка текста и метод Content
//  Метод Content() отправляет текстовое содержимое и позволяет при этом задать тип содержимого и кодировку. Одна из версий метода:
//  public static IResult Content(string content, string? contentType = default, System.Text.Encoding? contentEncoding = default);
//      Параметр content задает текстовое содержимое ответа
//      Параметр contentType задает MIME-тип ответа
//      Параметр contentEncoding задает кодировку

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => Results.Content("你好", "text/plain", System.Text.Encoding.Unicode));

//app.Run();

//  Если указан только первый параметр, то метод по умолчанию будет использовать в качестве типа содержимого "text/plain",
//  а в качестве кодировки "utf-8"

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => Results.Content("Hello ASP.NET core"));

//app.Run();
#endregion

#region Text
//  Метод Text() работает аналогичным образом, он также отравляет текст и принимает те же параметры:
//      public static IResult Text(string content, string? contentType = default, System.Text.Encoding? contentEncoding = default);

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/chinese", () => Results.Text("你好", "text/plain", System.Text.Encoding.Unicode));
//app.Map("/", () => Results.Text("Hello ASP.NET Core"));

//app.Run();
#endregion

#region Отправка json
//  Для отправки данных в формате json применяется метод Results.Json():
//      public static IResult Json (object? data, JsonSerializerOptions? options = default,
//                                  string? contentType = default, int? statusCode = default);

//  Параметры метода:
//      data: отправляемый объект
//      options: объект System.Text.Json.JsonSerializerOptions ?, который задает параметры сериализации
//      contentType: тип содержимого в виде строки. Если этот параметр не указан, то по умолчанию применяется тип
//      "application/json; charset=utf-8"
//      statusCode: отправляемый вместе с json код статуса. Если этот параметр не указан, то по умолчанию код статус - 200

//  Применение метода:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/person", () => Results.Json(new Person("Sam", 33)));      // отправка объекта Person
//app.Map("/", () => Results.Json(new { name = "Tome", age = 37 }));  // отправка анонимного объекта

//app.Run();

//record class Person(string Name, int Age);

// Если надо конкретизировать параметры сериализации в json, то можно использовать второй параметр метода, который представляет
// тип System.Text.Json.JsonSerializerOptions?:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/sam", () => Results.Json(new Person("Sam", 25),
//        new()
//        {
//            PropertyNameCaseInsensitive = false,
//            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.WriteAsString
//        }));

//app.Map("/bob", () => Results.Json(new Person("Bob", 41),
//        new(System.Text.Json.JsonSerializerDefaults.Web)));

//app.Map("/tom", () => Results.Json(new Person("Tom", 37),
//         new(System.Text.Json.JsonSerializerDefaults.General)));

//app.Run();

//record class Person(string Name, int Age);

//  Нередко формат json также применяется и для отправки ошибок. В этом случае мы можем задать статусный код ошибки с помощью
//  последнего параметра:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/error", () => Results.Json(new { message = "Unexpecter error" }, statusCode: 500));

app.Map("/", () => "Hello World");

app.Run();

//  В данном случае посылаем объект с информацией об ошибке и статусный код 500:
#endregion