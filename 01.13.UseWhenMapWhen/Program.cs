//  �������� ����� ���������. UseWhen � MapWhen

#region UseWhen
//  ����� UseWhen() �� ��������� ���������� ������� ��������� ������� ����������� ��������� ��� ��������� �������:

//  public static IApplicationBuilder UseWhen (this IApplicationBuilder app, Func<HttpContext,bool> predicate,
//                                              Action<IApplicationBuilder> configuration);

//  ��� � Use(), ����� UseWhen() ���������� ��� ����� ���������� ��� ���� IApplicationBuilder.

//  � �������� ��������� �� ��������� ������� Func>HttpContext,bool> - ��������� �������, �������� ������ ��������������� ������.
//  � ���� ������� ���������� ������ HttpContext. � ������������ ����� ������ ���� ��� bool - ���� ������ ������������� �������,
//  �� ������������ true, ����� ����������� false.

//  ��������� �������� ������ - ������� Action<IApplicationBuilder> ������������ ��������� �������� ��� �������� IApplicationBuilder,
//  ������� ���������� � ������� � �������� ���������.

//  ���������� ��������� ������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(context => context.Request.Path == "/time", // ���� ���� ������� "/time"
//    appBuilder =>
//    {
//        // ��������� ������ - ������� �� ������� ����������
//        appBuilder.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"Time: {time}");
//            await next();
//        });
//        appBuilder.Run(async context => 
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await context.Response.WriteAsync($"Time: {time}");
//        });
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello man!=)");
//});

//app.Run();

//  � ������ ������ ����� app.UseWhen() � �������� ������� ��������� �������� ��������� �������:
//  context => context.Request.Path == "/time"

//  ������ �������� ���������� ��������, � ������� ��������� ����������� ���������:
//
//  appBuilder =>
//{
//    // ��������� ������ - ������� �� ������� ����������
//    appBuilder.Use(async (context, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        Console.WriteLine($"Time: {time}");
//        await next();   // �������� ��������� middleware
//    });

//    appBuilder.Run(async context =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        await context.Response.WriteAsync($"Time: {time}");
//    });
//}

//  � ������ �������� � �������� ��������� ������� ������������ ��� middleware - � ������� ������� Use() � Run(). � ������ middleware
//  ��������� ��� ����� �� ������� ����������. �� ������ - ������������ ���������� middleware ���������� ���������� � ������� �
//  ����� �������.

//  ���� �� ���������� � ���������� �� ����, ������� ���������� �� "/time", �� ������� � ������ UseWhen() �����, ������� �����������
//  ��������� �� �����������. � ����������� middleware �� ������ app.Run().   // Hello man!=)
//  ������ ���� �� ���������� �� ���� "/time", �� ������� � ������ app.UseWhen() ����� �������. �������������� ����� �����������
//  ����������� ���������, ������� ����� ������������ ������. � ����� �� ������� ����������, � ����� � �������� ����� ����������
//  ������� �����.

//  ����� ��������, ��� �������� ����� ���������� ���� ��� ��� ������� ����������. ��������, � ������� ���� �� �����, ��� ���������
//  ������� ������������ � ����� middleware �� ������������ �����. �� ��� �����, ���� ������� ��������� ������� �� ��� � �� �����������
//  � ������ middleware, ��������, ��������� �������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(
//    context => context.Request.Path == "/time", // ���� ���� ������� "/time"
//    appBuilder =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        // ��������� ������ - ������� �� ������� ����������
//        appBuilder.Use(async (context, next) =>
//        {
//            Console.WriteLine($"Time: {time}");
//            await next();   // �������� ��������� middleware
//        });

//        // ���������� �����
//        appBuilder.Run(async context =>
//        {
//            await context.Response.WriteAsync($"Time: {time}");
//        });
//    });

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello METANIT.COM");
//});

//app.Run();

//  � ���� ������ ����� ����� ��������������� ���� ��� - ��� ������� ���������� � �������� ����� � ��������. �������������� ���
//  ����������� �� ����, ������� ��� �� ����� ���������� � ���������� �� ���� "/time", �� ����� �������� ���� � �� �� �����.

//  � ������� ���� ����� ��������� ����������� ������������ �����������, ������� ��������� �������� � �������� ����� ��������� ��
//  �����������. ������ �� ����� ����� �������� ������ �� ��������� �� ����� � �������� ����� ���������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(
//    context => context.Request.Path == "/time",
//    appBuilder =>
//    {
//        appBuilder.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"Time: {time}");
//            await next();
//        });
//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hey =)");
//});
//app.Run();

//  � ������ ������, ���� ������ ���� �� ���� "/time", ������� ����������� ����� ��������� � �����������, ������� ��������� �����
//  �� �������. � ����� ����������� ���������� �� app.Run(), ������� ���������� ��������� "Hey =)"

//  ��� ������� ������������� ����� ����� ���� �� ������� �������� �� �������� ����� ��������� � ��������� �����:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseWhen(
//    context => context.Request.Path == "/time",
//    HandleTimeRequest);

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hey =)");
//});
//app.Run();

//void HandleTimeRequest(IApplicationBuilder appBuilder)
//{
//    appBuilder.Use(async (context, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        Console.WriteLine($"Current time: {time}");
//        await next();
//    });
//}
#endregion

#region MapWhen
//  ����� MapWhen(), ��� � ����� UseWhen(), �� ��������� ���������� ������� ��������� ������� ����������� ���������:

//  public static IApplicationBuilder MapWhen(this IApplicationBuilder app, Func<HttpContext, bool> predicate,
//                                              Action<IApplicationBuilder> configuration);

//  ����� MapWhen() ����� ���������� ��� ����� ���������� ��� ���� IApplicationBuilder, ��������� �� �� ���������, ��� � UseWhen(),
//  � �������� �� ������ ����������� �������:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.MapWhen(
    context => context.Request.Path == "/time",   // �������: ���� ���� ������� "/time"
    appBuilder => appBuilder.Run(async context =>
    {
        var time = DateTime.Now.ToShortTimeString();
        await context.Response.WriteAsync($"Current time: {time}");
    })
);

app.Run(async context =>
{
    await context.Response.WriteAsync("Hey all");
});

app.Run();

//  ����� ����� ��, ���� �������� ���� "/time", �� ����������� ����� ���������, ��������� ������� app.MapWhen(), � ������� �������
//  ������������ ������� �����. ���� ���� ������� ������, �� ������������� �������� ����� ���������, � ������� ������������
//  ���������  Hey all
#endregion