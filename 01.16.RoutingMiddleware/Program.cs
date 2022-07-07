//  Построение конвейера обработки запроса

//  Обычно для обработки запроса применяется не один, а несколько компонентов middleware. И в этом случае большую роль может играть
//  порядок их помещения в конвейер обработки запроса, а также то, как они взаимодействуют с другими компонентами.

//  Кроме того, каждый компонент middleware может обрабатывать запрос до и после последующих в конвейере компонентов. Данное
//  обстоятельство позволяет предыдущим компонентам корректировать результат обработки последующих компонентов.

//  Рассмотрим простейший пример. Определим следующий класс RoutingMiddleware для обработки запроса:

//public class RoutingMiddleware
//{
//    readonly RequestDelegate next;
//    public RoutingMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        string path = context.Request.Path;
//        if (path == "/index")
//        {
//            await context.Response.WriteAsync("Index Page");
//        }
//        if (path == "/about")
//        {
//            await context.Response.WriteAsync("About Page");
//        }
//        else
//        {
//            context.Response.StatusCode = 404;
//            await context.Response.WriteAsync("Not found");
//        }
//    }
//}

//  Этот компонент в зависимости от строки запроса возвращает либо определенную строку, либо устанавливает код ошибки.

//  Допустим, мы хотим, чтобы пользователь был аутентифицирован при обращении к нашему приложению. Для этого добавим новый класс
//  AuthenticationMiddleware:

//public class AuthenticationMiddleware
//{
//    readonly RequestDelegate next;
//    public AuthenticationMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        var token = context.Request.Query["token"];
//        if (string.IsNullOrWhiteSpace(token))
//        {
//            context.Response.StatusCode = 403;
//        }
//        else
//        {
//            await next.Invoke(context);
//        }
//    }
//}

//  Условно будем считать, что если в строке запроса есть параметр token и он имеет какое-нибудь значение, то пользователь
//  аутентифицирован. А если он не аутентифицирован, то надо необходимо ограничить доступ пользователям к приложению. Если
//  пользователь не аутентифицирован, то устанавливаем статусный код 403, иначе передаем выполнение запроса следующему в
//  конвейере делегату.

//  Поскольку компоненту RoutingMiddleware нет смысла обрабатывать запрос, если пользователь не аутентифицирован, то в конвейере
//  компонент AuthenticationMiddleware должен быть помещен перед компонентом RoutingMiddleware:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<RoutingMiddleware>();

app.Run();

//  Таким образом, если мы сейчас запустим проект и обратимся по пути "/index" или "/about" и не передадим параметр token, то мы
//  получим ошибку. Если же обратимся по пути /index или /about и передадим значение параметра token, то увидим искомый текст
//  https://localhost:7297/about?token=33424   // About Page 
//  https://localhost:7297/about               // Access Denied
//  https://localhost:7297/main?token=33424    // Not found

//  Добавим еще один компонент middleware, который назовем ErrorHandlingMiddleware:

public class ErrorHandlingMiddleware
{
    readonly RequestDelegate next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        await next.Invoke(context);
        if (context.Response.StatusCode == 403)
        {
            await context.Response.WriteAsync("Access Denied");
        }
        else if (context.Response.StatusCode == 404)
        {
            await context.Response.WriteAsync("Not Found");
        }
    }
}
//  В отличие от предыдущих двух компонентов ErrorHandlingMiddleware сначала передает запрос на выполнение последующим делегатам,
//  а потом уже сам обрабатывает. Это возможно, поскольку каждый компонент обрабатывает запрос два раза: вначале вызывается та часть
//  кода, которая идет до await next.Invoke(context);, а после завершения обработки последующих компонентов вызывается та часть кода,
//  которая идет после await next.Invoke(context);. И в данном случае для ErrorHandlingMiddleware важен результат обработки запроса
//  последующими компонентами. В частности, он устанавливает сообщения об ошибках в зависимости от того, как статусный код установили
//  другие компоненты. Поэтому ErrorHandlingMiddleware должен быть помещен первым из всех трех компонентов

#region Конец кода
public class RoutingMiddleware
{
    readonly RequestDelegate next;
    public RoutingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;
        if (path == "/index")
        {
            await context.Response.WriteAsync("Index Page");
        }
        if (path == "/about")
        {
            await context.Response.WriteAsync("About Page");
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }
}
public class AuthenticationMiddleware
{
    readonly RequestDelegate next;
    public AuthenticationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = 403;
        }
        else
        {
            await next.Invoke(context);
        }
    }
}
#endregion