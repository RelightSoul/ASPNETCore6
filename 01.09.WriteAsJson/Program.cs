//  �������� � ��������� json
//  JSON �������� ���������������� �������� ��� �������� ������. ����������, ��� �� ����� �������� � �������� ������ json.

#region �������� JSON. ����� WriteAsJsonAsync
//  ��� �������� json ����� ��������������� ������� WriteAsJson()/ WriteAsJsonAsync() ������� HttpResponse.
//  ���� ����� ��������� ������������� ���������� � ���� ������� � ������ JSON � ������������� ��� ���������
//  "content-type" ������������� �������� "application/json; charset=utf-8":

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Run(async context =>
//{
//    Person tom = new Person("Tom", 22);
//    await context.Response.WriteAsJsonAsync(tom);
//});
//app.Run();

//����� ���� �� ��������������� � ����������� ������� WriteAsync():
//app.Run(async (context) =>
//{
//    var response = context.Response;
//    response.Headers.ContentType = "application/json; charset=utf-8";
//    await response.WriteAsync("{'name':'Tom', 'age':37}");
//});
#endregion

#region ��������� JSON. ����� ReadFromJsonAsync
//  ��� ��������� �� ������� ������ � ������� JSON � ������ HttpRequest ��������� ����� ReadFromJsonAsync(). ��
//  ��������� ������������� ������ � ������ ������������� ����.

//  C������� � ������� ����� html, � ������� ��������� ����� ���� index.html �  ��������� ��������� ���

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

//  ����� �� ������� �� ������ � ������� ������� fetch() �� ������ "/api/user" ����� ������������ ������ �� ����������
//  name � age, �������� ��� ������� ������� �� ����� �����. � ����� �� ������� ���-�������� ����� �������� ������ �
//  ������� json, � ������� ������� �������� text - ��������, ������� ������ ��������� �� �������.

//  ������ � ����� Program.cs ��������� ��� ��� ��������� ������, ������������ ���-���������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Run(async context =>
//{
//    var response = context.Response;
//    var request = context.Request;
//    if (request.Path == "/api/user")
//    {
//        string message = "������������ ������"; // ���������� ��������� �� ���������
//        try
//        {
//            Person? person = await request.ReadFromJsonAsync<Person>();  // �������� �������� ������ json
//            if (person != null)   // ���� ������ ��������������� � Person
//            {
//                message = $"Name: {person.Name}, Age: {person.Age}";
//            }
//        }
//        catch { }
//        // ���������� ������������ ������
//        await response.WriteAsJsonAsync(new { text = message });
//    }
//    else
//    {
//        response.ContentType = "text/html; charset=utf-8";
//        await response.SendFileAsync("html/index.html");
//    }

//});
//app.Run();

//  � ������ ������, ���� ��������� ���� �� ������ "/api/user", �� �������� ������ � ������� json. ��� ���������� ��
//  ������ ������� ������ �������� ���-�������� index.html.

//  ������ ReadFromJsonAsync() ������������� ���������� ������ � ������ ������������� ���� - � ������ ������ ���� Person:
//  var person = await request.ReadFromJsonAsync<Person>();
//  if (person != null) // ���� ������ ��������������� � Person
//       message = $"Name: {person.Name}  Age: {person.Age}";
//  ����� �������, ����� ��������� ������ ����� ������ - �������� ���������� person ����� ������������ ������ Person.

//  �� ����� ��������, ��� ���� ������ ������� �� ������������ ������ JSON, ���� ���� ����� ReadFromJsonAsync() �� ���
//  � ������� ������ ������� �� ���������� ������ Person, �� ����� ����� ������ ����������� ����������. ������� � ������
//  ������ ����� ������ ���������� � ����������� try..catch. ������ ������ �� ��������, ��� try..catch ����� ��������
//  ����� ������, � ����� �� ���������, ��� �� ���� ����������.

//  � � ����� � ����� �������� ��������� ������, ������� ����� ������������� � json � ��������� ����������, �������
//  �������� � �������� text. ��� ��������� ����� ��������� ��� ��������� �� ���-��������.

//  ����� ��������, ��� ��������� �� ������� json � ������� ����� � ������� ������ HasJsonContentType() - �� ����������
//  true, ���� ������ ������� json.
//  if (request.HasJsonContentType())
//  {
//      var person = await request.ReadFromJsonAsync<Person>();
//      if (person != null)
//           responseText = $"Name: {person.Name}  Age: {person.Age}";
//  }
#endregion

#region ��������� ������������
//  ��� ��������� ������ � ������� json �� ����� ����������� � ����� �������. ���� �� ����� ���������� ������, ��� ��
//  ��������� ���� �������� ����� ������ ReadFromJsonAsync � ����������� - try..catch. ��������, ���� �� �� ������ �
//  ���� ����� ������� ��������, �� ����������� �������� �������� �������� �� ������ ������� ������ ������� �� ���������
//  Age. � �� ������� ����������.

//  ����� �� ������� �������� ������� ����� ����� ���� ��������� ������������/�������������� � ������� ��������� ����
//  JsonSerializerOptions, ������� ����� ������������ � ����� ReadFromJsonAsync()

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
        var responseText = "������������ ������";    // ���������� ��������� �� ���������
        if (request.HasJsonContentType())
        {
            // ���������� ��������� ������������/��������������
            var jsonoptions = new JsonSerializerOptions();
            // ��������� ��������� ���� json � ������ ���� Person
            jsonoptions.Converters.Add(new PersonConverter());
            // ������������� ������ � ������� ���������� PersonConverter
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
                    // ���� �������� Age/age � ��� �������� �����
                    case "age" or "Age" when reader.TokenType == JsonTokenType.Number:
                        personAge = reader.GetInt32();
                        break;
                    // ���� �������� Age/age � ��� �������� ������
                    case "age" or "Age" when reader.TokenType == JsonTokenType.String:
                        string? strValue = reader.GetString();
                        // �������� �������������� ������ � �����
                        if (int.TryParse(strValue, out int value))
                        {
                            personAge = value;
                        }
                        break;
                    // ���� �������� Name/name
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
    // ����������� ������ Person � json
    public override void Write(Utf8JsonWriter writer, Person person, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", person.Name);
        writer.WriteNumber("age", person.Age);

        writer.WriteEndObject();
    }
}
#endregion

#region ����������� ���������� ��� ������������/�������������� ������� � json
//  ����� ���������� ��� ������������/�������������� ������� ������������� ���� � JSON ������ ������������� ��
//  ������ JsonConverter<T>. ����������� ����� JsonConverter ������������ �����, ��� ������� �������� ���� ���������
//  ������������/��������������. � ���� ���� ����� ����������� �������� ����� PersonConverter.

//  ��� ������������ ������ JsonConverter ���������� ����������� ��� ����������� ������ Read() (��������� ��������������
//  �� JSON � Person) � Write() (��������� ������������ �� Person � JSON).

//  ����� Write, ������� ���������� ������ Person � ������ JSON, �������� ������������ ������:
//public override void Write(Utf8JsonWriter writer, Person person, JsonSerializerOptions options)

//{
//    writer.WriteStartObject();
//    writer.WriteString("name", person.Name);
//    writer.WriteNumber("age", person.Age);
//    writer.WriteEndObject();
//}

//  �� ��������� ��� ���������:
//  Utf8JsonWriter - ������, ������� ���������� ������ � json
//  Person - ������, ������� ���� �������������
//  JsonSerializerOptions - �������������� ��������� ������������

//  ������� � ������� ������� Utf8JsonWriter ��������� ������ ������� � ������� json:
//  writer.WriteStartObject();

//  ��������������� ���������� ������ ������� Person:
//  writer.WriteString("name", person.Name);
//  writer.WriteNumber("age", person.Age);

//  � ��������� ������ �������:
//  writer.WriteEndObject();

//  ����� ��������, ��� � ���� ���� ������������ �� Person � JSON �� �����������.

//  ������ ��� �������������� �������� ��������� �������. ����� Read() ����� ��������� ��� ���������:
//  Utf8JsonReader - ������, ������� ������ ������ �� json
//  Type - ���, � ������� ���� ��������� �����������
//  JsonSerializerOptions - �������������� ��������� ������������

//  ����������� ������ Read() ������ ���� ����������������� ������ (� ������ ������ ������ ���� Person).

//  � ������ ���������� ������ ������� Person �� ���������, ������� ����� �����������, ���� � �������� ��������������
//  ���������� ��������:
//  var personName = "Undefined";
//  var personAge = 0;

//  ����� � ����� ��������� ������ ����� � ������ json � ������� ������ Read() ������� Utf8JsonReader:
//  while (reader.Read())

//  �����, ���� ��������� ����� ������������ �������� ��������, �� ��������� ��� � ��������� ��������� �����:
//  if (reader.TokenType == JsonTokenType.PropertyName)
//  {
//    var propertyName = reader.GetString();
//    reader.Read();

//  ����� ����� �� ����� ������, ��� ���������� �������� � ����� �������� ��� �����. ��� ����� ��������� ����������� switch:
//    switch (propertyName)
//    {
//  ��������, �� �������, ��� json ����� ��������� �������� � ������ "Age" ��� "age", ������� ����� ������� ��������� �����.
//  ��� ��� ��������� ��������� ��������� ���� case:
//  case "age" or "Age" when reader.TokenType == JsonTokenType.Number:
//    personAge = reader.GetInt32();
//    break;
//  �� ���� ���� �������� ���������� "Age" ��� "age" � ������������ �����(JsonTokenType.Number), �� �������� ����� reader.GetInt32()

//  �� �������� "Age/age" ����� ����� ��������� ������, ��������, "23".����� ������ ����� ���������������� � �����.� ��� ���������
//  ������ ��������� �������������� ���� case:
//  case "age" or "Age" when reader.TokenType == JsonTokenType.String:
//    string? stringValue = reader.GetString();
//    if (int.TryParse(stringValue, out int value))
//    {
//        personAge = value;
//    }
//    break;

//  �������� ������� ��������� �� json �������� ��� �������� Name:
//  case "Name" or "name":
//   string? name = reader.GetString();
//    if (name != null)
//        personName = name;

//  � ����� ����������� ������� �������������� ������ Person � ���������� ��� �� ������:
//  return new Person(personName, personAge);

//  ����� �������, �� ����� ���������, ����� �������� ����� ������ json, ����� �������� ��� ����� � ������� �������, ����������
//  ��� �������� � ������ Person.� � ������ ������, ���� ���� � ���������� json �� ����� ������ �������, ��� �������� age �����
//  ��������� ������, ������� �� �������������� � �����, ������ Person ��� ����� ����� ������.

//����� ������������ ��������� json, ��� ���� �������� � ��������� �����������:
//  var jsonoptions = new JsonSerializerOptions();
//  jsonoptions.Converters.Add(new PersonConverter());
//  var person = await request.ReadFromJsonAsync<Person>(jsonoptions);
#endregion
