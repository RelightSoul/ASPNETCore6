//  Отправка и получение json
//  JSON является распространенным форматом для передачи данных. Рассмотрим, как мы можем посылать и получить данные json.

#region Отправка JSON. Метод WriteAsJsonAsync
//  Для отправки json можно воспользоваться методом WriteAsJson()/ WriteAsJsonAsync() объекта HttpResponse.
//  Этот метод позволяет сериализовать переданные в него объекты в формат JSON и автоматически для заголовка
//  "content-type" устанавливает значение "application/json; charset=utf-8":

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    Person tom = new Person("Tom", 22);
//    await context.Response.WriteAsJsonAsync(tom);
//});
//app.Run();

//Можно было бы воспользоваться и стандартным методом WriteAsync():
//app.Run(async (context) =>
//{
//    var response = context.Response;
//    response.Headers.ContentType = "application/json; charset=utf-8";
//    await response.WriteAsync("{'name':'Tom', 'age':37}");
//});
#endregion

#region Получение JSON. Метод ReadFromJsonAsync
//  Для получения из запроса объект в формате JSON в классе HttpRequest определен метод ReadFromJsonAsync(). Он
//  позволяет сериализовать данные в объект определенного типа.

//  Cоздадим в проекте папку html, в которой определим новый файл index.html и  определим следующий код

//< !DOCTYPE html >
//< html >
//< head >
//    < meta charset = "utf-8" />
//    < title > METANIT.COM </ title >
//</ head >
//< body >
//    < h2 > User form </ h2 >
//    < div id = "message" ></ div >
//    < div >
//        < p > Name: < br />
//            < input name = "userName" id = "userName" />
//        </ p >
//        < p > Age: < br />
//            < input name = "userAge" id = "userAge" type = "number" />
//        </ p >
//        < button id = "sendBtn" > Send </ button >
//    </ div >
//    < script >
//        document.getElementById("sendBtn").addEventListener("click", send);
//async function send()
//{
//    const response = await fetch("/api/user", {
//    method: "POST",
//                headers: { "Accept": "application/json", "Content-Type": "application/json" },
//                body: JSON.stringify({
//name: document.getElementById("userName").value,
//                    age: document.getElementById("userAge").value
//                })
//            });
//const message = await response.json();
//document.getElementById("message").innerText = message.text;
//        }
//    </ script >
//</ body >
//</ html >

//  Здесь по нажатию на кнопку с помощью функции fetch() по адресу "/api/user" будет отправляться объект со свойствами
//  name и age, значения для которых берутся из полей формы. В ответ от сервера веб-страница также получает объект в
//  формате json, в котором имеется свойство text - свойство, которое хранит сообщение от сервера.

//  Теперь в файле Program.cs определим код для получения данных, отправляемых веб-страницей:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    var response = context.Response;
//    var request = context.Request;
//    if (request.Path == "/api/user")
//    {
//        string message = "Некорректные данные"; // содержание сообщения по умолчанию
//        try
//        {
//            Person? person = await request.ReadFromJsonAsync<Person>();  // пытаемся получить данные json
//            if (person != null)   // если данные сконвертированы в Person
//            {
//                message = $"Name: {person.Name}, Age: {person.Age}";
//            }
//        }
//        catch { }
//        // отправляем пользователю данные
//        await response.WriteAsJsonAsync(new { text = message });
//    }
//    else
//    {
//        response.ContentType = "text/html; charset=utf-8";
//        await response.SendFileAsync("html/index.html");
//    }

//});
//app.Run();

//  В данном случае, если обращение идет по адресу "/api/user", то получаем данные в формате json. При обращениях по
//  другим адресам просто посылаем веб-страницу index.html.

//  Метода ReadFromJsonAsync() десериализует полученные данные в объект определенного типа - в данном случае типа Person:
//  var person = await request.ReadFromJsonAsync<Person>();
//  if (person != null) // если данные сконвертированы в Person
//       message = $"Name: {person.Name}  Age: {person.Age}";
//  Таким образом, здесь результат вызова этого метода - значение переменной person будет представлять объект Person.

