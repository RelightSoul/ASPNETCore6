//  Аутентификация с помощью куки

//  Одним из распространенных способов аутентификации в веб-приложенях является аутентификация с помощью куки. И ASP.NET Core
//  имеет встроенную поддержку для данного типа аутентификации.

//  Для примения аутентификации с помощью куки в метод AddAuthentication() передается схема "Cookies":
//  builder.Services.AddAuthentication("Cookies")

//  Чтобы не ошибиться в написании схемы еще можно передавать константу CookieAuthenticationDefaults.AuthenticationScheme,
//  которая имеет то же самое значение.
//  builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

//  Кроме того, для настройки аутентификации с помщью куки необходимо вызвать метод AddCookie(), который реализован как метод
//  расширения для типа AuthenticationBuilder:
//  public static AuthenticationBuilder AddCookie(this AuthenticationBuilder builder,
//                                              Action<CookieAuthenticationOptions> configureOptions);

//  В качестве параметра метод принимает делегат, который с помощью объекта CookieAuthenticationOptions устанавливает
//  настройки аутентификации.

//  Рассмотрим на примере, как использовать самую простейшую аутентификацию с помощью куки. Для этого определим в файле
//  Program.cs следующий код:

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

// условная бд с пользователями
var people = new List<Person>
{
new Person("tom@gmail.com", "12345"),
new Person("bob@gmail.com", "55555")
};
// аутентификация с помощью куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 

app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    // html-форма для ввода логина/пароля
    string loginForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>Email</label><br />
                <input name='email' />
            </p>
            <p>
                <label>Password</label><br />
                <input type='password' name='password' />
            </p>
            <input type='submit' value='Login' />
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(loginForm);
});

