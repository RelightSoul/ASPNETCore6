//  �������� ������������ �� ������
//  ��������� ASP.NET Core ��������� ������������ ���������������� ��������� �� ������ C#.

//  ��������, ��������� � ������� ����� ���� person.json, ������� ����� ������� ������ ������������:
//{
//    "name": "Tom",
//    "age": "22"
//}

//  ��� ��� ������ ��������� � ������� ����� Person:
//public class Person
//{
//    public string Name { get; set; } = "";
//    public int Age { get; set; } = 0;
//}

//  ������ ������ ������������ �� ����� person.json � �������� ������ Person:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("person.json");
//var tom = new Person();
//// ��������� ������������ � �������� tom
//app.Configuration.Bind(tom);

//app.Run(async context => await context.Response.WriteAsync($"Name: {tom.Name}, Age: {tom.Age}")); ;

//app.Run();

//  �������� ������ ����������� � ���������� ������ Bind:
//      var tom = new Person();
//      app.Configuration.Bind(tom);

//  ��� ������� IConfiguration ��������� ����� Bind(), ������� � �������� ��������� ��������� ������, ������� ���� ������� � �������.
//  ����� ��������, ��� ����� ������������� � json � ������� Person ������� ������������ �� �������� �������, ��������� ���� �����
//  �������������� ������ (������� � ������ ������ ���� �� ������).

//  � �������� ������������ ������ Bind �� ����� �� ������������ ����� Get<T>(), ������� ���������� ������ ���������� ������:
//      Person tom = app.Configuration.Get<Person>();

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//builder.Configuration.AddJsonFile("person.json");

//app.Map("/", (IConfiguration appConfig) =>
//{
//    // ��������� ������������ � �������� tom
//    var tom = appConfig.Get<Person>();
//    return $"{tom.Name} - {tom.Age}";
//});

//app.Run();

#region �������� ������� ��������
//  ���������� �������� ����� ������� �� ��������� ������. ��������� ��������� ���� person.json:

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

//  ��� ������������� ���� ������ � ���� C# ��������� ��������� ������:

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

//  ������ �������� � ���������� �������� �� ������������ json � ������� ������� C#:

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

#region �������� ������������ �� xml
//  ������� ���� ������������ ������ Person � Company. � ������� � ������ ���� person.xml, ������� ����� ��������� ����������� ������:

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

//  �������� �������� �� ��������� � ����� xml �������� - ��� ����� ������� name, ������� ���������� �������� ������.

//  �������� ������������ �� ���� ������������� ����� xml � ����������:

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

//  ��� ����� �������� �� ����, �������� ������ ����������� ����� � json �� xml, � ���� ��������� ��� �������� �������.
#endregion

#region �������� ������ ������������
//  � �������� ���� ����������� �������� ��������� ������� ������������, ������ ����� ����� ������������ �������� ��������� ������.
//  ��������, ���� � ����� json � xml ���� ���������� ������ company, ������� ������ �������� ������������.
//  �������� �������� �������� ���� ������ � ������� ������ Company:

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

//  � ������� ������ GetSection() �������� ������ ��� ������ ������������ � ����� ����� ����� ������� ������ Bind ��� Get
//  � ��������� ��������.
#endregion