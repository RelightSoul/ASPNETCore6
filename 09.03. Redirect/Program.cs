//  Переадресация в Results API

#region LocalRedirect
//  Для переадресации на локальный адрес в рамках приложения применяется метод LocalRedirect():
//      public static IResult LocalRedirect (string localUrl, bool permanent = false, bool preserveMethod = false);

//  Параметры метода:
//      localUrl: локальный адрес для переадресации
//      permanent: указывает, будет ли переадресация постоянной (отправляется статусный код 301) или временной(отравляется статусный
//      код 302).
//      preserveMethod: если равен true, то сохраняется оригинальный метод HTTP-запроса.

//  Применение метода:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/old", () => Results.LocalRedirect("/new"));
//app.Map("/new", () => Results.Text("New address"));

//app.Map("/", () => "Hello World");

//app.Run();

//  В данном случае при обращении по пути "/old" будет осуществляться редирект на адрес "/new". Поскольку второй параметр не задан,
//  то будет выполняться временная переадресация.
#endregion

#region Метод Redirect
//  Метод Redirect() также осуществляет переадресацию и принимает те же параметры, что и LocalRedirect(), только адрес для
//  переадресации может не только локальным, но и внешним:
//      public static IResult Redirect(string url, bool permanent = false, bool preserveMethod = false);

//  Пример работы:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/old", () => Results.Redirect("https://metanit.com"));
app.Map("/", () => Results.Text("Hello <3"));

app.Run();
#endregion

#region RedirectToRoute
//  Метод RedirectToRoute() выполняет переадресацию на определенный маршрут:
//      public static IResult RedirectToRoute (string? routeName = default, object? 
//          routeValues = default, bool permanent = false, bool preserveMethod = false, string? fragment = default);

//  Параметры метода:
//      routeName: название маршрута
//      routeValues: значения для параметров маршрута
//      permanent: указывает, будет ли переадресация постоянной (отправляется статусный код 301) или временной(отравляется статусный
//      код 302).
//      preserveMethod: если равен true, то сохраняется оригинальный метод HTTP-запроса.
//      fragment: фрагмент, который добавляется к адресу для переадресации
#endregion