app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
{
    // получаем из формы email и пароль
    var form = context.Request.Form;
    // если email и/или пароль не установлены, посылаем статусный код ошибки 400
    if (!form.ContainsKey("email") || !form.ContainsKey("password"))
        return Results.BadRequest("Email и/или пароль не установлены");

    string email = form["email"];
    string password = form["password"];

    // находим пользователя 
    Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
    // если пользователь не найден, отправляем статусный код 401
    if (person is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
    // создаем объект ClaimsIdentity
    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    // установка аутентификационных куки
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl ?? "/");
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.Map("/", [Authorize] () => $"Hello World!");

app.Run();

record class Person(string Email, string Password);

//  Для представления пользователя класс Person:
//  record class Person(string Email, string Password);

//  В качестве условной базы данных для теста используется список people:
//  var people = new List<Person>
//  {
//    new Person("tom@gmail.com", "12345"),
//    new Person("bob@gmail.com", "55555")
//  };

//  Именно с этими данными мы будем сравнивать присланные от клиента логин и пароль.

//  Для подключения аутентификации куки регистрируем соответствующие сервисы:
//  builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//      .AddCookie(options => options.LoginPath = "/login");

//  Свойство LoginPath класса CookieAuthenticationOptions указывает на путь, по которому неаутентифицированный клиент будет
//  автоматически переадресовываться при обращении к ресурсу, для доступа к которому требуется аутентификация.

//  Конечная точка, которая обрабатывает get-запросы по пути "/login", будет отпаравлять в ответ html-форму для ввода email и пароля:

//app.MapGet("/login", async (HttpContext context) =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    // html-форма для ввода логина/пароля
//    string loginForm = @"<!DOCTYPE html>
//    <html>
//    <head>
//        <meta charset='utf-8' />
//        <title>METANIT.COM</title>
//    </head>
//    <body>
//        <h2>Login Form</h2>
//        <form method='post'>
//            <p>
//                <label>Email</label><br />
//                <input name='email' />
//            </p>
//            <p>
//                <label>Password</label><br />
//                <input type='password' name='password' />
//            </p>
//            <input type='submit' value='Login' />
//        </form>
//    </body>
//    </html>";
//    await context.Response.WriteAsync(loginForm);
//});

//  В данном случае для простоты весь html-код определен в виде строки, но, естественно можно сделать по-разному, например,
//  определить отдельную html-страницу и ее отправлять отправлять клиенту.

//  На отправляемой форме клиент должен будет заполнить поля "email" и "password", и после нажатия на кнопку отправки значения
//  этих полей в запросе типа POST будут получены в обработчике другой конечной точки:
//  app.MapPost("/login", async (string? returnUrl, HttpContext context) => 

//  Обработчик этой конечной точки принимает два параметра. Прежде всего, это передаваемый через механизм внедрения зависимостей
//  объект контекста запоса HttpContext. Кроме того, система аутентификации автоматически отправляет путь, с которого пользователь
//  был переадресован на форму логина. Поскольку у нас адрес формы логина и адрес обработки отправленных данных совпадает с "/login",
//  то через параметр returnUrl мы можем получить путь, по которому изначально обращался клиент. Однако поскольку клиент также
//  может напрямую обратиться к форме логина, то в этом случае данный параметр будет иметь значение null.

//  В самом же обработчике конечной точки вначале получаем из данных формы отправленные email и пароль:

//var form = context.Request.Form;
//if (!form.ContainsKey("email") || !form.ContainsKey("password"))
//    return Results.BadRequest("Email и/или пароль не установлены");

//string email = form["email"];
//string password = form["password"];

//  Получив данные, проверяем, а есть ли объект с такими данными в нашей условной базе данных - списке people:

//Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
//if (person is null) return Results.Unauthorized();

//  Далее производится установка аутентификационных кук, которые будут применяться для определения клиента и его прав в приложении:

//var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
//// создаем объект ClaimsIdentity
//ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
//// установка аутентификационных куки
//await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

//Для установки кук у класса HttpContext применяется асинхронный метод SignInAsync(). В качестве параметра он принимает
//применяемую схему аутентификации, в нашем случае это "Cookies", то есть значение константы
//CookieAuthenticationDefaults.AuthenticationScheme. А в качестве второго параметра передается объект ClaimsPrincipal,
//который представляет пользователя.

//Для правильного создания и настройки объекта ClaimsPrincipal вначале создается список claims - набор объектов Claim - грубо
//говоря набор данных, которые описывают пользователя. Эти данные шифруются и добавляются в аутентификационные куки. Каждый
//такой claim принимает тип и значение. В нашем случае у нас только один claim, который в качестве типа принимает константу
//ClaimTypes.Name, а в качестве значения - email пользователя.

//Далее создается объект ClaimsIdentity, который нужен для инициализации ClaimsPrincipal. В ClaimsIdentity передается ранее
//созданный список claims и тип аутентификации, в данном случае "Cookies". Тип аутентификации может представлять произвольную строку.

//И после вызова метода сontext.SignInAsync будут формироваться аутентификационные куки, которые будут отправлены клиенту и
//при последующих запросах будут передаваться обратно на сервер, десериализоваться и использоваться для аутентификации пользователя.

//В самом конце перенаправляем аутентификацированного пользователя обратно на адрес, с которого его перебросило на форму логина:
//  return Results.Redirect(returnUrl??"/");

//  Для выхода из сайта определена конечная точка, которая обрабатывает запросы по пути "/logout":

//app.MapGet("/logout", async (HttpContext context) =>
//{
//    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//    return Results.Redirect("/login");
//});

//  Его основа - вызов метода context.SignOutAsync(), который удаляет аутентификационные куки. В качестве параметра он принимает
//  схему аутентификации.

//  Для тестирования авторизации определена четвертая конечная точка, которая обрабатывает запросы к корню приложения:
//  app.Map("/", [Authorize]() => $"Hello World!");
//  Поскольку эта конечная точка использует атрибут Authorize, то доступ к ней имеют только аутентифицированные пользователи.