//  Аутентификация и авторизация
//  Введение в аутентификацию и авторизацию

//  Важное место в приложении занимает аутентификация и авторизация. Аутентификация представляет процесс определения пользователя.
//  Авторизация представляет процесс определения, имеет ли пользователь право доступа к некоторому ресурсу. То есть, если аутентификация
//  отвечает на вопрос "Кем является пользователь?", то авторизация отвечает на вопрос "Какие права пользователь имеет в системе?"
//  ASP.NET Core имеет встроенную поддержку аутентификации и авторизации.

#region Аутентификация
//  Для выполнения аутентификации в конвейере обработки запроса отвечает специальный компонент middleware - AuthenticationMiddleware.
//  Для встраивания этого middleware в конвейер применяется метод расширения UseAuthentication()
//        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app);

//  Следует отметить, что метод UseAuthentication() должен встраиваться в конвейер до любых компонентов middleware, которые используют
//  аутентификацию пользователей.

//  Для выполнения аутентификации этот компонент использует сервисы аутентификации, в частности, сервис IAuthenticationService, которые
//  регистрируются в приложении с помощью метода AddAuthentication():
//  public static AuthenticationBuilder AddAuthentication(this IServiceCollection services)
//  public static AuthenticationBuilder AddAuthentication(this IServiceCollection services, string defaultScheme)
//  public static AuthenticationBuilder AddAuthentication(this IServiceCollection services, Action<AuthenticationOptions> configureOptions)

//  В качестве параметра вторая версия метода AddAuthentication() принимает схему аутентификации в виде строки. Третья версия метода
//  AddAuthentication принимает делегат, который устанавливает опции аутентификации - объект AuthenticationOptions.

//  Какую бы мы версию метода не использовали, для аутентификации необходима установить схему аутентификации. Две наиболее
//  расcпространенные схемы аутентификации:

//  "Cookies": аутентификация на основе куки. Хранится в константе CookieAuthenticationDefaults.AuthenticationScheme

//  "Bearer": аутентификация на основе jwt-токенов. Хранится в константе JwtBearerDefaults.AuthenticationScheme

//  Схема аутентификации позволяет выбирать определенный обработчик аутентификации. Обработчик аутентификации собственно и выполняет
//  непосредственную аутентификацию пользователей на основе данных запросов и исходя из схемы аутентификации.

//  Например, для аутентификации с помощью куки передается схема "Cookies". Соответственно для аутентификации пользователя будет
//  выбираться встроенный обработчик аутентификации - класс Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler,
//  который на основе полученных в запросе cookie выполняет аутентификацию.

//  А если используется схема "Bearer", то это значит, что для аутентификации будет использоваться jwt-токен, а в качестве обработчика
//  аутентификации будет применяться класс Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler. Стоит отметить, что для
//  аутентификации с помощью jwt-токенов необходимо добавить в проект через Nuget пакет Microsoft.AspNetCore.Authentication.JwtBearer

//  При чем в ASP.NET Core мы не ограничены встроенными схемами аутентификации и можем создавать свои кастомные схемы и под них своих
//  обработчиков аутентификации.

//  Кроме применения схемы аутентификации необходимо подключить аутентификацию определенного типа. Для этого можно использовать следуюшие
//  методы:

//  AddCookie(): подключает и конфигурирует аутентификацию с помощью куки.

//  AddJwtBearer(): подключает и конфигурирует аутентификацию с помощью jwt-токенов (для этого метода необходим Nuget-пакет
//  Microsoft.AspNetCore.Authentication.JwtBearer)

//  Оба метода реализованы как методы расширения для типа AuthenticationBuilder, который возвращается методом AddAuthentication():

//var builder = WebApplication.CreateBuilder();
//// добавление сервисов аутентификации
//builder.Services.AddAuthentication("Bearer")  // схема аутентификации - с помощью jwt-токенов
//    .AddJwtBearer();  // подключение аутентификации с помощью jwt-токенов

//var app = builder.Build();

//app.UseAuthentication();    // добавление middleware аутентификации
#endregion

#region Авторизация
//  Авторизация представляет процесс определения прав пользователя в системе, к каким ресурсам приложения он имеет право доступа и
//  при каких условиях.

//  Хотя авторизация представляет отдельный независимый процесс, тем не менее для нее также необходимо, чтобы приложение также
//  применяло аутентификацию.

//  Для подключения авторизации необходимо встроить компонент Microsoft.AspNetCore.Authorization.AuthorizationMiddleware. Для этого
//  применяется встроенный метод расширения UseAuthorization()
//      public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app)

//  Кроме того, для применения авторизации необходимо зарегистрировать сервисы авторизации с помощью метода AddAuthorization():
//      public static IServiceCollection AddAuthorization(this IServiceCollection services)
//      public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<AuthorizationOptions> configure)

//  Вторая версия метода принимает делегат, который с помощью параметра AuthorizationOptions позволяет сконфигурировать авторизацию.

//  Ключевым элементом механизма авторизации в ASP.NET Core является атрибут AuthorizeAttribute из пространства имен
//  Microsoft.AspNetCore.Authorization, который позволяет ограничить доступ к ресурсам приложения. Например:

using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/hello", [Authorize]() => "Hello man!");
app.Map("/", () => "Home Page");

app.Run();

//  Здесь в приложении определены две конечных точки: "/" и "/hello".При этом конечная точка "/hello" применяет атрибут Authorize.
//  Атрибут указывается перед обработчиком конечной точки.

//  Применение данного атрибута означает, что к конечной точке "/hello" имеют доступ только аутентифицированные пользователи.
#endregion