//  Но стоит отметить, что если данные запроса не представляют объект JSON, либо если метод ReadFromJsonAsync() не смо
//  г связать данные запроса со свойствами класса Person, то вызов этого метода сгенерирует исключение. Поэтому в данном
//  случае вызов метода помещается в конструкцию try..catch. Однако нельзя не отметить, что try..catch здесь является
//  узким местом, и далее мы посмотрим, как от него избавиться.

//  И в конце в ответ посылаем анонимный объект, который также сериализуется в json с некоторым сообщением, которое
//  хранится в свойстве text. При получении этого сообщения оно выводится на веб-страницу.

//  Стоит отметить, что проверять на наличие json в запросе можно с помощью метода HasJsonContentType() - он возвращает
//  true, если клиент прислал json.
//  if (request.HasJsonContentType())
//  {
//      var person = await request.ReadFromJsonAsync<Person>();
//      if (person != null)
//           responseText = $"Name: {person.Name}  Age: {person.Age}";
//  }
#endregion

#region Настройка сериализации
//  При получении данных в формате json мы можем столкнуться с рядом проблем. Хотя бы взять предыдущий пример, где мы
//  вынуждены были помещать вызов метода ReadFromJsonAsync в конструкцию - try..catch. Например, если мы не введем в
//  поля формы никаких значений, то стандартный механизм привязки значений не сможет связать данные запроса со свойством
//  Age. И мы получим исключение.

//  Одним из решений подобных проблем также может быть настройка сериализации/десериализации с помощью параметра типа
//  JsonSerializerOptions, которое может передаваться в метод ReadFromJsonAsync()

using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async context =>
{
    var response = context.Response;
    var request = context.Request;
    if (request.Path == "/api/user")
    {
        var responseText = "Некорректные данные";    // содержание сообщения по умолчанию
        if (request.HasJsonContentType())
        {
            // определяем параметры сериализации/десериализации
            var jsonoptions = new JsonSerializerOptions();
            // добавляем конвертер кода json в объект типа Person
            jsonoptions.Converters.Add(new PersonConverter());
            // десериализуем данные с помощью конвертера PersonConverter
            var person = await request.ReadFromJsonAsync<Person>(jsonoptions);
            if (person != null)
            {
                responseText = $"Name: {person.Name}, Age: {person.Age}";
            }
        }
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});
app.Run();

public record Person(string Name, int Age);
public class PersonConverter : JsonConverter<Person>
{
    public override Person? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var personName = "Undefined";
        var personAge = 0;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    // если свойство Age/age и оно содержит число
                    case "age" or "Age" when reader.TokenType == JsonTokenType.Number:
                        personAge = reader.GetInt32();
                        break;
                    // если свойство Age/age и оно содержит строку
                    case "age" or "Age" when reader.TokenType == JsonTokenType.String:
                        string? strValue = reader.GetString();
                        // пытаемся конвертировать строку в число
                        if (int.TryParse(strValue, out int value))
                        {
                            personAge = value;
                        }
                        break;
                    // если свойство Name/name
                    case "name" or "Name":
                        string? name = reader.GetString();
                        if (name!=null)
                        {
                            personName = name;
                        }
                        break;
                }
            }
        }
        return new Person(personName, personAge);
    }
    // сериализуем объект Person в json
    public override void Write(Utf8JsonWriter writer, Person person, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", person.Name);
        writer.WriteNumber("age", person.Age);

        writer.WriteEndObject();
    }
}
#endregion

#region Определение конвертера для сериализации/десериализации объекта в json
//  Класс конвертера для сериализации/десериализации объекта определенного типа в JSON должен наследоваться от
//  класса JsonConverter<T>. Абстрактный класс JsonConverter типизируется типом, для объекта которого надо выполнить
//  сериализацию/десериализацию. В коде выше такой реализацией является класс PersonConverter.

