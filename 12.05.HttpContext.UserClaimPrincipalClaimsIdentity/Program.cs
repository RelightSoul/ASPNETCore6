//  HttpContext.User, ClaimPrincipal и ClaimsIdentity

#region Классы ClaimPrincipal и ClaimsIdentity и их роль в аутентификации
//  Одной из задач аутентификации в приложении ASP.NET Core является установка пользователя, который представлен в приложении
//  свойством User класса HttpContext:
//      public abstract System.Security.Claims.ClaimsPrincipal User { get; set; }

//  Данное свойство предоставляет класс ClaimsPrincipal из пространства имен System.Security.Claims

//  Непосредственно данные, которые идентифицируют пользователя (его идентичность) хранятся в свойстве Identity класса ClaimPrincipal:
//      public virtual System.Security.Principal.IIdentity? Identity { get; }

//  Это свойство представляет основную идентичность текущего пользователя. Но поскольку с одним пользователем может быть связан набор
//  идентичностей, то также в классе определено свойство Identities:
//      public virtual IEnumerable<ClaimsIdentity> Identities { get; }

//  Свойство Identity представляет интерфейс IIdentity, и, как правило, в качестве такой реализации применяется класс ClaimsIdentity.

//  Объект IIdentity, в свою очередь, предоставляет информацию о текущем пользователе через следующие свойства:
//  AuthenticationType: тип аутентификации в строковом виде
//  IsAuthenticated: возвращает true, если пользователь аутентифицирован
//  Name: возвращает имя пользователя. Обычно в качестве подобного имени используется логин, по которому пользователь входит в приложение

//  Для создания объекта ClaimsIdentity можно применять ряд конструкторов, но, для того, чтобы пользователь был аутентифицирован,
//  необходимо, как минимум, предоставить тип аутентификации, которая передается через конструктор. Тип аутентификации представляет
//  произвольную строку, которая описывает некоторым образом способ аутентификации. Например:
//      var identity = new ClaimsIdentity("Cookies");

//  В данном случае в тип аутентификации называется "Cookies".

//  Для установки идентичности пользователя объект ClaimsIdentity можно передать в ClaimsPrincipal либо через конструктор, либо
//  через метод AddIdetity():

//      var identity = new ClaimsIdentity("Undefined");
//      var principal = new ClaimsPrincipal(identity);

//  На примере аутентификации куки посмотрим на применение ClaimsPrincipal и ClaimsIdentity:

using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

// аутентификация с помощью куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/login", async (HttpContext context) =>
{
    var claimsIdentity = new ClaimsIdentity("Undefined");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    // установка аутентификационных куки
    await context.SignInAsync(claimsPrincipal);
    return Results.Redirect("/");
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return "Данные удалены";
});
app.Map("/", (HttpContext context) =>
{
    var user = context.User.Identity;
    if (user is not null && user.IsAuthenticated)
    {
        return $"Пользователь аутентифицирован. Тип аутентификации: {user.AuthenticationType}";
    }
    else
    {
        return "Пользователь НЕ аутентифицирован";
    }
});

app.Run();

//  Здесь в конечной точке app.MapGet("/login") создается идентичность claimsIdentity - объект ClaimsIdentity с типом аутентификации
//  "Undefined". Далее создается объект ClaimsPrincipal, который принимает идентичность claimsIdentity. Созданный объект claimsPrincipal
//  затем передается в метод context.SignInAsync(), который, используя этот объект, устанавливает аутентификационные куки. И в конце
//  происходит редирект на путь "/".

//  Конечная точка app.Map("/") получает через механизм внедрения зависимостей текущего пользователя через свойство context.User.
//  Фактически это тот самый объект ClaimsPrincipal, созданный выше и сохраненный в куках. И когда приходит запрос к приложению,
//  инфраструктура ASP.NET Core дешифрует и десериализует данные запроса и создает по ним объект ClaimsPrincipal, который хранится
//  в свойстве context.User. Если используется аутентификация на основе куки (как в примере выше), то данные о пользователе будут
//  извлекаться из аутентификационных кук. Если применяются jwt-токены, то данные берутся из полученного токена. Причем даже если
//  аутентификационных куки или токена в запросе нет, то объект ClaimsPrincipal все равно будет создаваться.

//  Получив идентичность пользователя, мы можем получить различную информацию о нем. Например, проверить, аутентифирован ли он,
//  получить тип аутентификации, получить другую связанную с ним информацию.

//  Таким образом, при первом обращении к приложению, когда у нас не установлено никаких аутентификационных кук, пользователь
//  из context.User не аутентифицирован. Но после перехода по пути "/login" будут созданы объекты ClaimsPrincipal и ClaimsIdentity
//  и по ним будут установлены аутентификационные куки. Соответственно при повторном переходе по пути "/" пользователь будет
//  аутентифицирован
#endregion

#region Получение ClaimsPrincipal
//  Поскольку объект HttpContext доступен через механизм внедрения зависимостей в любой точке приложения, то мы можем через этот объект
//  получить пользователя, как в примере выше. Однако, если нам нужно только свойство User, а не весь объект HttpContext, то мы можем
//  также через механизм внедрения зависимостей получить сервис ClaimsPrincipal, который будет аналогичен свойству context.User

//app.Map("/", (ClaimsPrincipal claimsPrincipal) =>
//{
//    var user = claimsPrincipal.Identity;
//    if (user is not null && user.IsAuthenticated)
//        return "Пользователь аутентифицирован";
//    else return "Пользователь не аутентифицирован";
//});
#endregion