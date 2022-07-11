//  ������

//  ������ ������������ ����� ��� ���������������� ��������, ����������� � ����� �������� � ������� ���������� �������. ������ �����
//  �������������� ��� ���������� �����-�� ��������� ������, ������� ������ ���� ��������, ���� ������������ �������� � �����������,
//  � �� ������� ����������� ��������.

//  ��� �������� ��������� ������ �� ������� ��������� ������� ��� ���-�������, ������� �������� � ���� � ������� ���������� ��� ����
//  �������� �� ������ �������� � ������� ���������� �������. �� ������� �������� ������������� ������ � �����. ���� �������������
//  ���������� �� ������ � ������ ��������. ������ ���������� ���� ������������� ��� ���������� ������ ������ �� ������. ��� ����
//  ��������� ������ ��� ���������� ������. �� ���� ������ �������� ����, ������� ����������� ��� ��� �������� ������, �� ��� ����
//  ��� ��������� ����� ������.

//  ������ ������ ������ ������ � ������� ������������� ���������� ������� ����� ���������� �������. �� ��������� ���� ����������
//  ����� 20 �������, ���� ��� ����� ����� ��������.

//  ������� ���������, ��� ������ ������ ���������� ��� ������ �������� � �� ����������� ����� ����������. �� ���� ��� �������
//  �������� �� ����� ���������� ����� ����������� ���� ����� ������.

//  ��� ������ �������� ������ ������� IDistributedCache, � ASP.NET Core ������������� ���������� ���������� IDistributedCache,
//  ������� �� ����� ������������. ��� ����� ��������� � ���������� ��������� ���:

//var builder = WebApplication.CreateBuilder();

//builder.Services.AddDistributedMemoryCache();// ��������� IDistributedMemoryCache
//builder.Services.AddSession(); // ��������� ������� ������

//var app = builder.Build();

//app.UseSession();  // ��������� middleware ��� ������ � ��������

//app.Run(async context => 
//{
//    if (context.Session.Keys.Contains("name"))
//    {
//        await context.Response.WriteAsync($"Hello {context.Session.GetString("name")}");
//    }
//    else
//    {
//        context.Session.SetString("name", "Tom");
//        await context.Response.WriteAsync("Hello World!");
//    }
//});

//app.Run();

//  ������� ��������� ����������� �������:
//      builder.Services.AddDistributedMemoryCache();// ��������� IDistributedMemoryCache
//      builder.Services.AddSession();  // ��������� ������� ������

//  ����� � ������� ������ UseSession() ���������� � �������� ��������� ������� middleware ��� ������ � ��������:

//app.UseSession();   // ��������� middleware ��� ������ � ��������

//app.Run(async (context) =>
//{
//    if (context.Session.Keys.Contains("name"))
//        await context.Response.WriteAsync($"Hello {context.Session.GetString("name")}!");
//    else
//    {
//        context.Session.SetString("name", "Tom");
//        await context.Response.WriteAsync("Hello World!");
//    }
//});

//  ���� �� ����� �� ���������� app.UseSession() ��� ��������� ���������� � ������ �� ���������� ����� ������, �� �������
//  ���������� InvalidOperationException.

//  ����� ������ app.UseSession() �� ������ ��������� �������� ����� �������� HttpContext.Session, ������� ������������ ������
//  ���������� ISession. � ������ ������ �� ���������, ��������� �� � ������ ���� "name". ���� ���� ���������, �� �������� �����
//  �������� �� ����� �����. ���� ���� �� ���������, ������������� ���.

#region ���������� �������
//  ������ ISession ���������� ��� ������� � �������, ������� �� ����� ������������:
//      Keys: ��������, �������������� ������ �����, ������� ������ ��� ��������� �����
//      Clear(): ������� ������
//      Get(string key): �������� �� ����� key ��������, ������� ������������ ������ ������
//      GetInt32(string key): �������� �� ����� key ��������, ������� ������������ ������������� ��������
//      GetString(string key): �������� �� ����� key ��������, ������� ������������ ������
//      Set(string key, byte[] value): ������������� �� ����� key ��������, ������� ������������ ������ ������
//      SetInt32(string key, int value): ������������� �� ����� key ��������, ������� ������������ ������������� �������� value
//      SetString(string key, string value): ������������� �� ����� key ��������, ������� ������������ ������ value
//      Remove(string key): ������� �������� �� �����
#endregion

#region ��������� ������
//  ��� ������������� ������ ��� ��� ��������������� �������������. ������ ������ ����� ���� �������������, ������� ����������� � �����.
//  �� ��������� ��� ���� ����� �������� ".AspNet.Session". � ����� �� ��������� ���� ����� ��������� CookieHttpOnly=true, �������
//  ��� �� �������� ��� ���������� �������� �� ��������. �� �� ����� �������������� ��� �������� ������ � ������� ������� �������
//  SessionOptions:
//      Cookie.Name: ��� ����
//      Cookie.Domain: �����, ��� �������� �������������� ����
//      Cookie.HttpOnly: �������� �� ���� ������ ��� �������� ����� HTTP-������
//      Cookie.Path: ����, ������� ������������ ������
//      Cookie.Expiration: ����� �������� ���� � ���� ������� System.TimeSpan
//      Cookie.IsEssential: ��� �������� true ���������, ��� ���� �������� � ���������� ��� ������ ����� ����������
//      IdleTimeout: ����� �������� ������ � ���� ������� System.TimeSpan ��� ������������ ������������. ��� ������ ����� �������
//      ������� ������������. ���� �������� �� ������� �� Cookie.Expiration.

//��� ������������� ���� ������� ������� ��������� ��������:

//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = ".Myapp.Session";
//    options.IdleTimeout = TimeSpan.FromSeconds(60);
//    options.Cookie.IsEssential = true;
//});
#endregion

#region �������� ������� ��������
//  � ������ ���� � ������� ��������� ������� ������. ���� �� ���� ��������� �����-�� ������� ������, �� ��� ���� ������������� �
//  ������, � ��� ��������� �� ������ - ������� ���������������. ��� �������, ��� ����� ������������ ������ ���������� ��� �������
//  ISession. � ���������, ������� ��������� �����:

using System.Text.Json;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize<T>(value));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
    }
}
//  ����� Set ��������� � ������ ������, � ����� Get ��������� �� �� ������.

//  ��������, � ��� ���� ����� Person:
class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

//  � ���������� �������� ������������ � ���������������� ������� � ������:

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();

        var app = builder.Build();

        app.UseSession();

        app.Run(async (context) =>
        {
            if (context.Session.Keys.Contains("person"))
            {
                Person? person = context.Session.Get<Person>("person");
                await context.Response.WriteAsync($"Hello {person?.Name}, your age: {person?.Age}!");
            }
            else
            {
                Person person = new Person { Name = "Tom", Age = 22 };
                context.Session.Set<Person>("person", person);
                await context.Response.WriteAsync("Hello World!");
            }
        });

        app.Run();
    }
}
#endregion