//  При наследовании класса JsonConverter необходимо реализовать его абстрактные методы Read() (выполняет десериализацию
//  из JSON в Person) и Write() (выполняет сериализацию из Person в JSON).

//  Метод Write, который записывает данные Person в формат JSON, выглядит относительно просто:
//public override void Write(Utf8JsonWriter writer, Person person, JsonSerializerOptions options)

//{
//    writer.WriteStartObject();
//    writer.WriteString("name", person.Name);
//    writer.WriteNumber("age", person.Age);
//    writer.WriteEndObject();
//}

//  Он принимает три параметра:
//  Utf8JsonWriter - объект, который записывает данные в json
//  Person - объект, который надо сериализовать
//  JsonSerializerOptions - дополнительные параметры сериализации

//  Сначала с помощью объекта Utf8JsonWriter открываем запись объекта в формате json:
//  writer.WriteStartObject();

//  Последовательно записываем данные объекта Person:
//  writer.WriteString("name", person.Name);
//  writer.WriteNumber("age", person.Age);

//  И завершаем запись объекта:
//  writer.WriteEndObject();

//  Стоит отметить, что в коде выше сереализация из Person в JSON не применяется.

//  Чтение или десериализация выглядит несколько сложнее. Метод Read() также принимает три параметра:
//  Utf8JsonReader - объект, который читает данные из json
//  Type - тип, в который надо выполнить конвертацию
//  JsonSerializerOptions - дополнительные параметры сериализации

//  Результатом метода Read() должен быть десериализованный объект (в данном случае объект типа Person).

//  В начале определяем данные объекта Person по умолчанию, которые будут применяться, если в процессе десериализации
//  произойдут проблемы:
//  var personName = "Undefined";
//  var personAge = 0;

//  Далее в цикле считываем каждый токен в строке json с помощью метода Read() объекта Utf8JsonReader:
//  while (reader.Read())

//  Затем, если считанный токен представляет название свойства, то считываем его и считываем следующий токен:
//  if (reader.TokenType == JsonTokenType.PropertyName)
//  {
//    var propertyName = reader.GetString();
//    reader.Read();

//  После этого мы можем узнать, как называется свойство и какое значение оно имеет. Для этого применяем конструкцию switch:
//    switch (propertyName)
//    {
//  Например, мы ожидаем, что json будет содержать свойство с именем "Age" или "age", которое будет хранить некоторое число.
//  Для его получения применяем следующий блок case:
//  case "age" or "Age" when reader.TokenType == JsonTokenType.Number:
//    personAge = reader.GetInt32();
//    break;
//  То есть если свойство называется "Age" или "age" и представляет число(JsonTokenType.Number), то вызываем метод reader.GetInt32()

//  Но свойство "Age/age" также может содержать строку, например, "23".Такая строка может конвертироваться в число.И для подобного
//  случая добавляем дополнительный блок case:
//  case "age" or "Age" when reader.TokenType == JsonTokenType.String:
//    string? stringValue = reader.GetString();
//    if (int.TryParse(stringValue, out int value))
//    {
//        personAge = value;
//    }
//    break;

//  Подобным образом считываем из json значение для свойства Name:
//  case "Name" or "name":
//   string? name = reader.GetString();
//    if (name != null)
//        personName = name;

//  В конце полученными данными инициализируем объект Person и возвращаем его из метода:
//  return new Person(personName, personAge);

//  Таким образом, мы можем проверить, какие свойства имеет объект json, какие значения они несут и принять решения, передавать
//  эти значения в объект Person.И в данном случае, даже если в присланном json не будет нужных свойств, или свойство age будет
//  содержать строку, которая не конвертируется в число, объект Person все равно будет создан.

//Чтобы использовать конвертер json, его надо добавить в коллекцию конвертеров:
//  var jsonoptions = new JsonSerializerOptions();
//  jsonoptions.Converters.Add(new PersonConverter());
//  var person = await request.ReadFromJsonAsync<Person>(jsonoptions);
#endregion
