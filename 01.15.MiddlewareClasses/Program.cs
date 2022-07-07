//  Классы middleware

//  В прошлых темах используемые компоненты middleware фактически представляли методы, то есть так называемые inline middleware.
//  Однако также ASP.NET Core позволяет определять middleware в виде отдельных классов.

//  Итак, добавим в проект новый класс, который назовем TokenMiddleware и который будет иметь следующий код:

//public class TokenMiddleware
//{
//    private readonly RequestDelegate next;

//    public TokenMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        var token = context.Request.Query["token"];
//        if (token != "12345678")
//        {
//            context.Response.StatusCode = 403;
//            await context.Response.WriteAsync("Token is invalid");
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}

//  Класс middleware должен иметь конструктор, который принимает параметр типа RequestDelegate. Через этот параметр можно получить
//  ссылку на тот делегат запроса, который стоит следующим в конвейере обработки запроса.

//  Также в классе должен быть определен метод, который должен называться либо Invoke, либо InvokeAsync. Причем этот метод должен
//  возвращать объект Task и принимать в качестве параметра контекст запроса - объект HttpContext. Данный метод собственно и будет
//  обрабатывать запрос.

//  Суть действия класса заключается в том, что мы получаем из запроса параметр "token". Если полученный токен равен строке "12345678",
//  то передаем запрос дальше следующему компоненту, вызвав метод _next.Invoke(). Иначе возвращаем пользователю сообщение об ошибке.

//  Для добавления компонента middleware, который представляет класс, в конвейер обработки запроса применяется метод UseMiddleware().
//  Так, изменим файл Program.cs следующим образом:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.UseMiddleware<TokenMiddleware>();

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello all");
//});
//app.Run();

//  С помощью метода UseMiddleware<T> в конструктор объекта TokenMiddleware будет внедряться объект для параметра RequestDelegate next.
//  Поэтому явным образом передавать значение для этого параметра нам не нужно.

//  Запустим проект.И если мы не передадим через строку запроса параметр token или передадим для него значение, отличное от "12345678",
//  то браузер отобразит ошибку. Если же будет передан корректный токен https://localhost:7097/?token=12345678, то вызов
//  app.UseMiddleware<TokenMiddleware>() передаст обработку запроса в компонент middleware из app.Run()

#region Метод расширения для встраивания middleware
//  Также нередко для встраивания подобных компонентов middleware определяются специальные методы расширения. Так, добавим в проект новый
//  класс, который назовем TokenExtensions:

//public static class TokenExtensions
//{
//    public static IApplicationBuilder UseToken(this IApplicationBuilder builder)
//    {
//        return builder.UseMiddleware<TokenMiddleware>();
//    }
//}

//  Здесь создается метод расширения для типа IApplicationBuilder. И этот метод встраивает компонент TokenMiddleware в конвейер обработки
//  запроса. Как правило, подобные методы возвращают объект IApplicationBuilder.

//  Теперь применим этот метод в коде программы в Program.cs:

//  var builder = WebApplication.CreateBuilder();
//  var app = builder.Build();

//  app.UseToken();

//  app.Run(async (context) => await context.Response.WriteAsync("Hello METANIT.COM"));

//  app.Run();
#endregion

#region Передача параметров
var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseToken("55555");

app.Run(async context =>
{
    await context.Response.WriteAsync("Hey all");
});
app.Run();

//  Изменим класс TokenMiddleware, чтобы он извне получал образец токена для сравнения:

public class TokenMiddleware
{
    private readonly RequestDelegate next;
    string pattern;
    public TokenMiddleware(RequestDelegate next, string pattern)
    {
        this.next = next;
        this.pattern = pattern; 
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"];
        if (token != pattern)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid Token");
        }
        else
        {
            await next.Invoke(context);
        }
    }
}

//  Образец токена, с которым идет сравнения, устанавливается через конструктор. Чтобы передать его в конструктор, изменим
//  класс TokenExtensions:

public static class TokenExtensions
{
    public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string pattern)
    {
        return builder.UseMiddleware<TokenMiddleware>(pattern);
    }
}

//  В метод builder.UseMiddleware можно передать набор значений, которые передаются в конструктор компонента middleware.
#endregion