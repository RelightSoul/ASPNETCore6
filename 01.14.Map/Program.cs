//  ����� Map

//  ����� Map() ����������� ��� �������� ����� ���������, ������� ����� ������������ ������ �� ������������� ����. ���� �����
//  ���������� ��� ����� ���������� ��� ���� IApplicationBuilder � ����� ��� ������������� ������. ��������:

//  public static IApplicationBuilder Map (this IApplicationBuilder app, string pathMatch, Action<IApplicationBuilder> configuration);

//  � �������� ��������� pathMatch ����� ��������� ���� �������, � ������� ����� �������������� �����. � �������� configuration
//  ������������ �������, � ������� ���������� ������ IApplicationBuilder � � ������� ����� ����������� ����� ���������.

//  ���������� ������� ������:

//WebApplicationBuilder builder = WebApplication.CreateBuilder();
//WebApplication app = builder.Build();

//app.Map("/time", appBuilder =>
//{
//    var time = DateTime.Now.ToShortTimeString();

//    // ��������� ������ - ������� �� ������� ����������
//    appBuilder.Use(async (context, next) =>
//    {
//        Console.WriteLine($"Time: {time}");
//        await next();  // �������� ��������� middleware
//    });

//    appBuilder.Run(async context =>
//    {
//        await context.Response.WriteAsync($"Time: {time}");
//    });
//});

//app.Run(async (context) => await context.Response.WriteAsync("Hello"));

//app.Run();

//  � ������ ������ ����� app.Map() ������� ����������� ���������, ������� ����� ������������ ������� �� ���� "/time":

//appBuilder =>
//{
//    var time = DateTime.Now.ToShortTimeString();
//    // ��������� ������ - ������� �� ������� ����������
//    appBuilder.Use(async (context, next) =>
//    {
//        Console.WriteLine($"Time: {time}");
//        await next();   // �������� ��������� middleware
//    });

//    appBuilder.Run(async context => await context.Response.WriteAsync($"Time: {time}"));
//}

//  ��������� ����� ��������� �������� ��� middleware, ������������ � ������� ������� Use() � Run(). ������� �������� �������
//  ����� � � ������ middleware ��������� ��� ����� �� �������. �� ������ - ������������ ���������� middleware ����������
//  ���������� � ������� � ����� �������.

//  ��� ������ ����� ��������, �������� �� "/time", ������ ����� �������������� �������� ������� ���������, ������� ������� �
//  ������ ������ �� ������ ����������:
//  app.Run(async (context) => await context.Response.WriteAsync("Hello"));

//  �������� ������� ����� ��������� ����� ��� ������ �����:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

//app.Map("/index", appBuild =>
//{
//    appBuild.Run(async context =>
//    {
//        await context.Response.WriteAsync("Index page");
//    });
//});

//app.Map("/about", appBuild =>
//{
//    appBuild.Run(async context =>
//    {
//        await context.Response.WriteAsync("About Page");
//    });
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Page not found");
//});

//app.Run();

//  ��� ������������� �������� ����� ��������� ����� ������� � ��������� ������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/index", Index);
//app.Map("/about", About);

//app.Run(async (context) => await context.Response.WriteAsync("Page Not Found"));

//app.Run();

void Index(IApplicationBuilder appBuild)
{
    appBuild.Run(async context =>
    {
        await context.Response.WriteAsync("Index Page");
    });
}

void About(IApplicationBuilder appBluid)
{
    appBluid.Run(async context =>
    {
        await context.Response.WriteAsync("About Page");
    });
}

#region ��������� ������ Map
//  ����� ���������, ������� ��������� � ������ Map(), ����� ����� ��������� �����, ������� ������������ ����������. ��������:

app.Map("/home", appBuilder =>
{
    appBuilder.Map("/index",Index);
    appBuilder.Map("/about", About);
    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Home Page");
    });
});

app.Run(async (context) => await context.Response.WriteAsync("Page Not Found"));

app.Run();

//  ����� ����� ��������� � ������� ������
//  app.Map("/home", appBuilder =>

//  ��� ����� ����� ������������ ������� �� ���� "/home".

//  ������ ���� ����� ��������� ��� ��������� �����, ������� ����� ������������ ������� �� ����� ������������ ���� �������� �����.
//  �� ���� ������ ����� About ����� ������������ ������ �� ���� "/home/about", � �� "/about"
#endregion