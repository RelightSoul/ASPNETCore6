//  �������� ������������ ����� IOptions

//  ��������� ASP.NET Core ��������� ������� Options, ������� ��������� ���������� ������������ �� ������ ��� ����� �������� � ����
//  ��� ����-��������, � ��� ������� ������������ �������.

//  ��� ���������� ����� �������� � ���������� � ������� IServiceCollection, ������� ������������ ��������� �������� ����������,
//  ��������� ����� Configure():

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          IConfiguration config) where TOptions : class

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          IConfiguration config, Action<BinderOptions> configureBinder) where TOptions : class

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          string name, IConfiguration config) where TOptions : class

//  public static IServiceCollection Configure<TOptions>(this IServiceCollection services,
//          string name, IConfiguration config, Action<BinderOptions> configureBinder)

//  ���� ����� ���������� ��� ����� ���������� ��� ���� IServiceCollection. � ��� ������ ������ ������������ �����, ������
//  �������� ���� ���������� ����� �������� ��������� ������������. � ����� ��� ������ ������ ��������� � �������� ������ ��
//  ���������� ������ ������������, �� ������ ������� ����� ����������� ������ TOptions.

//  ��������, � ��� � ������� ��������� ���� ������������ person.json �� ��������� ����������:

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

//  ������ ���� �� ���� ��������� ������ ������������. ������� name �������������� � ������ �����������, age - � ���������,
//  languages ������������ �����, �������� ������� ������������, � ������� company - ��������, � ������� ������������ ��������.
//  � �� ����� ��� ������ ������������ ��� ��������� � ���������� � ��������� ������. ��� ����� ������� ������� � ������ ����� Person:

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

//  ��� ������������� �������� ������������ ��������� �������������� ����� Company. ��, ��� ����� ��������, ����������� ������
//  Person ��������� �� ���������� json-�����.

//  � ����� �������� ���������������� ��������� ����� ������ Person, �� ����� ������������ ������ IOptions<TOptions>:

//using Microsoft.Extensions.Options;

//var builder = WebApplication.CreateBuilder();
//builder.Configuration.AddJsonFile("person.json");
//// ������������� ������ Person �� ���������� �� ������������
//builder.Services.Configure<Person>(builder.Configuration);

//var app = builder.Build();

//app.Map("/", (IOptions<Person> options) =>
//{
//    Person person = options.Value;  // �������� ���������� ����� Options ������ Person
//    return person;
//});
//app.Run();

//  ������ ����� ���������� ������� ������ Person, ������� ����� ������������ ����� �������� �������� ������������, � �������������
//  ����� json. ��� ����� ����� builder.Services.Configure() ���������� ����� Person � � �������� ��������� �������� ������������
//  ���������� (�������� builder.Configuration ��������� ��������� IConfiguration � ������� ����� ������������ � �������� ���������):
//      builder.Services.Configure<Person>(builder.Configuration);

//  ����� ����� ������� ��������� ������������ �� ����� �������� ��������� ������ ����� ������ IOptions<Person>:
//      app.Map("/", (IOptions<Person> options) =>
//      {
//           Person person = options.Value;  // �������� ���������� ����� Options ������ Person
//           return person;
//      });

//  ������ ����� �������� DI ���������� �� ������ ������ Person, � ������ IOptions<Person>, �� �������� �� ����� �������
//  ��������������� ��� ������ Person � ������� �������� Value.

//  ������ ������: ��������� � ������� ����� ����� middleware - PersonMiddleware, ������� ���������� ����� �������� ���������� �
//  ������������ �� ���-�������:

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

#region ��������� �������� �����������
//  ��� ������������� �� ����� �������������� ��������� � ������� ���������� ������ services.Configure():

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
//    Person person = options.Value;  // �������� ���������� ����� Options ������ Person
//    return person;
//});
//app.Run();

//  ����� ����� ���������� ��������� ������ ������������. ��������, ��������� ������ Company:

//using Microsoft.Extensions.Options;
 
//var builder = WebApplication.CreateBuilder();
//builder.Configuration.AddJsonFile("person.json");
//builder.Services.Configure<Person>(builder.Configuration);
//builder.Services.Configure<Company>(builder.Configuration.GetSection("company"));

//var app = builder.Build();

//app.Map("/", (IOptions<Company> options) => options.Value);

//app.Run();
#endregion