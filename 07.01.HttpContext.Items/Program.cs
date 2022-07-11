//  ��������� ����������.����.������
//  HttpContext.Items

//  ���������� ASP.NET Core ����� ������� ��������� ���������.��� ����� ���� ��� �����-�� ���������� ������, ��� � ������,
//  ������� ��������������� ��������� � ������� � ������������.� � ����������� �� ���� ������, ���������� ��������� �������
//  ��� �� ��������.

//  ���� �� ���������� �������� �������� ������ ������������ ��������� HttpContext.Items - ������ ���� IDictionary<object, object>.
//  ��� ��������� ������������� ��� ����� ������, ������� ��������������� ������� � ������� ��������. ����� ���������� ������� ���
//  ������ �� HttpContext.Items ���������. ������ ������ � ���� ��������� ����� ���� � ��������.� � ������� ������ ����� ���������
//  ��������� ���������.

//  � ����� ������ �� ����� ��������� ������ ���������? ��������, ���� � ��� ��������� ������� ��������� ��������� �����������
//  middleware, � �� �����, ����� ��� ���� ����������� ���� �������� ����� ������, �� ��� ��� ����� ��������� ��� ���������. ��������,
//  ����� � ���������� ��������� ��������� ���:   

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    context.Items["text"] = "Hello from HttpContext.Items";
//    await next.Invoke();
//});

//app.Run(async context => 
//{
//    await context.Response.WriteAsync($"Text: {context.Items["text"]}");
//});

//app.Run();

//  ����� � ����� middleware ����������� ���� "text" �� ��������� "Hello from HttpContext.Items":
//      context.Items["text"] = "Hello from HttpContext.Items";
//  � ������ middleware ���� ������ ������������ ��� ��������� ������������� ������.

//  HttpContext.Items ������������� ��� ������� ��� ���������� ����������:
//      void Add(object key, object value): ��������� ������ value � ������ key
//      void Clear(): ������� ��� �������
//      bool ContainsKey(object key): ���������� true, ���� ������� �������� ������ � ������ key
//      bool Remove(object key): ������� ������ � ������ key, � ������ �������� �������� ���������� true
//      bool TryGetValue(object key, out object value): ���������� true, ���� �������� ������� � ������ key ������� �������� �
//      ������ value

//  ���������� ��������� �������:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Items.Add("ninja", "(-_-) Ninja");
    await next.Invoke();
});

app.Run(async (context) => 
{
    if (context.Items.ContainsKey("ninja"))
    {
        await context.Response.WriteAsync($"Message: {context.Items["ninja"]}");
    }
    else
    {
        await context.Response.WriteAsync("Huhuhu, all ninja dead");
    }
});

app.Run();